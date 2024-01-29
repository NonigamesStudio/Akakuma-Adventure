using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToAttack : MonoBehaviour
{
    public EnemyAI enemyAi;
    
    public void Attack()
    {
        enemyAi.AttackWeapon();
    }
}
