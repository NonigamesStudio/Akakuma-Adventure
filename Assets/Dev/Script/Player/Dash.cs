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
    [SerializeField] Rigidbody rb;
    [SerializeField] AnimationClip dashAnimationClip;
    [SerializeField] LayerMask mask;
    
    bool canDash;
    
    
    
    void Start()
    {
        player.OnDash += ManageDash;
    }
    void Update()
    {
        if (coolDownDashTime > 0)
        {
            coolDownDashTime -= Time.deltaTime;
        }
        
    }    
    private void ManageDash(object sender, EventArgs e)
    {
        

        if (coolDownDashTime > 0) return;
           
        coolDownDashTime = 1f;

        player.isStuned = true;
        AnimController_Player.ins.PlayAnim(AnimNamesPlayer.Dash);
        
        StartCoroutine(DashCoroutine());
       
    }
   

    IEnumerator DashCoroutine()
    {
        Vector3 originalPosition = t.position;
        Vector3 posToMove = Vector3.zero;

        canDash = !Physics.Raycast(t.position, t.forward, out RaycastHit hit, distanceDash, mask);
        
        if (canDash)
        {
            
            posToMove = t.position + t.forward * distanceDash;
            
        }
        else
        {
            posToMove = hit.point;
        }

        float timeElapsed = 0;
       
       
        while (Vector3.Distance(t.position, posToMove) > 0.5f&&timeElapsed<.5f)
        {
            
            timeElapsed += Time.deltaTime;
            float step = Mathf.Lerp(0, 1, Mathf.Clamp(timeElapsed * speedDash, 0, 1));
            if (canDash)
            {
             
                t.position = Vector3.MoveTowards(t.position, posToMove, step* speedDash* Time.deltaTime);
               
                if (!Physics.Raycast(transform.position, Vector3.down, 1.5f, LayerMask.GetMask("Ground")))
                {
                   t.position = Vector3.MoveTowards(t.position, t.position + Vector3.down, step);
                   
                }
                
            }
            else
            {
                t.position = Vector3.MoveTowards(t.position, posToMove, step* speedDash* Time.deltaTime);
                
                if (!Physics.Raycast(transform.position, Vector3.down, 1.5f, LayerMask.GetMask("Ground")))
                {
                   t.position = Vector3.MoveTowards(t.position, t.position + Vector3.down, step);
                }
            }
            yield return null;
        }
       
        OnDashCompleted();
        
    }
    void OnDashCompleted()
    { 
        player.isStuned = false;
        
    }

}