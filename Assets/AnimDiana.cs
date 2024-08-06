using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDiana : MonoBehaviour
{
    [SerializeField] Health health;


    private void OnEnable()
    {
        health.OnLifeChange += HitAnim;
    }

    void HitAnim(Transform a)
    {
       if(!LeanTween.isTweening(gameObject)) LeanTween.rotateLocal(gameObject, new Vector3(-145,gameObject.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.z), 0.1f).setOnComplete(() => { LeanTween.rotateLocal(gameObject, new Vector3(-90, gameObject.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.z), 0.3f).setEaseInOutBack(); });
    }

    private void OnDisable()
    {
        health.OnLifeChange -= HitAnim;
    }
}
