using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterAFSMControl : AdvancedFSM
    {
        public AIWaypointNetwork waypointNetwork = null; // waypoint network ref
        
        // not sure I need this
        public Transform target; // target to aim for

        public int CurrentIndex = 0;
        public bool HasPath = false;
        public bool PathPending = false;
        public bool PathStale = false;
        public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;
        
        protected override void Initialize()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            NavAgent = GetComponentInChildren<NavMeshAgent>();
            waypointNetwork = FindObjectOfType<AIWaypointNetwork>();
            NavAgent.updateRotation = false;
	        NavAgent.updatePosition = true;
            
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
        
        protected override void FSMUpdate() {
            HasPath = NavAgent.hasPath;
            PathPending = NavAgent.pathPending;
            PathStale = NavAgent.isPathStale;
            PathStatus = NavAgent.pathStatus;
            
            if (target != null)
                NavAgent.SetDestination(target.position);

            if ((NavAgent.remainingDistance > NavAgent.stoppingDistance && !PathPending) 
                || PathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestination(true);
            else if (NavAgent.isPathStale)
                SetNextDestination(false);
        }
        
        public void SetNextDestination(bool increment) {
            if (!waypointNetwork) return;

            int incStep = increment ? 1 : 0;
            Transform nextWaypointTransform = null;
            //* set the next waypoint as the one that that follows numerically. Otherwise, don't increment.
            int nextWayPoint = (CurrentIndex + incStep >= waypointNetwork.Waypoints.Count)
                ? 0
                : CurrentIndex + incStep;
            nextWaypointTransform = waypointNetwork.Waypoints[nextWayPoint];

            //* as long as the location data for the next destination isn't empty, 
            //* tell the navAgent that its next destination is the next waypoint.
            if (nextWaypointTransform != null) {
                CurrentIndex = nextWayPoint;
                NavAgent.destination = nextWaypointTransform.position;
                return;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
