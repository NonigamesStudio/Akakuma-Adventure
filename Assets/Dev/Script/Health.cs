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
    public System.Action <Transform> OnTakeDmg;

    private void OnEnable()
    {
        actualHealth = maxHealth;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (tag!="Player") TakeDamage(actualHealth);
        }
    }

    public void TakeDamage(float dmg, Transform attacker=null)
    {
        actualHealth -= dmg;
        CheckIfIsDeath();
        OnTakeDmg?.Invoke(attacker);
        if (tag=="Sticky")
        {
           if(AudioManager.instance != null) AudioManager.instance.PlayOneShot(FMODEvents.instance.smallEnemyTakesDamage, transform.position);
        }
        //Debug.Log("Take Dmg: " + dmg + " ", gameObject);
    }
    public void TakeHealth(float health)
    {
        actualHealth += health;
    }
    public void CheckIfIsDeath()
    {
        if (actualHealth <= 0) 
        {
            OnDeath?.Invoke();

            if (tag=="Sticky")
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
