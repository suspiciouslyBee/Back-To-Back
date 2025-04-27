using UnityEngine;

public class BasicZombie : Enemy
{
    public override void AI()
    {
        Debug.Log($"[BasicZombie] moving towards (0,0)");
        MoveToPoint(new());
    }

    // spawn logic
    public override void Spawn()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // perform an attack
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }


    // death logic
    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    // basic movement
    public override void MoveToPoint(Vector2 pos)
    {
        Vector2 dir = pos - (Vector2)transform.position;

        transform.localScale = new(Mathf.Sign(dir.x), 1, 1);    // allows for inverting of sprite if it moves left

        rb.linearVelocity = new(Mathf.Sign(dir.x) * speed, rb.linearVelocity.y);
    }



}
