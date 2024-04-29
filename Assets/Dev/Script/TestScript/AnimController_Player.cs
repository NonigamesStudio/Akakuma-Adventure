using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController_Player : MonoBehaviour
{
    public static AnimController_Player ins;

    private void Awake()
    {
        if (ins != null) Destroy(this);
        else ins = this;
    }

    [SerializeField] Animator anim;

    public void PlayAnim(AnimNamesPlayer animToPlay)
    {
        switch (animToPlay)
        {
            case AnimNamesPlayer.Idle:
                anim.Play("Idle");
                break;
            case AnimNamesPlayer.TakeDmg:
                anim.Play("Damage");
                break;
            case AnimNamesPlayer.Dash:
                anim.Play("Dash");
                break;
            case AnimNamesPlayer.Death:
                anim.Play("Death");
                break;
            case AnimNamesPlayer.AttackSword:
                anim.SetTrigger("SwordAttack");
                break;
            case AnimNamesPlayer.AttackScythe:
                anim.Play("Guadania");
                break;
            case AnimNamesPlayer.AttackShield:
                anim.Play("Ataque escudos");
                break;
            case AnimNamesPlayer.AttackBow:
                anim.Play("Cambio arco");
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if(Input.GetAxis("Vertical") + Input.GetAxis("Horizontal")!=0) anim.SetFloat("Walk", 1); 
        else anim.SetFloat("Walk", 0);

        if (Input.GetMouseButtonDown(0)) PlayAnim(AnimNamesPlayer.AttackSword);
    }


}

public enum AnimNamesPlayer
{
    Idle,
    TakeDmg,
    Dash,
    Death,
    AttackSword,
    AttackScythe,
    AttackShield,
    AttackBow
}