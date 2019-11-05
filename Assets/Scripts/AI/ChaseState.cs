using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ChaseState : FSMState {
    public ChaseState(Transform[] wp) {
        waypoints = wp;
        stateID = FSMStateID.Chasing;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint(true);
    }

    public override void Reason(Transform player, Transform npc) {
        //Set the target position as the player position
        destPos = player.position;

        //Check the distance with player. When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 20.0f) {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<AICharacterAFSMControl>().SetTransition(Transition.ReachPlayer);
        }
        //Go back to patrol is it become too far
        else if (dist >= 30.0f) {
            npc.GetComponent<AICharacterAFSMControl>().target = null;
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<AICharacterAFSMControl>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc) {
            // I don't think we need both a destPos and a target.
            destPos = player.position;
            npc.GetComponent<AICharacterAFSMControl>().target = player.transform;
            
            // Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
            // npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);
    }
}