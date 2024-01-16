using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour
{
    [SerializeField] Coin[] coins;


    private void OnEnable()
    {
        foreach (Coin coin in coins)
        {
            Transform tCoin = coin.transform;
            tCoin.SetParent(null);
            tCoin.localEulerAngles = new Vector3(tCoin.localEulerAngles.x, Random.Range(0, 360), tCoin.localEulerAngles.z);
            coin.rb.AddForce(tCoin.forward * 4, ForceMode.Impulse);
            coin.rb.AddTorque(coin.transform.forward *10, ForceMode.Acceleration);
        }

        Destroy(gameObject);
    }

}
