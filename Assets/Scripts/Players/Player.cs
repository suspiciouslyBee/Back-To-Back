using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Weapon curWeapon;              // Current weapon prefab
    Transform curPosition;                          // Player's current position
    public string type;                             // "melee" or "range"?
    float health;                                   // Player health
    float experience;                               // For when we add experience and weapon drops
    float iframes;                                  // How long the player has iframes
    bool invulnrable;                               // Does the player have iframes
    bool dead;                                      // Is the player dead

    // Set variables
    void Start()
    {
        health = 10f;
        iframes = 3.0f;
        invulnrable = false;
        dead = false;
        curPosition = gameObject.transform; 
        if (curPosition.position.x <= 0)
        {
            curWeapon.transform.position = new Vector2(curPosition.position.x - 0.5f, curPosition.position.y);
        }
        else
        {
            curWeapon.transform.position = new Vector2(curPosition.position.x + 0.5f, curPosition.position.y);
        }
    }

    // Switches which way the player is facing and which side they are on.
    public bool Swap()
    {
        curPosition.position = new Vector2(curPosition.position.x * -1, curPosition.position.y);
        curWeapon.transform.position = new Vector2(curPosition.position.x * -1, curPosition.position.y);
        // LookAt( curPosition + curTransform.forward ); ?
        return true;
    }

    // Attacks with curWeapon, currently bool if we want to check if the weapon was used.
    public bool UseWeapon()
    {
        curWeapon.DoAttack();
        return true;
        // return curWeapon.DoAttack();
    }

    // Reloads curWeapon, currently bool if we need to eventually check if the weapon was reloaded properly
    public bool Reload()
    {
        curWeapon.Reload();
        return true;
        // return curWeapon.Reload();
    }

    // For when we introduce item drops.
    public void SwitchWeapon(Weapon newWeapon)
    {
        if (newWeapon != null)
        {
            Destroy(curWeapon);
            curWeapon = newWeapon;
        }
    }

    // Function for the enemies to call so the players can take damage and possibly die
    public void Hurt(int damage)
    {
        if (!invulnrable && !dead)
        {
            health -= damage;
            StartCoroutine(IFrameTimer());
            if (health <= 0)
            {
                Die();
            }
        }
    }

    // Eventual function for when we add healpacks
    public void Heal(int healAmount)
    {
        health += healAmount;
    }

    // Function called by the hurt function to properly kill the player when their health is low enough. 
    // There is a dead variable incase we want some sort of revive function, animation, or-
    // just don't want to delete the player right away.
    public void Die()
    {
        dead = true;
        Destroy(gameObject);
    }

    // Function called by the hurt function to give the player a few seconds of immunity
    IEnumerator IFrameTimer()
    {
        invulnrable = true;
        yield return new WaitForSeconds(iframes);
        invulnrable = false;
    }
}
