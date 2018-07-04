using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset_Scene : MonoBehaviour {

	public GameObject FuckinWwise;

	private void Awake () {

		//Je m'assure que j'ai pas deux Wwwise qui marchent en même temps
		AkSoundEngine.StopAll();

	}

	public void ResetMeForChristSake () {

		//Je reset l'Arduino
		GetComponent<PlayerController> ().OnRestart ();
		//Je recharge la scène
		SceneManager.LoadScene (0);

	}
}
