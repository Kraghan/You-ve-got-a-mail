using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God_move : MonoBehaviour {

	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool smooth;
	public float smoothTime = 5f;
	public float speed = 1;

	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;
	public Transform macamera;
	public Transform character;

	// Use this for initialization
	void Start () {
		m_CameraTargetRot = macamera.transform.localRotation;
		m_CharacterTargetRot = character.localRotation;
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, -90, 90);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Cursor.lockState = CursorLockMode.Locked;
		// Hide cursor when locking
		Cursor.visible = (CursorLockMode.Locked != CursorLockMode.Locked);

		if (Input.GetKey(KeyCode.Escape))
			Cursor.lockState = CursorLockMode.None;
		if (Input.GetKey(KeyCode.Space))
			Cursor.lockState = CursorLockMode.Locked;
		
		gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;

		float yRot = Input.GetAxis("Mouse X") * XSensitivity;
		float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
		
		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

		if(smooth)
		{
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
			macamera.localRotation = Quaternion.Slerp (macamera.localRotation, m_CameraTargetRot,
				smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = m_CharacterTargetRot;
			macamera.localRotation = m_CameraTargetRot;
		}

		if (Input.GetKey (KeyCode.Z)) {
			character.position += macamera.transform.forward * speed;
		}
		if (Input.GetKey (KeyCode.Q)) {
			character.position -= macamera.transform.right * speed;
		}
		if (Input.GetKey (KeyCode.S)) {
			character.position -= macamera.transform.forward * speed;
		}
		if (Input.GetKey (KeyCode.D)) {
			character.position += macamera.transform.right * speed;
		}
	}
}
