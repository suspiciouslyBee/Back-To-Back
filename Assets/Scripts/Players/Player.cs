using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    Transform curPosition;                                  // Player's current position
    SpriteRenderer sR;

    [SerializeField] Sprite normalOutfit;
    [SerializeField] Sprite bonusOutfit;

    [SerializeField] Weapon startingWeapon;                 // Basic weapon prefab
    [SerializeField] Weapon curWeapon;                      // Current weapon

    [SerializeField] Ability fAPrefab;
    [SerializeField] Ability sAPrefab;
    Ability firstAbility;                                   // First ability
    Ability secondAbility;                                  // Second ability

    [SerializeField] ParticleSystem hurtParticles;
    [SerializeField] ParticleSystem upgradeParticles;
    [SerializeField] float maxHealth;                       // Max player health
    [SerializeField] float health;                          // Current player health

    [SerializeField] float attackCooldown;                  // Time between attacks
    [SerializeField] float reloadCooldown;                  // Time between attacks

    public bool left;                                       // To keep track of which side the players are on
    public string type;                                     // "melee" or "range"?
    bool canAttack;                                         // Stop the player from attacking or possibly swapping under certain states
    public bool acting;                                     // Stop the player from performing actions while using an ability
    float iframes;                                          // How long the player has iframes
    bool invulnrable;                                       // Does the player have iframes
    public bool dead;                                       // Is the player dead
    public bool bonus;

    // [SerializeField] private float autoHealTime = 10f;      // time before players start healing
    // [SerializeField] private int autoHealAmt = 1;           

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip dieSFX;

    // Set variables
    void Start()
    {
        health = maxHealth;
        iframes = 1.5f;
        invulnrable = false;
        canAttack = true;
        dead = false;
        sR = gameObject.GetComponent<SpriteRenderer>();
        SetSprite(false);
        curPosition = gameObject.transform;
        Weapon startingWeapon = GetComponent<UpgradePathTracker>().GetCurWeapon().GetComponent<Weapon>();
        curWeapon = Instantiate(startingWeapon, gameObject.transform);
        attackCooldown = curWeapon.GetUseTime();
        reloadCooldown = curWeapon.GetReloadTime();
        curWeapon.transform.position = new Vector2(curPosition.position.x + 0.7f, curPosition.position.y);
        if (fAPrefab != null)
        {
            firstAbility = Instantiate(fAPrefab, gameObject.transform);
        }
        if (sAPrefab != null)
        {
            secondAbility = Instantiate(sAPrefab, gameObject.transform);
        }
        changeDirection(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Switches which way the player is facing and which side they are on.
    public bool Swap(bool solo)
    {
        if (!solo)
        {
            curPosition.position = new Vector2(curPosition.position.x * -1, curPosition.position.y);
        }
        changeDirection(true);
        return true;
    }

    // Helper function to swap to help control the sprite direction
    public void changeDirection(bool inGame)
    {
        if (inGame)
        {
            left = !left;
        }
        if (left)
        {
            curPosition.localScale = new Vector2(-1, curPosition.localScale.y);
            //curWeapon.transform.localScale = new Vector2(-1, curWeapon.transform.localScale.y);
        }
        else
        {
            curPosition.localScale = new Vector2(1, curPosition.localScale.y);
            //curWeapon.transform.localScale = new Vector2(1, curWeapon.transform.localScale.y);
        }
    }

    // Attacks with curWeapon, currently bool if we want to check if the weapon was used.
    public (bool, bool) UseWeapon()
    {
        if (canAttack && !acting)
        {
            StartCoroutine(AttackTimer());
            bool hasAmmo = curWeapon.DoAttack(bonus);
            if (type == "ranged")
            {
                HUDManager.Instance.ChangeBars(2, false);
            }
            if (hasAmmo && bonus)
            {
                SetSprite(false);
            }
            return (true, hasAmmo);
        }
        else if (canAttack)
        {
            if (type == "ranged")
            {
                HUDManager.Instance.ChangeBars(2.5f, true);
            }
        }
        //curWeapon.CantUseWeaponFX();
        return (false, false);
    }

    // Calls the abilities
    public bool CallAbility(bool first)
    {
        if (!acting)
        {
            if (first && firstAbility != null)
            {
                return firstAbility.UseAbility();
            }
            else if (secondAbility != null)
            {
                return secondAbility.UseAbility();
            }
        }
        return false;
    }

    // Accessor for action logic
    public void PerformingAction(float actionLength, int actionType = 0)
    {
        StartCoroutine(ActionTimer(actionLength, actionType));
    }

    // Reloads curWeapon, currently bool if we need to eventually check if the weapon was reloaded properly
    public bool Reload()
    {
        if (!acting)
        {
            bool check = curWeapon.Reload();
            if (check)
            {
                PerformingAction(reloadCooldown, 1);
            }
            else
            {
                HUDManager.Instance.ChangeBars(2.5f, false);
            }
            return check;
        }
        return false;
    }

    // Switches the weapon when enough exp is gained
    public void SwitchWeapon(GameObject newWeapon)
    {
        if (newWeapon != null)
        {
            ParticleSystem xP = Instantiate(upgradeParticles, gameObject.transform);
            StartCoroutine(PlayParticles(xP));
            newWeapon = Instantiate(newWeapon, curWeapon.gameObject.transform.position, curWeapon.gameObject.transform.rotation, gameObject.transform);
            Destroy(curWeapon.gameObject);
            curWeapon = newWeapon.GetComponent<Weapon>();
            attackCooldown = curWeapon.GetUseTime();
            reloadCooldown = curWeapon.GetReloadTime();
            if (type == "ranged")
            {
                HUDManager.Instance.ChangeBars(2, false);
            }
        }
    }

    // Whenever an enemy touches the player, the have the enemy call the attack function
    // which will call the player's hurt function.
    // Could be make a OnCollisionEnter if we fully implement hitboxes for the enemies
    // which could hopefully reduce how much the players are checking collisions.
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !invulnrable)
        {
            col.gameObject.GetComponent<Enemy>().Attack(gameObject);
            StartCoroutine(IFrameTimer());
        }
    }

    // Function for the enemies to call so the players can take damage and possibly die
    public void Hurt(int damage)
    {
        if (!invulnrable && !dead)
        {
            health -= damage;
            ParticleSystem hP = Instantiate(hurtParticles, gameObject.transform);
            hP.gameObject.transform.localScale = transform.localScale;
            StartCoroutine(PlayParticles(hP));
            StartCoroutine(IFrameTimer());
            HUDManager.Instance.ChangeBars(1, false);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    // Eventual function for when we add healpacks
    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    // Function called by the hurt function to properly kill the player when their health is low enough. 
    // There is a dead variable incase we want some sort of revive function, animation, or-
    // just don't want to delete the player right away.
    public void Die()
    {
        dead = true;
        PlayerManager.Instance.UpdatePlayerCount();
        Destroy(gameObject);
    }

    // Function called by the hurt function to give the player a few seconds of immunity
    IEnumerator IFrameTimer()
    {
        invulnrable = true;
        yield return new WaitForSeconds(iframes);
        invulnrable = false;
    }

    // The delay between attacks
    IEnumerator AttackTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ActionTimer(float actionLength, int actionType)
    {
        acting = true;
        Color c = curWeapon.gameObject.GetComponent<SpriteRenderer>().color;
        curWeapon.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.55f, 0.55f);
        yield return new WaitForSeconds(actionLength);
        switch (actionType)
        {
            // Changing reload hud
            case 1:
                HUDManager.Instance.ChangeBars(2, false);
                break;
        }
        curWeapon.gameObject.GetComponent<SpriteRenderer>().color = c;
        acting = false;
    }

    // Returns the max and cur health stats
    public (float, float) GetHealthInfo()
    {
        return (maxHealth, health);
    }

    // Returns the max and cur ammo stats
    public (float, float) GetAmmoInfo()
    {
        return (curWeapon.GetTotalUses(), curWeapon.GetRemainingUses());
    }

    public (bool, bool) GetAbilityInfo()
    {
        (bool, bool) abilityInfo = (false, false);
        if (firstAbility != null)
        {
            abilityInfo.Item1 = firstAbility.GetCanUse();
        }
        if (secondAbility != null)
        {
            abilityInfo.Item2 = secondAbility.GetCanUse();
        }
        return abilityInfo;
        //return (firstAbility.GetCanUse(), secondAbility.GetCanUse());
    }

    public void SetAbility(Ability a, bool first)
    {
        if (first)
        {
            firstAbility = Instantiate(a, gameObject.transform);
        }
        else
        {
            secondAbility = Instantiate(a, gameObject.transform);
        }
    }

    public void SetSprite(bool outfit)
    {
        if (!outfit)
        {
            sR.sprite = normalOutfit;
            bonus = false;
        }
        else
        {
            sR.sprite = bonusOutfit;
            bonus = true;
        }
    }

    IEnumerator PlayParticles(ParticleSystem pS)
    {
        pS.Play();
        yield return new WaitForSeconds(pS.main.duration);
        Destroy(pS.gameObject);
    }
}
