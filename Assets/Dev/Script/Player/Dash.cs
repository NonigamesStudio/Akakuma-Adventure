using UnityEngine;
using System;
using System.Drawing;
using System.Collections;


public class Dash : MonoBehaviour
{
    [SerializeField] private Transform t;
    [SerializeField] Player player;
    [SerializeField] float distanceDash;
    [SerializeField] float speedDash;
    [SerializeField] float coolDownDashTime;
    [SerializeField] Animator animator;
    
    bool canDash;
    
    
    
    void Start()
    {
        player.OnDash += ManageDash;
    }
    void Update()
    {
        coolDownDashTime -= Time.deltaTime;
        
    }    
    private void ManageDash(object sender, EventArgs e)
    {
        

        if (coolDownDashTime > 0) return;
           
        coolDownDashTime = 1f;

        player.isStuned = true;
        
        StartCoroutine(DashCoroutine());
       
    }
   

    IEnumerator DashCoroutine()
    {
        Vector3 originalPosition = t.position;
        Vector3 posToMove = Vector3.zero;

        canDash = !Physics.Raycast(t.position, t.forward, out RaycastHit hit, distanceDash);
        if (canDash)
        {
            
            posToMove = t.position + t.forward * distanceDash;
            AnimController_Player.ins.PlayAnim(AnimNamesPlayer.Dash);
        }
        else
        {
            posToMove = hit.point;
        }

        float timeElapsed = 0;
        
        while (Vector3.Distance(t.position, posToMove)>0.5f)
        {
            
            
            timeElapsed += Time.deltaTime;
            float step = Mathf.Lerp(0, 1, Mathf.Clamp(timeElapsed * speedDash, 0, 1));

            if (canDash)
            {
               t.position = Vector3.MoveTowards(t.position, posToMove, step);
            }
            else
            {
                t.position = Vector3.MoveTowards(t.position, hit.point, step);
            }

            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            yield return null;

        }
        OnDashCompleted();
        

    

    }
    void OnDashCompleted()
    { 
        player.isStuned = false;
        
    }

}