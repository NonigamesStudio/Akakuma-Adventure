using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoarding : MonoBehaviour
{
    private int currentStep;
    GameObject currentPanel;

    [SerializeField] bool test;

    [Space(10)]
    [Header("UI Refs")]
    public GameObject asdwStep_Panel;
    public GameObject attackStep_Panel;
    public GameObject attackBowStep_Panel;
    public GameObject changeWeaponStep_Panel;
    public GameObject useSkillStep_Panel;
    public GameObject useDashStep_Panel;

    [Space(10)]

    public GameObject door;
    bool animReady;



    bool onBoardingEventFinished;
    private IEnumerator Start()
    {
       if(test)
        {
            AnimDoor();
            onBoardingEventFinished = true;
            yield break;
        }

        yield return new WaitForSeconds(0.5f);


        AnimPanels(true, asdwStep_Panel);
        yield return new WaitForSeconds(2f);
        bool asdw = true;
        while (asdw)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)) asdw = false;
            yield return null;
        }
        AnimPanels(false, asdwStep_Panel);


        yield return new WaitForSeconds(2f);


        AnimPanels(true, attackStep_Panel);
        yield return new WaitForSeconds(1f);
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        AnimPanels(false, attackStep_Panel);


        yield return new WaitForSeconds(2f);


        AnimPanels(true, attackBowStep_Panel);
        yield return new WaitForSeconds(1f);
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }
        AnimPanels(false, attackBowStep_Panel);


        yield return new WaitForSeconds(2f);


        AnimPanels(true, changeWeaponStep_Panel);
        yield return new WaitForSeconds(2f);
        bool changeWeaponStep = true;
        while (changeWeaponStep)
        {
            if (Input.GetKeyDown(KeyCode.Q)) changeWeaponStep = false;
            yield return null;
        }
        AnimPanels(false, changeWeaponStep_Panel);


        yield return new WaitForSeconds(2f);


        AnimPanels(true, useSkillStep_Panel);
        yield return new WaitForSeconds(1f);
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
        AnimPanels(false, useSkillStep_Panel);


        yield return new WaitForSeconds(2f);


        AnimPanels(true, useDashStep_Panel);
        yield return new WaitForSeconds(1f);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        AnimPanels(false, useDashStep_Panel);

        yield return new WaitForSeconds(1f);
        AnimDoor();
        onBoardingEventFinished = true;

    }


    void NextStep()
    {
        animReady = true;
        StartCoroutine(NextStepWait());
    }
    IEnumerator NextStepWait()
    {
        yield return new WaitForSeconds(2);
        currentStep++;
        ChangeStepOnBoarding((OnBoardingSteps)currentStep);
        animReady = false;
    }


    public void ChangeStepOnBoarding(OnBoardingSteps step)
    {
        if (currentPanel != null) { AnimPanels(false, currentPanel);}
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

        if (onOff)
        {
                LeanTween.moveY(panel, 0, 1f).setEaseOutBack().setOnComplete(() =>
                {
                    LeanTween.moveY(panel, 20f, 1f).setLoopPingPong(-1);
                });
            

        }
        else { 
           
            LeanTween.moveY(panel, -400, 1f).setEaseInBack();  
        }

    }

    public void AnimDoor()
    {
        if (door == null) return;
        LeanTween.rotateY(door, 28, 0.5f).setOnComplete(()=> { gameObject.SetActive(false); });
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