using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
           GameObject bullet =  PoolManager.Instance.RequestBullet();
           //note that if I comment out the location info below,
           // the bullet var becomes inactive (null)
           bullet.transform.position = Vector3.zero;
        }
        
    }
}
