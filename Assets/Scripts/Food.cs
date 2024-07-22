using UnityEngine;

public class Food : MonoBehaviour {

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform == transform) {
                    OnClick();
                }
            }
        }
    }

    private void OnClick() {
        Debug.Log("Food object clicked!");
    }
}