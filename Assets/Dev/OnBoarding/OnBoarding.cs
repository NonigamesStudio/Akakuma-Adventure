using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoarding : MonoBehaviour
{
    private int currentStep;
    GameObject currentPanel;

    [Header("UI Refs")]
    public GameObject asdwStep_Panel;
    public GameObject attackStep_Panel;
    public GameObject attackBowStep_Panel;
    public GameObject changeWeaponStep_Panel;
    public GameObject useSkillStep_Panel;
    public GameObject useDashStep_Panel;

    [Space(10)]

    public GameObject door;


    public int CurrentStep { get => currentStep; set { currentStep = value; ChangeStepOnBoarding((OnBoardingSteps)value); } }


    bool onBoardingEventFinished;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        CurrentStep = 0;
    }

    private void Update()
    {
        if (onBoardingEventFinished) return;

        switch ((OnBoardingSteps)CurrentStep)
        {
            case OnBoardingSteps.Move:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)) CurrentStep++;
                break;
            case OnBoardingSteps.Attack:
                if(Input.GetMouseButtonDown(0)) CurrentStep++;
                break;
            case OnBoardingSteps.Arrow:
                if (Input.GetMouseButtonDown(1)) CurrentStep++;
                break;
            case OnBoardingSteps.ChangeWeapon:
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)) CurrentStep++;
                break;
            case OnBoardingSteps.Skill:
                if(Input.GetKeyDown(KeyCode.F)) CurrentStep++;
                break;
            case OnBoardingSteps.Dash:
                if (Input.GetKeyDown(KeyCode.Space)) CurrentStep++;
                break;
            default:
                AnimDoor();
                onBoardingEventFinished = true;
                break;
        }
    }

    public void ChangeStepOnBoarding(OnBoardingSteps step)
    {
        if(currentPanel!=null) AnimPanels(false, currentPanel);

        switch (step)
        {
            case OnBoardingSteps.Move:
                currentPanel = asdwStep_Panel;
                AnimPanels(true, currentPanel);
                break;
            case OnBoardingSteps.Attack:
                currentPanel = attackStep_Panel;
                AnimPanels(true, currentPanel);
                break;
            case OnBoardingSteps.Arrow:
                currentPanel = attackBowStep_Panel;
                AnimPanels(true, currentPanel);
                break;
            case OnBoardingSteps.ChangeWeapon:
                currentPanel = changeWeaponStep_Panel;
                AnimPanels(true, currentPanel);
                break;
            case OnBoardingSteps.Skill:
                currentPanel = useSkillStep_Panel;
                AnimPanels(true, currentPanel);
                break;
            case OnBoardingSteps.Dash:
                currentPanel = useDashStep_Panel;
                AnimPanels(true, currentPanel);
                break;
            default:
                
                break;
        }
    }   

    #region Anims
    public void AnimPanels(bool onOff, GameObject panel)
    {
        LeanTween.cancel(panel);
        if (onOff) { 
            LeanTween.delayedCall(2f, () => { 
                LeanTween.moveY(panel, 0, 1f).setEaseOutBack().setOnComplete(()=> { 
                    LeanTween.moveY(panel, 20f, 1f).setLoopPingPong(-1); 
                }); 
            });
            
        }
        else LeanTween.moveY(panel, -400, 1f).setEaseInBack();
    }

    public void AnimDoor()
    {
        LeanTween.rotateY(door, -45, 0.5f).setOnComplete(()=> { gameObject.SetActive(false); });
    }
    #endregion

}

public enum OnBoardingSteps
{
    Move,
    Attack,
    Arrow,
    ChangeWeapon,
    Skill,
    Dash
}