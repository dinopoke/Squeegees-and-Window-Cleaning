using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDetector : MonoBehaviour
{
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
            other.gameObject.GetComponent<CleanableWindow>().enabled = true;
            other.gameObject.GetComponent<Outline>().enabled = true;

        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Window")){
            other.gameObject.GetComponent<CleanableWindow>().enabled = false;
            other.gameObject.GetComponent<Outline>().enabled = false;

        }
    }
}
