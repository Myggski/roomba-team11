using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ButtonUIClick : MonoBehaviour {
    [SerializeField]
    private string buttonName;

    [SerializeField]
    private UnityEvent onButtonClick;

    private UIDocument _uiDocument;
    private Button _button;

    private void Setup() {
        _uiDocument = GetComponent<UIDocument>();
        _button = _uiDocument.rootVisualElement.Q<Button>(buttonName);

        if (!ReferenceEquals(_button, null))
        {
            _button.clicked += OnButtonClicked;    
        }
    }

    private void Clear() {
        if (!ReferenceEquals(_button, null)) {
            _button.clicked -= OnButtonClicked;
        }
    }

    private void OnButtonClicked() {
        onButtonClick?.Invoke();
    }

    private void OnEnable() => Setup();

    private void OnDisable() => Clear();
}
