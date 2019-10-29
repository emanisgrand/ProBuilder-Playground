using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class FSM : MonoBehaviour {
    // Player Transform
    protected Transform PlayerTransform;
    
    // next Destination position of the agent
    protected Vector3 DestPos;
    
    // List of points for patrolling
    protected List<Transform> pointList = new List<Transform>();
    
    // Any kind of weapon specifics would go here
    // public Transform weaponTransform {get; set;}

    protected virtual void Initialize() {}
    protected virtual void FSMUpdate() {}
    protected  virtual void FSMFixedUpdate() {}

    private void Start() {
        Initialize();
    }

    private void Update() {
        FSMUpdate();
    }

    private void FixedUpdate() {
        FSMFixedUpdate();
    }
}