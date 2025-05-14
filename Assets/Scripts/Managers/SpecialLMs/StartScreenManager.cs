using UnityEngine;
using UnityEngine.UIElements;

public class StartScreenManager : LevelManager
{

 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //need to show main menu


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
        MainManager.Instance.ChangeStageRelatively(2);
    }

    public override void ReloadGun()
    {

    }

    public override void SwingSword()
    {
        MainManager.Instance.ChangeStageRelatively(1);
    }

    public override void SwapChars()
    {

    }
}
