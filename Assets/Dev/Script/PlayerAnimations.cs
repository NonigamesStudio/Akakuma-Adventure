using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]private Animator playerAnimator;
    [SerializeField] Health playerH;
    [SerializeField] Player playerW;
    [SerializeField] PlayerMovement playerMovement;


    private void OnEnable()
    {
        playerH.OnTakeDmg += ()=> { playerAnimator.SetTrigger("onHit"); };
        playerH.OnDeath += () => { playerAnimator.SetBool("onDeath",true); Debug.Log("Llamada muerte"); };
        playerW.OnWeaponAttack += () => { playerAnimator.SetTrigger("onAttack"); };
        playerW.OnBowReady += () => { playerAnimator.SetTrigger("bowWindUp"); };
        playerW.OnBowRealese += () => { playerAnimator.SetTrigger("bowRealese"); };
        playerW.OnWeaponSkill += SkillUse;
    }
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical")!= 0 || Input.GetAxis("Horizontal") != 0)
        {
            playerAnimator.SetBool("moving", true);
        }
        else
        {
            playerAnimator.SetBool("moving", false);
        }
        playerAnimator.SetFloat("directionRun",playerMovement.speed*10);

    }

    private void SkillUse()
    {
        switch (playerW.currentWeapon)
        {
            default:
                break;
            case 0:
                break;
            case 1:
                playerAnimator.SetBool("scytheSkill",true);
                LeanTween.delayedCall(5, () => playerAnimator.SetBool("scytheSkill", false));
                break;
            case 2:
                playerAnimator.SetTrigger("blockSkill");
                break;
        }
    }


}
