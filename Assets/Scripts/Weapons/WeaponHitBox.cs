using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WeaponHitBox : MonoBehaviour
{
  // Weapon hitbox attributes
  [SerializeField] private float damage;      // The damage the weapon does
  [SerializeField] private float lifeTime;    // How long the weapon hitbox lasts for
  [SerializeField] private int pierceCount;   // How many enemies can the hitbox touch before destroying itself
  [SerializeField] private float velocity;    // How quickly does the hitbox move (0 for melee hitbox)

  private float timeAlive;

  //------------------------- Getters -------------------------
  public float GetDamage()
  {
    /*
    The damage this hit box deals.

    Inputs:
      * None

    Output:
      * The Damage that this hit box does
    */
    return damage;
  }

  public float GetLifeTime()
  {
    /*
    How long the hitbox should last in seconds.

    Inputs:
      * None

    Output:
      * The hit box life time.
    */
    return lifeTime;
  }

  public float GetPierce()
  {
    /*
    The number of enemies that this hitbox can still collide with before being deleted

    Inputs:
      * None

    Output:
      * The number of enemies that can still be touched
    */
    return pierceCount;
  }

  public float GetVelocity()
  {
    /*
    How quickly the hitbox should move. Should be set to 0 if it's a melee hitbox.

    Inputs:
      * None

    Output:
      * How quickly the hitbox moves
    */
    return velocity;
  }

  //------------------------- Setters -------------------------
  public bool ReducePierce(int amount)
  {
    /*
    Reduce the Pierce Count by the passed in amount. This should be called once per enemy hit. If the enemy takes up multiple peirce, pass in the amount it takes (Example: A tank might take up 2 pierce)

    Inputs:
      * Amount of pierce the enemy takes up

    Output:
      * If there is any pierce remianing (False if no more pierce left)
    */

    // Reduce the pierce
    pierceCount -= amount;

    // If there is any pierce left, return true
    if (pierceCount > 0)
    {
      return true;
    }

    // If there is no pierce left, return false
    return false;
  }

  //------------------------- Actions -------------------------
  void Update()
  {
    timeAlive += Time.deltaTime;
    if (timeAlive >= lifeTime)
    {
      Destroy(gameObject);
    }
  }
}
