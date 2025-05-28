using UnityEngine;

public class Caltrop : Ability
{
    [SerializeField] private Vector2 throwForceVector;
    [SerializeField] private float throwVariation;
    [SerializeField] private GameObject CaltopHitbox;
    [SerializeField] private int numCaltrops;
    private GameObject player;
    //------------------------- Getters -------------------------

    private int GetPlayerDirection()
    {
        /*
        Get the direction of the player

        Inputs:
        * None

        Output:
        * the direction facing (-1 = left)
        */
        return (int)Mathf.Sign(player.transform.localScale.x);
    }

    private Vector2 GetRandomizedForce()
    {
        /*
        Get a random force to throw the caltrop away from the palyer

        Inputs:
        * None

        Output:
        * the force vector for the caltrop
        */

        int direction = GetPlayerDirection();
        float randomX = Random.Range(-1 * throwVariation / 2, throwVariation / 2);
        float randomY = 1; //Random.Range(-1 * throwVariation / 2, throwVariation / 2);
        float forceX = (direction * throwForceVector.x) + randomX;
        float forceY = (throwForceVector.y) + randomY;
        return new Vector2(forceX, forceY);
    }

    //------------------------- Setters -------------------------
    protected override void SetUpEffect()
    {
        /*
        Set up the variables used in the caltrops

        Inputs:
        * None

        Output:
        * None
        */
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

    //------------------------- Actions -------------------------
    protected override void ActivateCustomEffect()
    {
        /*
        Throw a number of caltops toward the facing of the player

        Inputs:
        * None

        Output:
        * None
        */
        ThrowMultCaltops(numCaltrops);
    }

    private void ThrowMultCaltops(int amount)
    {
        /*
        Throw the number of caltops

        Inputs:
        * None

        Output:
        * None
        */

        for (int i = 0; i < amount; i++)
        {
            Vector2 tempForce = GetRandomizedForce();
            ThrowCaltrop(tempForce);
        }
    }

    private void ThrowCaltrop(Vector2 force)
    {
        /*
        Initalize a Caltrop with the given force vector

        Inputs:
        * None

        Output:
        * None
        */

        Vector3 pos = player.transform.position;
        GameObject newCaltrop = Instantiate(CaltopHitbox, pos, new Quaternion());
        newCaltrop.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
