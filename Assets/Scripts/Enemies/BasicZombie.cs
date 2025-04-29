using UnityEngine;

public class BasicZombie : Enemy
{

    [SerializeField] private float jumpInterval;        // how frequently this Zombie should jump
    private float jumpTimer;
    public override void AI()
    {
        jumpTimer += Time.deltaTime;
        if (jumpTimer > jumpInterval && grounded)
        {
            jumpTimer = 0;
            Jump();
        }
        Debug.Log($"[BasicZombie] moving towards (0,0)");
        MoveToDestination();
    }

    // spawn logic
    public override void Spawn()
    {
        rb = GetComponent<Rigidbody2D>();
        destination = new();
    }


    // perform an attack
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }


    // death logic
    public override void Die()
    {
        //for now, we just need this to dissapear
        Destroy(gameObject);
    }

    // basic movement
    public override void MoveToPoint(Vector2 pos)
    {
        Vector2 dir = pos - (Vector2)transform.position;

        transform.localScale = new Vector3(Mathf.Sign(dir.x) * transform.localScale.x, transform.localScale.y, transform.localScale.z);    // allows for inverting of sprite if it moves left

        rb.linearVelocity = new(Mathf.Sign(dir.x) * speed, rb.linearVelocity.y);
    }

    public override void Jump()
    {
        rb.linearVelocity = new(rb.linearVelocity.x, jumpPower);
    }


    // Update the visuals of the enemy
    public override void VisualUpdate()
    {
        //for some reason i cant edit alpha directly??
        Color baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor.a = Mathf.Clamp(HPRatio(), 0, 1);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;
    }
}
