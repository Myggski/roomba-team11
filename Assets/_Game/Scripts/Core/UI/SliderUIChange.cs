using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SliderUIChange : MonoBehaviour {
    [SerializeField]
    private string sliderName;
    [SerializeField]
    private UnityEvent onSliderChange;
    [SerializeField]
    private FloatVariable sliderValue;

    private UIDocument _uiDocument;
    private Slider _slider;

    private void Setup() {
        _uiDocument = GetComponent<UIDocument>();
        _slider = _uiDocument.rootVisualElement.Q<Slider>(sliderName);
        _slider.value = sliderValue.Value;

        if (!ReferenceEquals(_slider, null))
        {
            _slider.RegisterValueChangedCallback(OnSliderChange);    
        }
    }

    private void Clear() {
        if (!ReferenceEquals(_slider, null)) {
            _slider.UnregisterValueChangedCallback(OnSliderChange);
        }
    }

    private void OnSliderChange(ChangeEvent<float> evt) {
        sliderValue.ChangeValue(evt.newValue);
        onSliderChange?.Invoke();
    }

    private void OnEnable() => Setup();
    private void OnDisable() => Clear();
}