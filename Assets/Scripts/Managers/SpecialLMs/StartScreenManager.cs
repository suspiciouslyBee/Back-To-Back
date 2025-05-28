using UnityEngine;
using UnityEngine.UIElements;

public class StartScreenManager : LevelManager
{
    [SerializeField] InputPreference uM;

    bool changingMenu;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //need to show main menu
        changingMenu = false;
    }

    protected override void FixedUpdate()
    {
        //should do nothing?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //they need to be empty to clear the behavior of base
    public override void FireGun()
    {
        if (!changingMenu)
        {
            traverseMenu(1);
        }
    }

    public override void ReloadGun()
    {

    }

    public override void SwingSword()
    {
        if (!changingMenu)
        {
            traverseMenu(2);
        }
    }

    public override void SwapChars()
    {
        uM.changePreference(false);
    }
    public override void MeleeAbility1()
    {

    }

    public override void MeleeAbility2()
    {

    }

    public override void RangedAbility1()
    {

    }

    public override void RangedAbility2()
    {

    }

    public override void RestartLevel()
    {
        
    }

    void traverseMenu(int i)
    {
        changingMenu = true;
        MainManager.Instance.ChangeStageRelatively(i);
    }
}
