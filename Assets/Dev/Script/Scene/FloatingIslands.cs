using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIslands : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveTime;

    
    void Start()
    {
        float upPosition = transform.position.y + moveDistance;
        float downPosition = transform.position.y - moveDistance;

       
        LeanTween.moveY(gameObject, upPosition, moveTime)
            .setEaseInOutSine() 
            .setLoopPingPong(-1); 
    }

  
}