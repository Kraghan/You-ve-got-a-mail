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
	}
}
#endif