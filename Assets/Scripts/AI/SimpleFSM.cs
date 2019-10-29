using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.ThirdPerson;

public class SimpleFSM : FSM {
    public enum FSMState {
        None,
        Patrol,
        Chase, 
        Attack,
        Dead,
    }

    // Current state that the NPC is reaching
    public FSMState currentState;
    
    private NavMeshAgent _agent;
    private AgentNavigation _agentNavigation;
    

    protected override void Initialize() {
        // AICharacterControl = GetComponent<AICharacterControl>();
        _agentNavigation = GetComponent<AgentNavigation>();
        _agent = GetComponent<NavMeshAgent>();
        currentState = FSMState.Patrol;
        // Reference to Player object
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = objPlayer.transform;
    }

    protected override void FSMUpdate() {
        switch (currentState) {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
        }
        
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, Single.MaxValue);
    }

    /// <summary>
    /// Patrol State
    /// </summary>
    private void UpdatePatrolState() {
        _agent.speed = 0.5f;
        // increment to the next waypoint
        _agentNavigation.PathStale = false;
        // AICharacterControl.SetTarget(null);
        if (Vector3.Distance(transform.position, PlayerTransform.position) <= 6f) {
            currentState = FSMState.Chase;
        }
    }
    /// <summary>
    /// Chase State
    /// </summary>
    private void UpdateChaseState() {
        // stop incrementing waypoints.
        _agentNavigation.PathStale = true;
        
        //todo- No longer using AICharacterControl call in FSM script or the simple FSM.
        // AICharacterControl.SetTarget(PlayerTransform);
        _agent.speed = 0.88f;
        float dist = Vector3.Distance(transform.position, PlayerTransform.position);        
        
        if (dist >= 7f) {
            currentState = FSMState.Patrol;
        }
    }
}