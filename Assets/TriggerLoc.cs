using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoc : MonoBehaviour {
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EditorOnly"){ 
        Debug.Log("piss");
        anim.SetTrigger("popped");
        }
    }

}
