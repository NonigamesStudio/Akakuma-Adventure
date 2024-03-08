using UnityEngine;
using System;


public class Dash : MonoBehaviour
{
    [SerializeField] private Transform t;
    [SerializeField] Player player;
    [SerializeField] float distanceDash;
    [SerializeField] float speedDash;
    [SerializeField] float coolDownDashTime;
    bool canDash;
    float distanceDetection = 0.7f; 
    
    
    void Start()
    {
        player.OnDash += ManageDash;
    }
    void Update()
    {
        coolDownDashTime -= Time.deltaTime;
        canDash = !Physics.Raycast(t.position, t.forward, distanceDetection);
    }    
    private void ManageDash(object sender, EventArgs e)
    {
        if (coolDownDashTime > 0) return;
       
        coolDownDashTime = 1f;

        player.isStuned = true;
        

        Vector3 posToMove = t.position + t.forward * distanceDash;

    
        LeanTween.value(0, 1, speedDash).setOnUpdate((float value) => { 
        if (canDash)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove, value);
            
        }else
        {
            OnDashCompleted();
            LeanTween.cancelAll();

        }
        }).setOnComplete(()=> { OnDashCompleted();});
    }
    void OnDashCompleted()
    {
       player.isStuned = false;
    }
}