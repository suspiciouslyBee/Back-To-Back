using UnityEngine;

public class TutorialZombie : Enemy
{
    [SerializeField] private float jumpInterval;        // how frequently this Zombie should jump
    private float jumpTimer;
    public override void AI()
    {
        jumpTimer += Time.deltaTime;
        if (jumpTimer > jumpInterval && grounded)
        {
            jumpTimer = 0;
            // Jump();
        }
        // Debug.Log($"[BasicZombie] moving towards (0,0)");
        if (!TutorialManager.TMInstance.tutorialPause)
        {
            MoveToDestination();
        }
    }

    // spawn logic
    public override void Spawn()
    {
        rb = GetComponent<Rigidbody2D>();
        destination = new();
    }


    // perform an attack
    public override void Attack(GameObject player)
    {
        player.GetComponent<Player>().Hurt(attackDamage);
        // ApplyForce(new Vector2(-10 * transform.localScale.x, 0));
    }


    // death logic
    public override void Die()
    {
        if (TutorialManager.TMInstance.tutorialStage == 1.75f || TutorialManager.TMInstance.tutorialStage == 2.5f || TutorialManager.TMInstance.tutorialStage == 2.85f)
        {
            TutorialManager.TMInstance.nextStage();
        }
        else if ((TutorialManager.TMInstance.tutorialStage == 3.75f || TutorialManager.TMInstance.tutorialStage == 4.5f) && EnemyManager.Instance.GetEnemyCount() == 1)
        {
            TutorialManager.TMInstance.nextStage();
        }
        //for now, we just need this to dissapear
        StartCoroutine(PlayDeathSequence());
    }

    // basic movement
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
