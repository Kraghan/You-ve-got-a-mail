using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Door : MonoBehaviour {

	public Collider Opener;
	public Transform thedoor;
	public float height;
	public float speed;

	private float originalheight;
	private bool opened;

	void Start () {

		originalheight = thedoor.transform.position.y;

	}

	void Openthedoor () {

		if (thedoor.transform.position.y <= originalheight + height) {

			thedoor.transform.position += Vector3.up * speed * Time.deltaTime;

		}
	}

	void Closethedoor () {

		if (thedoor.transform.position.y >= originalheight) {

			thedoor.transform.position -= Vector3.up * speed * Time.deltaTime;

		}
	}

	void OnTriggerEnter (Collider player) {

		if (player.gameObject.tag == "Player") {
			
			opened = true;

		}

	}

	void OnTriggerExit (Collider playertoo) {

		if (playertoo.gameObject.tag == "Player") {
			
			opened = false;

		}

	}

	void Update () {

		if (opened)
			Openthedoor ();
		else
			Closethedoor ();

	}
}
