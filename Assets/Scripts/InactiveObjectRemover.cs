using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor;

[CustomEditor(typeof(InactiveObjectRemover))]
public class InactiveObjectRemoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InactiveObjectRemover myScript = (InactiveObjectRemover)target;

        if (GUILayout.Button("Remove"))
        {
            myScript.Remove();
        }
    }
}

#endif
    public class InactiveObjectRemover : MonoBehaviour {

    public void Remove()
    {
        Transform[] aTransforms = GetComponentsInChildren<Transform>(true);
        print(aTransforms.Length);
        
        for(int i = 0; i < aTransforms.Length; ++i)
        {
            if(!aTransforms[i].gameObject.activeSelf)
            {
                DestroyImmediate(aTransforms[i].gameObject);
            }
        }

    }
}
