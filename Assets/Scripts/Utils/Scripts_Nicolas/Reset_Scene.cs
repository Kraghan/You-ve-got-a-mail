using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset_Scene : MonoBehaviour {

	public void ResetMeForChristSake () {

		SceneManager.LoadScene (0);

	}

	void OnTriggerEnter () {

		ResetMeForChristSake ();

	}
}
