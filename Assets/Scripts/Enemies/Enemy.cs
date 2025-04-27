using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{

    private Vector2 destination;


    // protected values
    protected float curHP;
    protected float maxHP;

    protected float speed;

    protected Rigidbody2D rb;


    // controls everything of how the enemy works
    public abstract void AI();              // controls the behavior of this enemy type

    // spawning mechanics
    public abstract void Spawn();           //  initialize a new instance

    // Attack functionality
    public abstract void Attack();          //  Peform the attack that this entity can hold


    public abstract void MoveToPoint(Vector2 pos);  // attempt to move towards a destination

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

    public void SetDestination(Vector2 pos)
    {
        destination = pos;
    }

    // remove this Enemy from
    public abstract void Die();             // play effects, etc. when this object dies



}