using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TaskListDisplayer : MonoBehaviour {
    [SerializeField]
    private List<RuntimeSetBase> sets;
    [SerializeField]
    private GameEventChoreItemSet onTaskCompleted;
    [SerializeField]
    private GameEvent onGameOver;
    
    [Header("Audio")] 
    [SerializeField] 
    private string _onTaskCompletedSound;
    [SerializeField] 
    private string _onGameOverSound;
    [SerializeField] 
    private Transform _playerTr;

    private UIDocument _uiDocument;
    private VisualElement _taskList;
    private int totalTasksCompleted = 0;

    private Dictionary<Type, VisualElement> _elements =
        new Dictionary<Type, VisualElement>();

    private VisualElement CreateTaskItem(string taskName, string taskValue) {
        VisualElement taskItem = new VisualElement();
        taskItem.AddToClassList("task-item");

        Label taskItemName = new Label(taskName);
        taskItemName.AddToClassList("task-item-name");
        taskItemName.name = "TaskName";
        
        Label taskItemValue = new Label(taskValue);
        taskItemValue.AddToClassList("task-item-value");
        taskItemValue.name = "TaskValue";

        VisualElement taskItemBar = new VisualElement();
        taskItemBar.AddToClassList("task-item-bar");
        
        taskItem.Add(taskItemName);
        taskItem.Add(taskItemValue);
        taskItem.Add(taskItemBar);

        return taskItem;
    }

    public void UpdateTaskItem(RuntimeSetBase set) {
        SetTaskValue(set, GetTaskValue(set.NumberOfChoreItemsCompleted, set.NumberOfChoreItems));

        if (set.NumberOfChoreItemsCompleted == set.NumberOfChoreItems) {
            SetTaskAsComplete(set);
            totalTasksCompleted++;
            onTaskCompleted?.Call(set);
            Sounds.PlaySound(Sounds.CreateSoundEvent(_onTaskCompletedSound, _playerTr));
        }

        if (totalTasksCompleted == sets.Count) {
            onGameOver?.Call();
            Sounds.PlaySound(Sounds.CreateSoundEvent(_onGameOverSound, _playerTr));
        }
    }

    private void SetTaskAsComplete(RuntimeSetBase set) {
        _elements.TryGetValue(set.GetType(), out VisualElement taskItem);

        if (!ReferenceEquals(taskItem, null)) {
            taskItem.AddToClassList("task-completed");
        }
    }

    private void SetTaskValue(RuntimeSetBase set, string taskValue) {
        _elements.TryGetValue(set.GetType(), out VisualElement taskItem);

        if (!ReferenceEquals(taskItem, null)) {
            taskItem.Q<Label>("TaskValue").text = taskValue;
        }
    }

    private string GetTaskValue(int currentValue, int maxValue) {
        return $@"{currentValue}/{maxValue}";
    }
    
    private void Setup() {
        _uiDocument = GetComponent<UIDocument>();
        _taskList = _uiDocument.rootVisualElement.Q<VisualElement>("TaskList");

        StartCoroutine(LoadUI());
    }

    private IEnumerator LoadUI() {
        // Wait a frame to make sure that the sets is being loaded
        yield return null;

        foreach (var set in sets) {
            VisualElement taskItem = CreateTaskItem(set.TaskName,GetTaskValue(set.NumberOfChoreItemsCompleted, set.NumberOfChoreItems));
            _elements.Add(set.GetType(), taskItem);
            _taskList.Add(taskItem);
        }
        
        foreach (var element in _elements) {
            _taskList.Add(element.Value);
        }
    }
    
    private void Start() => Setup();
}
