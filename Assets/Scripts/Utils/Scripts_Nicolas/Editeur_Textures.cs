#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Texture_random))]
public class Editeur_Textures : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		Texture_random myScript = (Texture_random)target;

		if (GUILayout.Button ("Changer_Textures")) {
			myScript.Color_them_all ();
		}

		if (GUILayout.Button ("Trollolo_Saruman")) {
			myScript.Saruman_the_multicolored ();
		}

		if (GUILayout.Button ("Random_Height")) {
			myScript.Change_height ();
		}

		if (GUILayout.Button ("Random_Height")) {
			myScript.Rotate_them_all ();
		}

	}
}
#endif