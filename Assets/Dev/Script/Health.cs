using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Health : MonoBehaviour
{
    
    [SerializeField] public float maxHealth;
    [SerializeField] public float actualHealth;
    public System.Action OnDeath;
    public System.Action <Transform> OnLifeChange;

    
    private void OnEnable()
    {
        actualHealth = maxHealth;
    }
    
    public void TakeDamage(float dmg, Transform attacker=null)
    {
        actualHealth -= dmg;
        CheckIfIsDeath();
        OnLifeChange?.Invoke(attacker);
        if (TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
        {
            enemyAI.SwitchToAttackState();
            enemyAI.SearchAndSetNearbyAllys();
            if(AudioManager.instance != null) AudioManager.instance.PlayOneShot(FMODEvents.instance.smallEnemyTakesDamage, transform.position);
        }
        
    }
    public void TakeHealth(float health)
    {
        actualHealth += health;
        OnLifeChange?.Invoke(null);
        
    }
    public void CheckIfIsDeath()
    {
        if (actualHealth <= 0) 
        {
            OnDeath?.Invoke();

            if (tag=="Enemy")
            {
                if (AudioManager.instance != null) AudioManager.instance.PlayOneShot(FMODEvents.instance.smallEnemyDeath, transform.position);
            }
        }
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        actualHealth=maxHealth;
    }
}
