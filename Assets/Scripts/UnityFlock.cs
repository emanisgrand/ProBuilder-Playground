using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnityFlock : MonoBehaviour {
    public float minSpeed = 20.0f;
    public float turnSpeed = 20.0f;
    // how many times to update randomPush based on randomForce
    public float randomFreq = 20.0f; 
    public float randomForce = 20.0f; 
    
    //--Alignment values
    public float toOriginForce = 50.0f;
    // how much the flock will spread out
    public float toOriginRange = 100.0f;

    public float gravity = 2.0f;
    
    //-- seperation variables
    public float avoidanceRadius = 50.0f;
    public float avoidanceForce = 20.0f;
    // these two maintain a minimum distance between individual boids
    
    //cohesion variables
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;
    
    //these variables control the movement of the boid
    private Transform origin;
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Transform[] objects;
    private UnityFlock[] otherFlocks;
    private Transform transformComponent;

    private void Start() {
        randomFreq = 1.0f / randomFreq;
        
        //Assign the parent as orgin
        // this will be the controller object for the other boids to follow
        origin = transform.parent;
        
        //Flock transform
        transformComponent = transform;
        
        // Temporary components
        Component[] tempFlocks = null;
        
        // Get all the unity flock components from the parent transform in the group
        if (transform.parent) {
            tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
        }
        
        // Assign and store all the flock objects in this group
        // store them in our own variables for later reference
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new UnityFlock[tempFlocks.Length];

        for (int i = 0; i < tempFlocks.Length; i++) {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (UnityFlock) tempFlocks[i];
        }
        
        //Null Parent as the flock leader will be UnityFlockController object 
        transformComponent.parent = null;
        
        //Calculating random push depends on the random frequency provided
        StartCoroutine(UpdateRandom());
    }

    IEnumerator UpdateRandom() {
        while (true) {
            // insideUnitSphere returns a random vector3 within a sphere radius of the randomForce value
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreq + Random.Range(-randomFreq/2.0f, randomFreq/2.0f));
        }
    }

    private void Update() {
        // internal variables
        float speed = velocity.magnitude;
        Vector3 avgVeclocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        float count = 0;
        float f = 0.0f;
        float d = 0.0f;
        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;
        Vector3 wantedVel;

        for (int i = 0; i < objects.Length; i++) {
            Transform transform = objects[i];
            if (transform != transformComponent) {
                Vector3 otherPosition = transform.position;
                
                //Average position to calculate cohesion
                avgPosition += transform.position;
                count++;
                
                //Directional vector from other flock to this flock
                forceV = myPosition - otherPosition;
                
                //Magnitude of that directional vector (Length)
                d = forceV.magnitude;
                
                //Add the push value if the magnitude, the length of the vector,
                // is less than the followRadius to the leader
                if (d < followRadius) {
                    //calculate the velocity, the speed of the object, based on
                    // the avoidance distance between flocks if the current magnitude is less
                    // than the specified avoidance radius
                    if (d < avoidanceRadius) {
                        f = 1.0f - (d / avoidanceRadius);
                        if (d > 0) avgVeclocity += (forceV / d) * f * avoidanceForce;
                    }
                    
                    // just keep the current distance with the leader
                    f = d / followRadius;
                    UnityFlock otherSeagull = otherFlocks[i];
                    // we normalize the otherSeagull velocity vector to get 
                    // the direction of movement, then we set a new velocity
                    avgVeclocity += otherSeagull.normalizedVelocity * f * followVelocity;
                    
                }
            }
        }
    }
}