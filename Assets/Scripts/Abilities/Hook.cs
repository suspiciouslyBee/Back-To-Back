using UnityEngine;

public class Hook : Ability
{
    [SerializeField] private float hookForce;
    [SerializeField] private float maxSearchDistance;
    [SerializeField] private LayerMask enemyLayer;
    //------------------------- Getters -------------------------
    private GameObject GetPlayer()
    {
        /*
        Get the player that used the ability

        Inputs:
        * None

        Output:
        * the player for the ability
        */
        return transform.parent.gameObject;
    }

    private GameObject FindEnemy(int direction)
    {
        /*
        Find the closest enemy in the given direction 

        Inputs:
        * Direction to look in (-1 = left)

        Output:
        * the first enemy found. null if no enemy found
        */
        RaycastHit enemyHit;
        Vector2 searchDirection = new Vector2(direction, 0);
        Physics.Raycast(transform.position, transform.TransformDirection(searchDirection), out enemyHit, maxSearchDistance, enemyLayer);
        return enemyHit.transform.gameObject;
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
        GameObject player = GetPlayer();
        int direction = GetPlayerDirection(player);
        Vector2 forceToEnemy = GetForce(direction);
        GameObject enemy = FindEnemy(direction);
        PullEnemyIn(enemy, forceToEnemy);
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
}
