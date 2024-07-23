using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLine : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public List<GameObject> attachedObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < attachedObjects.Count; i++)
        {
            if (i < lineRenderer.positionCount && attachedObjects != null)
            { lineRenderer.SetPosition(i, attachedObjects[i].transform.position); }
        }
    }
}
