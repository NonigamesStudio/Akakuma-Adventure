using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) anim.Play("down");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) anim.Play("up");
    }
}
