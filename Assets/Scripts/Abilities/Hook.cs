using UnityEngine;
using System.Collections;

public class Hook : Ability
{
    [SerializeField] private float hookForce;
    [SerializeField] private float maxSearchDistance;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float lineDrawTime;
    private LineRenderer lr;
    private GameObject player;
    //------------------------- Getters -------------------------

    private GameObject FindEnemy(int direction)
    {
        /*
        Find the closest enemy in the given direction 

        Inputs:
        * Direction to look in (-1 = left)

        Output:
        * the first enemy found. null if no enemy found
        */
        Vector2 searchDirection = new Vector2(Mathf.Sign(direction), 0);
        RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, searchDirection, maxSearchDistance, enemyLayer);

        if (!enemyHit)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, new Vector3(direction * maxSearchDistance, transform.position.y, 0));
            StartCoroutine(DeleteLine());
            return null;
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, enemyHit.transform.position);
            StartCoroutine(DeleteLine());
            return enemyHit.transform.gameObject;
        }
    }

    private int GetPlayerDirection(GameObject player)
    {
        /*
        Get the direction of the player

        Inputs:
        * the player

        Output:
        * the direction facing (-1 = left)
        */
        return (int)Mathf.Sign(player.transform.localScale.x);
    }

    private Vector2 GetForce(int direction)
    {
        /*
        Get the force vector of given the facing of the player

        Inputs:
        * the player facing direction (-1 = left)

        Output:
        * the force to apply
        */
        return new Vector2(hookForce * -1 * direction, 0);
    }

    //------------------------- Setters -------------------------
    protected override void SetUpEffect()
    {
        /*
        Set up the variables used in the hook

        Inputs:
        * None

        Output:
        * None
        */
        SetLineRenderer();
        SetPlayer();
    }

    private void SetPlayer()
    {
        /*
        Get the player that used the ability

        Inputs:
        * None

        Output:
        * None
        */
        player = transform.parent.gameObject;
    }
    private void SetLineRenderer()
    {
        /*
        Get the line renderer for the hook

        Inputs:
        * None

        Output:
        * None
        */
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }

    //------------------------- Actions -------------------------
    protected override void ActivateCustomEffect()
    {
        /*
        Pull the closest enemy the player is facing towards the player

        Inputs:
        * None

        Output:
        * None
        */
        int direction = GetPlayerDirection(player);
        Vector2 forceToEnemy = GetForce(direction);
        GameObject enemy = FindEnemy(direction);
        if (enemy != null)
        {
            Debug.Log("Pulling enemy");
            PullEnemyIn(enemy, forceToEnemy);
        }
    }

    private void PullEnemyIn(GameObject enemy, Vector2 force)
    {
        /*
        Add a force to the given enemy towards the player

        Inputs:
        * Enemy to pull

        Output:
        * None
        */
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.ApplyForce(force);
    }

    IEnumerator DeleteLine()
    {
        yield return new WaitForSeconds(lineDrawTime);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }
}
