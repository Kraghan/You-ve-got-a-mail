#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generation_procedurale))]
public class Editeur_generation_procedurale : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		Generation_procedurale myScript = (Generation_procedurale)target;

		if (GUILayout.Button ("Corriger chemin")) {
			myScript.Recur_Triche ();
		}

		if (GUILayout.Button ("Etendre chemin")) {
			myScript.GrowPath ();
		}

		if (GUILayout.Button ("Construire poteaux")) {
			myScript.BuildPylones ();
		}

		if (GUILayout.Button ("Construire bâtiments")) {
			myScript.BuildBuildings ();
		}

		if (GUILayout.Button ("Rotation bâtiments")) {
			myScript.RotateBuildings ();
		}

	}
}
#endif