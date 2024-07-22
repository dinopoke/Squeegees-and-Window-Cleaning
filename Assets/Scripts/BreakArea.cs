using System.Collections;
using UnityEngine;

public class BreakArea : MonoBehaviour {

    private Coroutine itemAvailable;
    private float enableTime = 1.5f;

    [SerializeField] private GameObject item;

    public void ToggleItem(bool enable) {
        MakeClickable(false);
        if (itemAvailable != null) StopCoroutine(itemAvailable);
        if (enable) itemAvailable = StartCoroutine(TogglingItem());
    }

    private IEnumerator TogglingItem() {
        yield return new WaitForSeconds(enableTime);
        MakeClickable(true);
    }

    private void MakeClickable(bool enable) {

        if (item.gameObject.GetComponent<Food>() != null) {
            item.GetComponent<Food>().enabled = enable;
            return;
        }

        if (item.gameObject.GetComponent<Toilet>() != null) {
            item.GetComponent<Toilet>().enabled = enable;
            return;
        }

    }
}
