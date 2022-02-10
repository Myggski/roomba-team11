using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TimeDisplayer : MonoBehaviour {
    [SerializeField]
    private FloatVariable totalTimePlayed;
    [SerializeField]
    private GameEvent onTimeOut;

    private UIDocument _UIDocument;
    private Label _timerText;

    private void Setup() {
        _UIDocument = GetComponent<UIDocument>();
        _timerText = _UIDocument.rootVisualElement.Q<Label>("TimeText");
    }

    private string TimerText() {
        float minutes = Mathf.FloorToInt(totalTimePlayed.Value / 60);
        float seconds = Mathf.FloorToInt(totalTimePlayed.Value % 60);

        return $"{minutes:00}:{seconds:00}";
    }

    private void UpdateText() {
        totalTimePlayed.ApplyValue(Time.deltaTime);
        _timerText.text = TimerText();
    }

    private void OnEnable() => Setup();
    private void Update() => UpdateText();
}
