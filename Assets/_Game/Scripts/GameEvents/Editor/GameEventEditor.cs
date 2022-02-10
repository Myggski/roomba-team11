using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates a button to trigger events manually, good for manual testing
/// </summary>
[CustomEditor(typeof(GameEvent))]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;
        
        if (GUILayout.Button("Call")) {
            (target as GameEvent)?.Call();
        }
    }
}