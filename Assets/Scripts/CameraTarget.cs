using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    [SerializeField] private GameObject lift;

    private Vector3 position;

    private void Awake() {
        position = transform.position;
    }

    private void Update() {
        position.y = lift.transform.position.y;
        transform.position = position;
    }


}
