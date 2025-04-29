using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    Weapon curWeapon;
    public string type;
    float health;
    float experience;               // For when we add experience and weapon drops
    bool iframes;
    bool dead;

    // Set variables
    void Start()
    {
        health = 10f;
        iframes = false;
        dead = false;
    }

    // Switches which way the player is facing and which side they are on.
    public bool Swap()
    {
        // LookAt( curPosition + curTransform.forward ); ?
        return true;
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

    // Reloads curWeapon, currently bool if we need to eventually check if the weapon was reloaded properly
    public bool Reload()
    {
        curWeapon.Reload();
        return true;
        // return curWeapon.Reload();
    }

    // Attacks with curWeapon, currently bool if we want to check if the weapon was used.
    public bool UseWeapon()
    {
        curWeapon.DoAttack();
        return true;
        // return curWeapon.DoAttack();
    }

    // Function for the enemies to call so the players can take damage and possibly die
    public void Hurt(int damage)
    {
        if (!iframes && !dead)
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
    // There is a dead variable incase we want some sort of revive function, animation, or don't want to delete the player right away.
    public void Die()
    {
        dead = true;
        Destroy(gameObject);
    }

    // Function called by the hurt function to give the player a few seconds of immunity
    IEnumerator IFrameTimer()
    {
        iframes = true;
        yield return new WaitForSeconds(3.0f);
        iframes = false;
    }
}
