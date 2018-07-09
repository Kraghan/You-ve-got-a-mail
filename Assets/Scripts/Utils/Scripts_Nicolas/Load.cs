using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour {

	public SoundManager Le_Wwise;
	public KeybordController Mouse_Sensibility;
	public PlayerController Bike_Sensibility;

	public Slider MusicPoints, MusicLeisure, MusicStory;
	public Slider SFXPoints, SFXLeisure, SFXStory;
	public Slider SensibilityPoints, SensibilityLeisure, SensibilityStory;
	public Slider MouseSensibilityPoints, MouseSensibilityLeisure, MouseSensibilityStory;

	public Material Velodor, velonormal;

	// Use this for initialization
	void Start () {

		Load_Save ();

	}

	public void Load_Save () {
		
		//Je charge les préférences des options du joueur
		MusicPoints.value = PlayerPrefs.GetFloat ("Music", 70f);
		MusicLeisure.value = PlayerPrefs.GetFloat ("Music", 70f);
		MusicStory.value = PlayerPrefs.GetFloat ("Music", 70f);

		SFXPoints.value = PlayerPrefs.GetFloat ("FX", 70f);
		SFXLeisure.value = PlayerPrefs.GetFloat ("FX", 70f);
		SFXStory.value = PlayerPrefs.GetFloat ("FX", 70f);

		SensibilityPoints.value = PlayerPrefs.GetFloat ("Sensibility", 5f);
		SensibilityLeisure.value = PlayerPrefs.GetFloat ("Sensibility", 5f);
		SensibilityStory.value = PlayerPrefs.GetFloat ("Sensibility", 5f);

		MouseSensibilityPoints.value = PlayerPrefs.GetFloat ("MouseSensibility", 2.5f);
		MouseSensibilityLeisure.value = PlayerPrefs.GetFloat ("MouseSensibility", 2.5f);
		MouseSensibilityStory.value = PlayerPrefs.GetFloat ("MouseSensibility", 2.5f);

		//J'applique le tout
		Le_Wwise.SetVolumeSFX (SFXLeisure);
		Le_Wwise.SetVolumeMusic (MusicLeisure);
		Mouse_Sensibility.SetMouseSensibility (MouseSensibilityLeisure);
		Bike_Sensibility.SetSensibility (SensibilityLeisure);

		if (PlayerPrefs.GetFloat ("Velodor", 0f) == 1f) {
			MeshRenderer[] levelo = GetComponentsInChildren<MeshRenderer> ();

			foreach (MeshRenderer boutdvelo in levelo) {
				boutdvelo.sharedMaterial = Velodor;
			}
		}
		else {
			MeshRenderer[] levelo = GetComponentsInChildren<MeshRenderer> ();

			foreach (MeshRenderer boutdvelo in levelo) {
				boutdvelo.sharedMaterial = velonormal;
			}
		}
	}

	public void Load_Leisure_Mailboxes () {

		foreach (VacuumMailBox laboite in ScoreManager.The_Mailboxes) {

			string testid;

			testid = "Mailbox_" + laboite.id;

			if (PlayerPrefs.GetFloat (testid, 0f) == 1f) {
				laboite.GetComponent<ScoreMailbox> ().m_alreadyAdded = false;
				laboite.SetDelivered (true);
				laboite.SetMaterial (laboite.m_deliveredMaterial);
			} else {
				laboite.GetComponent<ScoreMailbox> ().m_alreadyAdded = false;
				laboite.SetDelivered (false);
				laboite.SetMaterial (laboite.m_normalMaterial);
			}
		}
	}
}
