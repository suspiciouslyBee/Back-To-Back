using UnityEngine;

public class SwapKnockBack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().ApplyForce(new Vector2(-275 * col.transform.localScale.x, 15));
        }
    }
}
