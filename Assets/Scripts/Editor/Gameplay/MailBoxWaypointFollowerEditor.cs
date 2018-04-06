using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MailBoxWaypointFollower))]
public class MailBoxWaypointFollowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MailBoxWaypointFollower myScript = (MailBoxWaypointFollower)target;
        if(myScript.HasWaypointContainer())
        {
            if (GUILayout.Button("Add waypoint"))
            {
                myScript.AddWaypoint();
            }
        }
    }
}