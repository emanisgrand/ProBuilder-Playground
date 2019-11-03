using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : AdvancedFSM
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public AIWaypointNetwork waypointNetwork { get; private set; } // waypoint network ref
        public Transform target; // target to aim for
        

        protected override void Initialize()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            
            waypointNetwork = FindObjectOfType<AIWaypointNetwork>();
            agent.updateRotation = false;
	        agent.updatePosition = true;

            ConstructFSM();
        }
        
        public void SetTransition(Transition t) {
            PerformTransition(t);
        }

        private void ConstructFSM() {
            pointList = waypointNetwork.Waypoints;
            
            Transform[] waypoints = new Transform[pointList.Count];
            int i = 0;
            
            // this is the same as AgentNavigation waypoint finder
            foreach (Transform obj in pointList) {
//                waypoints[i] = obj.transform;
                i++;
            }
            
            /*
             * Set all of the states and transitions.
             */
            ChaseState chase = new ChaseState(waypoints);
            chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
            chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
        }
        
        protected override void FSMUpdate()
        {
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
