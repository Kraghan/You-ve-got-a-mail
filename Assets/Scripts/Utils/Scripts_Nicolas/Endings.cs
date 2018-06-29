using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Endings : MonoBehaviour {

	public ScoreManager The_Scores;

	public Material Velodor;

	public GameObject Panne, Leisure_UI, Story_UI;
	public GameObject Points_Ending, Leisure_Ending, Story_Ending;
	public Text Points, Total_Time, Bonus_Time, Balanced_Time;

	public void PointsEnding() {
		
		Points.text = The_Scores.GetPointsText ();

		Panne.SetActive (false);
		Points_Ending.SetActive (true);

	}

	public void LeisureEnding() {

		MeshRenderer[] levelo = GetComponentsInChildren<MeshRenderer> ();

		foreach (MeshRenderer boutdvelo in levelo) {
			boutdvelo.sharedMaterial = Velodor;
		}

		Leisure_UI.SetActive (false);
		Leisure_Ending.SetActive (true);

	}

	public void StoryEnding() {

		Total_Time.text = The_Scores.GetTimeText ();
		Bonus_Time.text = The_Scores.GetBonusText ();
		Balanced_Time.text = The_Scores.GetBalancedText ();

		Story_UI.SetActive (false);
		Story_Ending.SetActive (true);
	}

}
