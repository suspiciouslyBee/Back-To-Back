using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
  [SerializeField] private GameObject hitboxPrefab; // The Hitbox the weapon deals damage through
  [SerializeField] private int totalUsesPerReload;  // The total attacks the weapon can do before a realod (ammo)
  private int remainingUses;                        // The number of attacks left before a reload is required

  public AudioSource audioSource;
  [SerializeField] private AudioClip useSFX;
  [SerializeField] private AudioClip cantUseSFX;
  [SerializeField] private AudioClip reloadSFX;

  bool canAttack;                                 // Stop the player from attacking or possibly swapping under certain states
  [SerializeField] float attackCooldown;          // Time between attacks



  public enum weaponType
  {
    ranged,
    melee
  }

  public weaponType type;                             // "melee" or "range"?

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    Reload();
    canAttack = true;
  }
  //------------------------- Getters -------------------------
  public int GetCurrentUses()
  {
    /*
    Get the current "Ammo" left

    Inputs:
      * None

    Output:
      * Returns the number of attack left until a reload is required. Returns -1 if the weapon does not use ammo
    */
    return remainingUses;
  }

  public int GetTotalUses()
  {
    /*
    Get the total "Ammo" left

    Inputs:
      * None

    Output:
      * Returns the number of attack per reload. Returns -1 if the weapon does not use ammo
    */
    return totalUsesPerReload;
  }
  //------------------------- Setters -------------------------
  public bool Reload()
  {
    /*
    Reset the remaing uses to the maximum uses.

    Inputs:
      * None

    Output:
      * Returns false if totalUsesPerReload is -1 (a melee weapon)
    */

    // Melee weapon
    if (totalUsesPerReload < 0)
    {
      return false;
    }
    // Ranged weapon
    else
    {
      remainingUses = totalUsesPerReload;
      return true;
    }
  }

  //------------------------- Actions -------------------------
  public bool DoAttack()
  {
    /*
    If there are reaming uses, creates a hitbox and decrements the remaing uses.

    Inputs:
      * None

    Output:
      * Returns false if the weapon is ranged and has no more remaing uses
    */
    bool hasAmmo = remainingUses > 0;
    if (canAttack)
    {
      StartCoroutine(AttackTimer());
      //bool hasAmmo = curWeapon.DoAttack();
      if (type == weaponType.ranged)
      {
        HUDManager.Instance.ChangeBars(2, !hasAmmo);
      }
      //return hasAmmo;
    }

    // If the weapon is ranged and out of ammo, return false
    if (totalUsesPerReload > 0 && remainingUses <= 0)
    {
      return false;
    }
    // Decrement the remaing uses if it's a ranged weapon
    else if (totalUsesPerReload > 0)
    {
      remainingUses--;
    }

    // Spawn a hit box
    SpawnHitBox();
    if (audioSource != null)
    {
      audioSource.PlayOneShot(useSFX);
    }
    return true;
  }

  // The delay between attacks
  IEnumerator AttackTimer()
  {
    canAttack = false;
    yield return new WaitForSeconds(attackCooldown);
    canAttack = true;
  }

  //------------------------- Helpers -------------------------
  private void SpawnHitBox()
  {
    /*
    Creates a instance of the hit box and applies any momentum it should have

    Inputs:
      * None

    Output:
      * None
    */

    // The position and rotation of the hitbox
    Vector3 hitboxPos = transform.position;
    Quaternion hitboxRot = transform.rotation;

    // Find Parent's direction
    float direction = this.transform.parent.localScale.x;
    direction = direction / Mathf.Abs(direction);

    // Create the object
    GameObject myHitbox = Instantiate(hitboxPrefab, hitboxPos, hitboxRot);
    myHitbox.transform.localScale = new Vector2(myHitbox.transform.localScale.y * direction, myHitbox.transform.localScale.y);

    // Get the hitbox's velocity and see if a force needs to be applied
    float hitboxVelocity = hitboxPrefab.GetComponent<WeaponHitBox>().GetVelocity();
    if (hitboxVelocity > 0)
    {
      float Theta = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
      float weaponXVel = Mathf.Cos(Theta) * hitboxVelocity * direction;
      float weaponYVel = Mathf.Sin(Theta) * hitboxVelocity;
      myHitbox.GetComponent<Rigidbody2D>().AddForce(new Vector3(weaponXVel, weaponYVel, 0));
    }
  }
}
