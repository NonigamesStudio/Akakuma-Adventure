using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController_Player : MonoBehaviour
{
    public static AnimController_Player ins;
    [SerializeField] private Rigidbody rb;
    [SerializeField] Animator anim;

    public bool dashing;

    private float counterWaitTime;

    private float waitTime=0.1f;

    private Player player;
    
    
    private void Awake()
    {
        if (ins != null) Destroy(this);
        else ins = this;
        player = GetComponent<Player>();
    }

   

    public void PlayAnim(AnimNamesPlayer animToPlay)
    {
        switch (animToPlay)
        {
            case AnimNamesPlayer.Idle:
                anim.Play("Idle");
                break;
            case AnimNamesPlayer.DrawSword:
                anim.SetTrigger("DrawSword");
                break;
            case AnimNamesPlayer.TakeDmg:
                anim.Play("Damage");
                break;
            case AnimNamesPlayer.Dash:
                anim.SetTrigger("Dash");
                break;
            case AnimNamesPlayer.Death:
                anim.Play("Death");
                break;
            case AnimNamesPlayer.AttackSword:
                anim.SetTrigger("SwordAttack");
                break;
            case AnimNamesPlayer.AttackScythe:
                anim.Play("Guadana");
                break;
            case AnimNamesPlayer.AttackShield:
                anim.SetTrigger("ShieldPower");
                break;
            case AnimNamesPlayer.AttackBow:
                anim.SetTrigger("AttackBow");
                break;
            case AnimNamesPlayer.ReleaseBow:

                anim.SetBool("ReleaseBow 0", true);
                LeanTween.delayedCall(0.1f, () => { anim.SetBool("ReleaseBow 0", false); });
                // anim.SetBool("ReleaseBow 0", false);
                break;
            default:
                break;
        }
    }

    

    private void Update()
    {
        if (player.isStuned)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            return;
        } 
        counterWaitTime += Time.deltaTime;

        Vector3 isMoving = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        if (isMoving.magnitude > 0.1f)
        {
            counterWaitTime = 0;
            anim.SetBool("Walk", true);
           
        }else
        {
            if (counterWaitTime > waitTime)
            {
            anim.SetBool("Walk", false); 
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
            anim.SetBool("Run", true);
        }else 
        {
            anim.SetBool("Run", false);
        }

        SetPlayerDirection();

    }
    void SetPlayerDirection()
    {
        Vector3 globalDirection = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        if (globalDirection.magnitude < 0.1f) return;
        

        float offsetDegrees = 45f; 

        Quaternion offsetRotation = Quaternion.Euler(0, offsetDegrees, 0); 

        Vector3 rotatedGlobalDirection = offsetRotation * globalDirection;

        float vertical = Vector3.Dot(rotatedGlobalDirection.normalized, transform.forward.normalized);

        float horizontal = Vector3.Dot(rotatedGlobalDirection.normalized, transform.right.normalized);

        float positionTransitionSpeed = .05f;

        
        anim.SetFloat ("Vertical", vertical, positionTransitionSpeed, Time.deltaTime);
        

        if (vertical>-0.7)
        {
        anim.SetFloat ("Horizontal", horizontal, positionTransitionSpeed, Time.deltaTime);
        }else 
        {
        anim.SetFloat ("Horizontal", 0, 0.05f, Time.deltaTime);
        }

        
    }

    
}






public enum AnimNamesPlayer
{
    Idle,
    DrawSword,
    TakeDmg,
    Dash,
    Death,
    AttackSword,
    AttackScythe,
    AttackShield,
    AttackBow,
    ReleaseBow
}