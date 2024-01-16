using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    [SerializeField] StateBoss[] states;
    StateBoss currentState;
    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        foreach (StateBoss item in states)
        {
            item.bossStateMachine = this;
        }
    }
    private void OnEnable()
    {
        health.OnDeath += OnDeathBoss;
    }

    private void OnDisable()
    {
        health.OnDeath -= OnDeathBoss;
    }

    private void Start()
    {
        ChangeState(states[0]);
    }

    public void ChangeToWalkState()
    {
        ChangeState(states[0]);
    }

    public void RandomState()
    {
        List<StateBoss> temp = new List<StateBoss>();

        for (int i = 1; i < states.Length; i++) //compruebo cuantos stados no tienen cooldown, empiezo desde el 0 porqe es el idle
        {
            if(!states[i].OnColdDown) temp.Add(states[i]);
        }

        if(temp.Count == 0) // si todos tienen cooldown, ejecuto el idlestate para esperar 2seg 
        {
            ChangeState(states[0]);
            return;
        }

        ChangeState(temp[Random.Range(0, temp.Count)]); // si existe uno aunquesea manda random;
    }

    void ChangeState(StateBoss _newState)
    {
        if(currentState!=null) currentState.enabled = false;
        currentState = _newState;
        currentState.enabled = true;
    }

    void OnDeathBoss()
    {
        Destroy(gameObject);
    }


    
}
public class StateBoss:MonoBehaviour
{
    [HideInInspector]public BossStateMachine bossStateMachine;
    public bool OnColdDown;
    public float coolDown;
}

public enum States
{
    Walk,Attack,Skill1,Skill2
}
