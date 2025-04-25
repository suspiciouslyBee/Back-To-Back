using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public abstract void Spawn();           //  initialize a new instance
    public abstract void Attack();          //  Peform the attack that this entity can hold

    public abstract void AI();              // controls the behavior of this enemy type

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

    public abstract void Die();             // play effects, etc. when this object dies

    // protected values
    protected float curHP;
    protected float maxHP;

    protected float speed;
}