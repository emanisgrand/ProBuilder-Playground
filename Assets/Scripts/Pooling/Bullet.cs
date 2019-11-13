using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    // Start is called before the first frame update
    private void OnEnable() {
        Invoke("Hide", 1);
    }

    void Hide() {
        this.gameObject.SetActive(false);
    }
}
