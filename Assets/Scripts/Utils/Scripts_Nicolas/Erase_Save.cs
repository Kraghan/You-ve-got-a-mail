using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Erase_Save : MonoBehaviour {

	// Update is called once per frame
	void Update () {

		if ((Input.GetKey (KeyCode.Delete)) && (Input.GetKey (KeyCode.E)) && (Input.GetKey (KeyCode.R))) {
			//Je réinitialise le fichier de sauvegarde
			PlayerPrefs.SetFloat ("FX", 70f);
			PlayerPrefs.SetFloat ("Music", 70f);
			PlayerPrefs.SetFloat ("Sensibility", 5f);
			PlayerPrefs.SetFloat ("MouseSensibility", 2.5f);
			PlayerPrefs.SetFloat ("Velodor", 0f);

			//Je réinitialise les boites aux lettres en mode ballade
			foreach (VacuumMailBox laboite in ScoreManager.The_Mailboxes) {

				string testid;
				testid = "Mailbox_" + laboite.id;

				PlayerPrefs.SetFloat (testid, 0f);

			}

			//Je dis que j'ai trouvé aucune mailbox
			ScoreMailbox.s_totalmailbox = 0;
			ScoreManager.onceleisure = false;

			//Je recharge les paramètres du jeu
			GetComponent<Load> ().Load_Save ();

			//Je recharge les boites aux lettres du jeu en mode ballade
			if (Mode_selector.m_defaultPlayMode == Mode_selector.MyPlayMode.LEISURE)
				GetComponent<Load> ().Load_Leisure_Mailboxes ();
		}
	}
}
