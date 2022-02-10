using System.Collections;
using UnityEngine;

public class SetToCompleteOutsideLevel : MonoBehaviour {
    private ChoreItemBase _choreItem;

    private void Setup() {
        _choreItem = GetComponent<ChoreItemBase>();
        StartCoroutine(TrySetComplete());
    }
    
    private IEnumerator TrySetComplete() {
        while (transform.position.y > -100f) {
            yield return new WaitForSeconds(2);
        }

        if (!ReferenceEquals(_choreItem, null)) {
            _choreItem.ChoreCompleted = true;
            _choreItem.gameObject.SetActive(false);
        }
    }

    private void Awake() => Setup();
}
