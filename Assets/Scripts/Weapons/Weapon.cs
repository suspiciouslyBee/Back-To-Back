using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField] private GameObject hitboxPrefab; // The Hitbox the weapon deals damage through
  [SerializeField] private int totalUsesPerReload;  // The total attacks the weapon can do before a realod (ammo)
  private int remainingUses;                        // The number of attacks left before a reload is required

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    Reload();
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
    return true;
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

    // Create the object
    GameObject myHitbox = Instantiate(hitboxPrefab, hitboxPos, hitboxRot);

    // Get the hitbox's velocity and see if a force needs to be applied
    float hitboxVelocity = hitboxPrefab.GetComponent<WeaponHitBox>().GetVelocity();
    if (hitboxVelocity > 0)
    {
      float Theta = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
      float weaponXVel = Mathf.Cos(Theta) * hitboxVelocity * transform.localScale.x;
      float weaponYVel = Mathf.Sin(Theta) * hitboxVelocity;
      myHitbox.GetComponent<Rigidbody2D>().AddForce(new Vector3(weaponXVel, weaponYVel, 0));
    }
  }
}
