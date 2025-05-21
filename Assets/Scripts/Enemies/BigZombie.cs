using UnityEngine;

public class BigZombie : Enemy
{
    public override void AI()
    {
        MoveToDestination();
    }

    public override void Spawn()
    {
        rb = GetComponent<Rigidbody2D>();
        destination = new();

    }


    // perform an attack
    public override void Attack(GameObject player)
    {
        Debug.Log("[BigZombie] attacking!");
        player.GetComponent<Player>().Hurt(attackDamage);
        // ApplyForce(new Vector2(-10 * transform.localScale.x, 0));
    }


    // death logic
    public override void Die()
    {
        //for now, we just need this to dissapear
        StartCoroutine(PlayDeathSequence());

    }

    public override void MoveToPoint(Vector2 pos)
    {
        Vector2 dir = pos - (Vector2)transform.position;

        transform.localScale = new Vector3(Mathf.Sign(dir.x), transform.localScale.y, transform.localScale.z);    // allows for inverting of sprite if it moves left

        rb.linearVelocityX = Mathf.Sign(dir.x) * speed;
        // rb.linearVelocity = new(Mathf.Sign(dir.x) * speed, rb.linearVelocity.y);
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
