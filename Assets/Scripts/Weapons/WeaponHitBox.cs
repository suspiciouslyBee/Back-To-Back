using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
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
  [SerializeField] private float knockback;     // the force applied to an enemy when hit
  [SerializeField] private float bonusMultiplyer;
  private GameObject owner;

  private float timeAlive;

  //private HashSet<GameObject> piercedEnemies = new HashSet<GameObject>();

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

  public void AssignOwner(GameObject player, bool isBonus)
  {
    /*
    Assign the owner of the hitbox to the player that creates it

    Inputs:
      * The player that creates the hitbox

    Output:
      * None
    */

    owner = player;
    if (isBonus)
    {
      damage *= bonusMultiplyer;
    }
  }

  //------------------------- Actions -------------------------
  void Update()
  {
    // Increase the time alive, then if the time alive is longer than the lifetime, destroy the object
    timeAlive += Time.deltaTime;
    if (timeAlive >= lifeTime)
    {
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    // Hitbox hit an enemy
    if (other.CompareTag("Enemy"))
    {
      // Deal damage to the enemy
      Enemy enemyScript = other.GetComponent<Enemy>();
      float direction = Mathf.Abs(transform.localScale.x) / transform.localScale.x;
      Vector2 force = new Vector2(knockback * direction, knockback / 10);
      enemyScript.ApplyForce(force);
      bool didKill = enemyScript.ChangeHP(-1 * damage);
      // Reduce the amount of pierce (Currently 1. If we add Enemies with more "defense", this would be set by that value)
      bool isPierceLeft;
      isPierceLeft = ReducePierce(1);
      // If there is no pierce left, destroy the hitbox
      if (!isPierceLeft)
      {
        Destroy(gameObject);
      }
      // Apply exp
      if (didKill)
      {
        int exp = enemyScript.GetExpReward();
        if (owner.transform.parent.gameObject != null)
        {
          owner.transform.parent.gameObject.GetComponent<UpgradeManager>().AddExpFromPlayer(exp, owner);
        }

      }
    }
  }
}
