using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;


public class PoolManager : MonoBehaviour {
    
    private static PoolManager _instance;
    public static PoolManager Instance {
        get {
            if (_instance == null)
                Debug.Log("The PoolManager is null");
            return _instance;
        }
    }

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private GameObject _bulletContainer;
    
    [SerializeField] private List<GameObject> _bulletPool;

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        _bulletPool = GenerateBullets(10);
    }

    List<GameObject> GenerateBullets(int amountOfBullets) {
        for (int i = 0; i < amountOfBullets; i++) {
            GameObject bullet = Instantiate(_bulletPrefab);
            bullet.transform.parent = _bulletContainer.transform;
            bullet.SetActive(false);
            
            _bulletPool.Add(bullet);
        }

        return _bulletPool;
    }

    public GameObject RequestBullet() {
        // loop through the bullet list - [x]
        foreach (var bullet in _bulletPool) {
            // check for in-active bullet - [x]
            if (bullet.activeInHierarchy == false) {
                // set it active - [x] and return it to the player [x]
                bullet.SetActive(true);
                return bullet;
            }
        }
        // if we made it to this point...we need to generate more bullets
        // SO if no bullets are available (all are active) [x]
        // generate x amount of bullets, run the request bullet method [x]
        GameObject newBullet = Instantiate(_bulletPrefab);
        newBullet.transform.parent = _bulletContainer.transform;
        _bulletPool.Add(newBullet);
        
        return newBullet;
    }
    
}