using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Timer))]
public class TimerEditor : Editor
{
    SerializedProperty m_timeToReach;

    void OnEnable()
    {
        m_timeToReach = serializedObject.FindProperty("m_timeToReach");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_timeToReach);
        serializedObject.ApplyModifiedProperties();
    }
}
