using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Action<NPCAnimState> OnNpcStateChange;
    [SerializeField] UnityEngine.AI.NavMeshAgent agent;
    private NPCstate _npcState= NPCstate.Idle;
    [SerializeField] NPCstate npcStateEditor= NPCstate.Idle;
    [SerializeField] NPCAnimController animController;
    [SerializeField] List<PairPositionOrientation> idlePositions;
    [SerializeField] List<PairPositionOrientation> workStations;

    private NPCstate npcState
    {
        get { return _npcState; }
        set
        {
            if (_npcState != value)
            {
                _npcState = value;
                npcStateEditor = value;
                OnNPCStateChanged();
            }
        }
    }

    void Update()
    {

    }
    void OnEnable()
    {
        OnNpcStateChange+=animController.SetAnimation;
        animController.onWorkAnimationEnds+=ComeBackToIdlePosition;
        OnNPCStateChanged();
    }
    void OnDisable()
    {
        OnNpcStateChange-=animController.SetAnimation;
        animController.onWorkAnimationEnds-=ComeBackToIdlePosition;
    }
    void OnNPCStateChanged()
    {
        switch (npcState)
        {
            
            case NPCstate.Idle:
                agent.isStopped = true;
                StartCoroutine (CheckForRandomStateChange());
                OnNpcStateChange?.Invoke(NPCAnimState.Idle);
                break;
            case NPCstate.Working:
                StartCoroutine(GoToPoint(GetRandomDestination(workStations), NPCstate.Working));
                break;
            case NPCstate.ComingBackToIdle:
                StartCoroutine(GoToPoint(GetRandomDestination(idlePositions), NPCstate.Idle));
                break;
            case NPCstate.Interacting:
                break;

        }
    }
    
    void FollowState(Vector3 target)
    {
        agent.isStopped = false; 
        agent.destination = target;
        OnNpcStateChange?.Invoke(NPCAnimState.Walking);

    }
    void OnValidate()
    {
        npcState = npcStateEditor;
    }

    IEnumerator GoToPoint(PairPositionOrientation destiny, NPCstate nPCstateTarget)
    {
        FollowState(destiny.positionTransform.transform.position);
        yield return StartCoroutine(CheckDistanceCoroutine());
        yield return StartCoroutine(RotateTowards(destiny.orientationTransform.transform.position));
        if (nPCstateTarget == NPCstate.Working)
        {
            OnNpcStateChange?.Invoke(NPCAnimState.Working);
        }
        else if (nPCstateTarget == NPCstate.Idle)
        {
            OnNpcStateChange?.Invoke(NPCAnimState.Idle);
            npcState=NPCstate.Idle;
        }
        
    }

    

    IEnumerator CheckDistanceCoroutine(float maxStoppedTime = 1f)
    {
        float stoppedTime = 0f;
        float previousRemainingDistance = agent.remainingDistance;

        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
        
            if (Mathf.Approximately(agent.remainingDistance, previousRemainingDistance))
            {
                stoppedTime += Time.deltaTime;
            }
            else
            {
                stoppedTime = 0f; 
            }

            
            if (stoppedTime >= maxStoppedTime)
            {
                
                yield break;
            }

            previousRemainingDistance = agent.remainingDistance;
            yield return null;
        }
    }

    IEnumerator RotateTowards(Vector3 targetPosition)
    {
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 10f)
        {
            Vector3 crossProduct = Vector3.Cross(transform.forward, direction);
            float dotProduct = Vector3.Dot(crossProduct, Vector3.up);

            if (dotProduct > 0)
            {
                animController.SetAnimation(NPCAnimState.RotatingRight);
            }
            else if (dotProduct < 0)
            {
                animController.SetAnimation(NPCAnimState.RotatingLeft);
            }
            

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100f);
            yield return null;
        }
       
    }

    PairPositionOrientation GetRandomDestination(List<PairPositionOrientation> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("No hay estaciones de trabajo disponibles.");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    void ComeBackToIdlePosition()
    {
        npcState=NPCstate.ComingBackToIdle;
    }
    IEnumerator CheckForRandomStateChange()
    {
        yield return new WaitForSeconds(5); 
        while (true)
        {
            float randomValue = UnityEngine.Random.value;
            if (randomValue <= 0.25f)
            {
                npcState = NPCstate.Working;
                yield break; 
            }
            else
            {
                
                yield return new WaitForSeconds(5); 
            }
        }
    }
}

enum NPCstate
{
    Idle,
    Working,
    Interacting,
    ComingBackToIdle,
}
[System.Serializable]
public class PairPositionOrientation
{
    public bool isSit;
    public GameObject positionTransform;
    public GameObject orientationTransform;
    
}

