using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirggerBoss : MonoBehaviour
{

    [SerializeField] GameObject boss;
    [SerializeField] KingSlimeHealthBarUI kingSlimeHealthBarUI;
    [SerializeField] EnemyController enemyController;
    // Start is called before the first frame update
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            boss.SetActive(true);
            kingSlimeHealthBarUI.Show();
            enemyController.AnimDoorClose();
            Destroy(gameObject);

        }
    }
}
