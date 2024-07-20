using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDetector : MonoBehaviour
{

    [SerializeField] private CameraManager cameraManager;

    public GameObject currentWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Window")){
            GameManager.Instance.canClean = true;
            currentWindow = other.gameObject;
            currentWindow.GetComponent<Outline>().enabled = true;

            cameraManager.SetWindowCameraPosition(other.transform.position);

        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Window")){
            GameManager.Instance.canClean = false;
            currentWindow.GetComponent<Outline>().enabled = false;
            currentWindow = null;
        }
    }
}
