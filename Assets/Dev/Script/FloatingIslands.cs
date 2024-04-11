using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIslands : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveTime = 3f;

    
    void Start()
    {
        float upPosition = transform.position.y + moveDistance;
        float downPosition = transform.position.y - moveDistance;

       
        LeanTween.moveY(gameObject, upPosition, moveTime)
            .setEaseInOutSine() 
            .setLoopPingPong(); 
    }

  
}