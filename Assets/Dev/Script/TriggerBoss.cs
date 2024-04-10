using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    [SerializeField] GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            boss.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
