using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CaltropHitbox : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;
    //------------------------- Getters -------------------------
    //------------------------- Setters -------------------------
    public void Start()
    {
        /*
        Set up the variables used in the caltrop

        Inputs:
        * None

        Output:
        * None
        */
        rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(rotationSpeed * Mathf.Deg2Rad * rb.inertia * Random.Range(1f, 3f), ForceMode2D.Impulse);
        StartCoroutine(CaltropLifeTime());
    }
    //------------------------- Actions -------------------------
    private void DestroyCaltrop()
    {
        /*
        Destroy the caltrop

        Inputs:
        * None

        Output:
        * None
        */
        Destroy(gameObject);
    }

    IEnumerator CaltropLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyCaltrop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Hitbox hit an enemy
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            Enemy enemyScript = other.GetComponent<Enemy>();
            enemyScript.ChangeHP(-1 * damage);
            Debug.Log("Hurt Enemy");
        }
    }
}
