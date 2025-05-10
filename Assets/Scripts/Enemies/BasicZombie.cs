using UnityEngine;

public class BasicZombie : Enemy
{

    [SerializeField] private float jumpInterval;        // how frequently this Zombie should jump

    private float randomizedJumpInterval;
    private float jumpTimer;
    public override void AI()
    {
        jumpTimer += Time.deltaTime;
        if (jumpTimer > randomizedJumpInterval && grounded)
        {
            jumpTimer = 0;
            randomizedJumpInterval = jumpInterval * Random.Range(0.75f, 1.25f);
            Jump();
        }
        // Debug.Log($"[BasicZombie] moving towards (0,0)");
        MoveToDestination();
    }

    // spawn logic
    public override void Spawn()
    {
        rb = GetComponent<Rigidbody2D>();
        destination = new();
        randomizedJumpInterval = jumpInterval * Random.Range(0.75f, 1.25f);
    }


    // perform an attack
    public override void Attack(GameObject player)
    {
        player.GetComponent<Player>().Hurt(10);
        // ApplyForce(new Vector2(-10 * transform.localScale.x, 0));
    }


    // death logic
    public override void Die()
    {
        //for now, we just need this to dissapear
        Destroy(gameObject);
        EnemyManager.Instance.LoseEnemy(gameObject);
    }

    // basic movement
    public override void MoveToPoint(Vector2 pos)
    {
        Vector2 dir = pos - (Vector2)transform.position;

        transform.localScale = new Vector3(Mathf.Sign(dir.x), transform.localScale.y, transform.localScale.z);    // allows for inverting of sprite if it moves left

        rb.linearVelocityX = Mathf.Sign(dir.x) * speed;
        // rb.linearVelocity = new(Mathf.Sign(dir.x) * speed, rb.linearVelocity.y);
    }

    public override void Jump()
    {
        rb.linearVelocity = new(rb.linearVelocity.x, jumpPower);
    }

    public override void VisualUpdate()
    {
        //for some reason i cant edit alpha directly??
        Color baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        //baseColor.a = Mathf.Clamp(HPRatio(), 0.2f, 1);
        baseColor.g = Mathf.Clamp(HPRatio(), 0.3f, 1);
        baseColor.b = Mathf.Clamp(HPRatio(), 0.3f, 1);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;
    }
}
