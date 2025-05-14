using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
  [SerializeField] private GameObject hitboxPrefab; // The Hitbox the weapon deals damage through
  [SerializeField] private float useTime;
  [SerializeField] private float reloadTime;
  [SerializeField] private int totalUsesPerReload;  // The total attacks the weapon can do before a realod (ammo)
  public int remainingUses;                        // The number of attacks left before a reload is required


  [SerializeField] AudioSource audioSource;
  [SerializeField] AudioClip useSFX;              // sound to play when used
  [SerializeField] AudioClip cantUseSFX;          // sound to play when can't be used for some reason
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    Reload();
    audioSource = GetComponent<AudioSource>();
    audioSource.playOnAwake = false;
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

  public float GetUseTime()
  {
    /*
    Get the use time of the weapon

    Inputs:
      * None

    Output:
      * Returns the use time for the weapon.
    */
    return useTime;
  }

  public float GetReloadTime()
  {
    /*
    Get the reload time of the weapon

    Inputs:
      * None

    Output:
      * Returns the reload time for the weapon.
    */
    return reloadTime;
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
        or if the remainingUses is equal to totalUsesPerRelod 
    */

    // Melee weapon
    if ((totalUsesPerReload < 0) || (remainingUses == totalUsesPerReload))
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
  public bool DoAttack(bool isBonus)
  {
    /*
    If there are reaming uses, creates a hitbox and decrements the remaing uses.

    Inputs:
      * None

    Output:
      * Returns false if the weapon is ranged and has no more remaing uses
    */

    // If the weapon is ranged and out of ammo, return false
    if (totalUsesPerReload > 0 && remainingUses <= 0)
    {
      CantUseWeaponFX();
      return false;
    }
    // Decrement the remaing uses if it's a ranged weapon
    else if (totalUsesPerReload > 0)
    {
      remainingUses--;
    }
    audioSource.PlayOneShot(useSFX);
    // Spawn a hit box
    SpawnHitBox(isBonus);
    return true;
  }

  //------------------------- Helpers -------------------------
  private void SpawnHitBox(bool isBonus)
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

    // Assign the player to the hit box (For exp assignment on enemy kill)
    myHitbox.GetComponent<WeaponHitBox>().AssignOwner(transform.parent.gameObject, isBonus);
  }


  // "getter" method that plays the "can't use"
  // sound effect. for use by the Player class.
  public void CantUseWeaponFX()
  {
    audioSource.PlayOneShot(cantUseSFX);
  }
}
