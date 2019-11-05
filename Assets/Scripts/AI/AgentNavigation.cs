using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentNavigation : MonoBehaviour {
    // Inspector assigned variables
    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;
    public AnimationCurve JumpCurve = new AnimationCurve();

    // Private members
    public NavMeshAgent _navAgent = null;

    // Start is called before the first frame update
    void Start() {
        _navAgent = GetComponent<NavMeshAgent>();
        //* if waypoint network has no transforms assigned to it, don't error 
        if (WaypointNetwork == null) return;
        SetNextDestination(false);
    }

    public void SetNextDestination(bool increment) {
        if (!WaypointNetwork) return;

        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;
        //* set the next waypoint as the one that that follows numerically. Otherwise, don't increment.
        int nextWayPoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count)
            ? 0
            : CurrentIndex + incStep;
        nextWaypointTransform = WaypointNetwork.Waypoints[nextWayPoint];

        //* as long as the location data for the next destination isn't empty, 
        //* tell the navAgent that its next destination is the next waypoint.
        if (nextWaypointTransform != null) {
            CurrentIndex = nextWayPoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }
    }

    // Update is called once per frame
    void Update() {
        //* keep all of the agents navigation information current
        HasPath = _navAgent.hasPath;
        PathPending = _navAgent.pathPending;
        PathStale = _navAgent.isPathStale;
        PathStatus = _navAgent.pathStatus;

        //set up OffMeshLink
        if (_navAgent.isOnOffMeshLink)
            if (_navAgent.isOnOffMeshLink) {
                StartCoroutine(Jump(1.0f));
                return;
            }

        if ((_navAgent.remainingDistance <= _navAgent.stoppingDistance && !PathPending)
            || PathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestination(true);
        else if (_navAgent.isPathStale)
            SetNextDestination(false);

        IEnumerator Jump(float duration) {
            OffMeshLinkData data = _navAgent.currentOffMeshLinkData;
            Vector3 startPos = _navAgent.transform.position;
            Vector3 endPos = data.endPos + (_navAgent.baseOffset * Vector3.up);
            float time = 0.0f;

            while (time < duration) {
                float t = time / duration;
                _navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + (JumpCurve.Evaluate(t) * Vector3.up);
                time += Time.deltaTime;
                yield return null;
            }

            _navAgent.CompleteOffMeshLink();
        }
    }
}