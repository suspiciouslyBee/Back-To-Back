using UnityEngine;

public class WeaponTesterInputs : MonoBehaviour
{
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangePrefab;

    private Weapon meleeWeapon;
    private Weapon rangeWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create objects
        meleePrefab = Instantiate(meleePrefab, transform.position, transform.rotation);
        rangePrefab = Instantiate(rangePrefab, transform.position, transform.rotation);

        // Move the objects apart
        meleePrefab.transform.Translate(-2, -1, 0);
        rangePrefab.transform.Translate(2, -1, 0);

        // Aquire The Object's weapon attributes
        meleeWeapon = meleePrefab.GetComponent<Weapon>();
        rangeWeapon = rangePrefab.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        // Test melee attack
        if(Input.GetKeyDown(KeyCode.A))
        {
            meleeWeapon.DoAttack();
        }

        // Test Range attack
        if(Input.GetKeyDown(KeyCode.S))
        {
            rangeWeapon.DoAttack();
        }

        // Test melee reload
        if(Input.GetKeyDown(KeyCode.D))
        {
            meleeWeapon.Reload();
        }

        // Test range reload
        if(Input.GetKeyDown(KeyCode.F))
        {
            rangeWeapon.Reload();
        }

        rangePrefab.transform.Rotate(0, 0, 30 * Time.deltaTime);
    }
}
