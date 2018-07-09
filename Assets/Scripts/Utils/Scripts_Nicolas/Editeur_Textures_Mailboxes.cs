#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Color_mailboxes))]
public class Editeur_Textures_Mailboxes : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		Color_mailboxes myScript = (Color_mailboxes)target;

		if (GUILayout.Button ("Colorer_Mailboxes")) {
			myScript.Color_all_mailboxes ();

		}
	}
}
#endif