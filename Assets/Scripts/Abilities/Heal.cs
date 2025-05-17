using System.Collections;
using UnityEngine;

public class Heal : Ability
{
    [SerializeField] private float waitTime;
    [SerializeField] private float abilityTime;
    [SerializeField] private float healAmount;

    private Player thisParent;

    //------------------------- Setters -------------------------
    protected override void SetUpEffect()
    {
        /*
        Set up the variables used in healing

        Inputs:
        * None

        Output:
        * None
        */

        thisParent = transform.parent.gameObject.GetComponent<Player>();
        return;
    }

    //------------------------- Actions -------------------------
    protected override void ActivateCustomEffect()
    {
        /*
        ???

        Inputs:
        * None

        Output:
        * None
        */

        StartCoroutine(Healing());
    }

    IEnumerator Healing()
    {
        thisParent.changeDirection(true);
        float timer = 0;
        float waitTimer = 0;
        while(timer < abilityTime)
        {
            timer += Time.deltaTime;
            waitTimer += Time.deltaTime;
            if (waitTimer > waitTime)
            {
                waitTimer = 0;
                PlayerManager.Instance.HealPlayers(healAmount, healAmount);
            }
            yield return new WaitForSeconds(0.01f);
        }
        thisParent.changeDirection(true);
    }
}
