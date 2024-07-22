using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDetector : MonoBehaviour
{

    [SerializeField] private CameraManager cameraManager;

    private Outline outline;

    public GameObject currentFocusedObject;
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
            currentFocusedObject = other.gameObject;
            outline = other.GetComponent<Outline>();
            outline.enabled = true;
            cameraManager.currentOutline = outline;
            cameraManager.SetWindowCameraPosition(other.transform.position);
        }

        if (other.gameObject.CompareTag("LunchBox")) {
            GameManager.Instance.canTakeBreak = true;
            currentFocusedObject = other.gameObject;
            outline = other.GetComponent<Outline>();
            outline.enabled = true;
            cameraManager.currentOutline = outline;
            cameraManager.SetWindowCameraPosition(other.transform.position);
        }
        
        if (other.gameObject.CompareTag("Toilet")) {
            GameManager.Instance.canTakeBreak = true;
            currentFocusedObject = other.gameObject;
            outline = other.GetComponent<Outline>();
            outline.enabled = true;
            cameraManager.currentOutline = outline;
            cameraManager.SetWindowCameraPosition(other.transform.position);
        }

    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Window")){
            GameManager.Instance.canClean = false;
            currentFocusedObject.GetComponent<Outline>().enabled = false;
            currentFocusedObject = null;
        }

        if (other.gameObject.CompareTag("LunchBox")) {
            GameManager.Instance.canTakeBreak = false;
            currentFocusedObject.GetComponent<Outline>().enabled = false;
            currentFocusedObject = null;
        }

        if (other.gameObject.CompareTag("Toilet")) {
            GameManager.Instance.canTakeBreak = false;
            currentFocusedObject.GetComponent<Outline>().enabled = false;
            currentFocusedObject = null;
        }
    }
}
