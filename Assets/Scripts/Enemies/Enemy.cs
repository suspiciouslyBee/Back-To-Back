using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{

    [SerializeField] protected Vector2 destination;


    // protected values
    protected float curHP;
    [SerializeField] protected float maxHP;

    [SerializeField] protected float speed;

    [SerializeField] protected float jumpPower;

    [SerializeField] protected Collider2D hitbox;

    [SerializeField] protected bool grounded;
    [SerializeField] protected float minStunWaitTime;
    protected bool isStunned;

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
    }

    public void MoveToDestination()
    {
        if(!isStunned)
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
    public void ChangeHP(float amount)
    {
        curHP += amount;
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        else if (curHP <= 0f)
        {
            Die();
        }
    }

    // Appy a force to the enemy
    public void ApplyForce(Vector2 force)
    {
        isStunned = true;
        rb.AddForce(force);
        StartCoroutine(StunWait());
    }

    // Reset Stun
    private IEnumerator StunWait()
    {
        while(Vector2.Distance(rb.linearVelocity, Vector2.zero) > 0.01 )
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

}