using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset_Scene : MonoBehaviour {

	public Transform Velo;
	private Vector3 Pos_ini;
	private Quaternion Rot_ini;
	private float time, wait = 2f;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
		
	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}


	// Use this for initialization
	void Start () {
		Pos_ini = Velo.position;
		Rot_ini = Velo.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetPress (SteamVR_Controller.ButtonMask.Grip))
			time += Time.deltaTime;
		else
			time = 0;

		if (time >= wait) {

			Velo.position = Pos_ini;
			Velo.rotation = Rot_ini;
			time = 0;

		}
			
		
	}

}

