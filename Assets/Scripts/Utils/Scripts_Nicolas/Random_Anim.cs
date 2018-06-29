using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Anim : MonoBehaviour {

	bool playAnim = true;
	public Animation animation;

	void Update(){
		if (playAnim) {
			animation.Play("Yawning");  //Put your animation string
			StartCoroutine(WaitAnim()); //wait random seconds for animation
		}
	}

	public IEnumerator WaitAnim()
	{
		playAnim = false;              
		int randomWait = Random.Range(0, 10);                
		//print ("Time" + randomWait + " Play"); //debug                
		yield return new WaitForSeconds(randomWait);                
		animation.Play("Yawning");  //Put your animation string
		playAnim = true;
	}
}