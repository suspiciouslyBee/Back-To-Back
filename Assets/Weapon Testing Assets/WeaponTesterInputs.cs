using UnityEngine;

public class WeaponTesterInputs : MonoBehaviour
{
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangePrefab;

    [SerializeField] private Vector2[] weaponPoints;

    private Weapon meleeWeapon;
    private Weapon rangeWeapon;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create objects
        meleePrefab = Instantiate(meleePrefab, transform.position, transform.rotation);
        rangePrefab = Instantiate(rangePrefab, transform.position, transform.rotation);

        // insert default values
        if (weaponPoints.Length < 2)
        {
            weaponPoints = new Vector2[2];
            weaponPoints[0] = new(-1, 1);
            weaponPoints[1] = new(1, 1);
        }
        // Move the objects apart
        meleePrefab.transform.Translate(weaponPoints[0]);
        rangePrefab.transform.Translate(weaponPoints[1]);

        // Aquire The Object's weapon attributes
        meleeWeapon = meleePrefab.GetComponent<Weapon>();
        rangeWeapon = rangePrefab.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        // Test melee attack
        if (Input.GetKeyDown(KeyCode.A))
        {
            meleeWeapon.DoAttack();
        }

        // Test Range attack
        if (Input.GetKeyDown(KeyCode.S))
        {
            rangeWeapon.DoAttack();
        }

        // Test melee reload
        if (Input.GetKeyDown(KeyCode.D))
        {
            meleeWeapon.Reload();
        }

        // Test range reload
        if (Input.GetKeyDown(KeyCode.F))
        {
            rangeWeapon.Reload();
        }

        rangePrefab.transform.Rotate(0, 0, 30 * Time.deltaTime);
    }
}
