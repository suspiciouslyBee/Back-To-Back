using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class Enemy : MonoBehaviour
{

    [SerializeField] protected Vector2 destination;


    // protected values
    protected float curHP;
    [SerializeField] protected float maxHP;

    [SerializeField] protected float speed;

    [SerializeField] protected float jumpPower;
    [SerializeField] protected float knockbackMult = 1;
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected int expReward;
    [SerializeField] protected bool grounded;
    [SerializeField] protected float minStunWaitTime;
    protected bool isStunned;
    [SerializeField] float iframes;                                  // How long the enemy has iframes

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip dieSFX;


    bool invulnrable;                               // Does the enemy have iframes

    protected Rigidbody2D rb;

    protected LayerMask ground;

    // controls everything of how the enemy works
    public abstract void AI();              // controls the behavior of this enemy type

    // spawning mechanics
    public abstract void Spawn();           //  initialize a new instance

    // Attack functionality
    public abstract void Attack(GameObject player);          //  Peform the attack that this entity can hold

    public abstract void MoveToPoint(Vector2 pos);  // attempt to move towards a destination

    public abstract void Jump();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = LayerMask.GetMask("terrain");
        curHP = maxHP;
        isStunned = false;
        invulnrable = false;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void MoveToDestination()
    {
        if (!isStunned)
        {
            MoveToPoint(destination);
        }
    }


    // check if Enemy is touching the ground
    public void CheckIfGrounded()
    {
        if (hitbox != null && hitbox.IsTouchingLayers(ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    // damage should be passed as a negative value
    // heals/damages the enemy instance
    public bool ChangeHP(float amount)
    {
        if (!invulnrable)
        {

            StartCoroutine(IFrameTimer());
            curHP += amount;
            if (amount < 0 && curHP > 0)
            {
                audioSource.PlayOneShot(hurtSFX);
            }
            if (curHP > maxHP)
            {
                curHP = maxHP;
            }
            else if (curHP <= 0f)
            {
                Die();
                return true;
            }
        }
        return false;
    }

    // Appy a force to the enemy
    public void ApplyForce(Vector2 force)
    {
        if (!invulnrable)
        {
            isStunned = true;
            rb.AddForce(force * knockbackMult, ForceMode2D.Impulse);
            StartCoroutine(StunWait());
        }
    }

    // Reset Stun
    private IEnumerator StunWait()
    {
        yield return new WaitForSeconds(0.3f);
        while (Vector2.Distance(rb.linearVelocity, Vector2.zero) > 0.01)
        {
            yield return new WaitForSeconds(minStunWaitTime);
        }
        isStunned = false;
    }

    public void SetDestination(Vector2 pos)
    {
        destination = pos;
    }


    // remove this Enemy from
    public abstract void Die();             // play effects, etc. when this object dies

    //This can be overridden, but this gives the ratio of HP
    //helper function basically to help code be a bit more readable.
    public float HPRatio()
    {
        return curHP / maxHP;
    }

    public abstract void VisualUpdate();

    IEnumerator IFrameTimer()
    {
        invulnrable = true;
        yield return new WaitForSeconds(iframes);
        invulnrable = false;
    }

    public int GetExpReward()
    {
        return expReward;
    }


    // play the death sound of this enemy
    protected IEnumerator PlayDeathSequence()
    {

        DisableEnemy();
        audioSource.PlayOneShot(dieSFX);
        EnemyManager.Instance.LoseEnemy(gameObject);
        yield return new WaitForSeconds(dieSFX.length);
        Destroy(gameObject);

    }

    // effectively removes the enemy from play
    protected void DisableEnemy()
    {
        hitbox.enabled = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
}