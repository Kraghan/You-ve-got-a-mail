using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset_Scene : MonoBehaviour {

	public GameObject FuckinWwise;

	private void Awake () {

		AkSoundEngine.StopAll();

	}

	public void ResetMeForChristSake () {

		SceneManager.LoadScene (0);

	}
}
