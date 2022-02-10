using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour {
    [SerializeField]
    private float timeInSeconds = 1f;

    private float _timeLeft;

    private void Setup() {
        _timeLeft = timeInSeconds;
    }

    private void TryToDisable() {
        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0) {
            gameObject.SetActive(false);
        }
    }

    private void Update() => TryToDisable();
    private void OnEnable() => Setup();
}