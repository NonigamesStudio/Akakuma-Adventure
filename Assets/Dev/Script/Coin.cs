using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Coin : MonoBehaviour
{
    [SerializeField] int value;
    public Rigidbody rb;

    public static Action OnCoinCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { OnCoinCollected?.Invoke(); Destroy(gameObject); }
    }


}
