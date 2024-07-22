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
        position.y = Mathf.Clamp(lift.transform.position.y, -60, float.MaxValue);
        transform.position = position;
    }


}
