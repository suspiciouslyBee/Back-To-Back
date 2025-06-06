using System.Collections;
using UnityEngine;

public class Heal : Ability
{
    [SerializeField] ParticleSystem healingParticles;
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

        thisParent.PerformingAction(abilityTime + 0.4f);
        ParticleSystem hP = Instantiate(healingParticles, gameObject.transform);
        StartCoroutine(Healing(hP));
    }

    IEnumerator Healing(ParticleSystem pS)
    {
        pS.Play();
        thisParent.changeDirection(true);
        float timer = 0;
        float waitTimer = 0;
        while(timer < abilityTime)
        {
            timer += 0.1f;
            waitTimer += 0.1f;
            if (waitTimer > waitTime)
            {
                waitTimer = 0;
                PlayerManager.Instance.HealPlayers(healAmount, healAmount);
            }
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(pS);
        thisParent.changeDirection(true);
    }
}
