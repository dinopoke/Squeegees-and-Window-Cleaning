using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDetector : MonoBehaviour
{

    [SerializeField] private CameraManager cameraManager;

    private Outline outline;

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
            outline = other.GetComponent<Outline>();
            outline.enabled = true;
            cameraManager.currentOutline = outline;
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
