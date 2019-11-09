using System;
using System.Collections;
using System.Collections.Generic;
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
        _instance = null;
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
}