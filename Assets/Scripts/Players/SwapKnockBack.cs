using System.Collections;
using UnityEngine;

public class SwapKnockBack : MonoBehaviour
{
    SpriteRenderer sR;
    Collider2D c2;

    private void Start()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
        c2 = gameObject.GetComponent<Collider2D>();
        sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, 0);
        c2.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().ApplyForce(new Vector2(-5f * col.transform.localScale.x, 0.5f));
        }
    }

    public IEnumerator Appear(float time)
    {
        sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, 1);
        c2.enabled = true;
        for (float i = 0; i < time; i += (time / 10))
        {
            sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, sR.color.a - 0.1f);
            yield return new WaitForSeconds(time / 10);
        }
        c2.enabled = false;
    }
}
