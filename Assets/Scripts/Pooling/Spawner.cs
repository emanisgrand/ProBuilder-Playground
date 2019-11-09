using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    private PoolManager pm = PoolManager.Instance;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
           GameObject bullet =  pm.RequestBullet();
        }
        
    }
}
