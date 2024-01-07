using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float actualHealth;
    public System.Action OnDeath;
    public System.Action OnTakeDmg;

    private void OnEnable()
    {
        actualHealth = maxHealth;
    }
    public void TakeDamage(float dmg)
    {
        actualHealth -= dmg;
        CheckIfIsDeath();
        OnTakeDmg?.Invoke();
        //Debug.Log("Take Dmg: " + dmg + " ", gameObject);
    }
    public void TakeHealth(float health)
    {
        actualHealth += health;
    }
    public void CheckIfIsDeath()
    {
        if (actualHealth <= 0) OnDeath?.Invoke();
    }
}
