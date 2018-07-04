using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BicycleController))]
[RequireComponent(typeof(Collider))]
public class CrashDetection : MonoBehaviour
{
    private Rigidbody m_body;
    private BicycleController m_controller;

    private bool m_crashed = false;
    [SerializeField]
    private SafePylone m_lastRespawnPylone;
    
    [SerializeField]
    ParticleSystem m_particles;

    [SerializeField]
    Camera m_camera;

    int m_oldLayerMask;
    Color m_oldColor;
	public GameObject WarnText;

	public float pause = 2;
	[HideInInspector]
	public float timer;

	public LineRenderer Line_effect;
	private float timerline;

    void Start ()
    {
		m_camera = Camera.main;
        m_controller = GetComponent<BicycleController>();
        m_oldLayerMask = m_camera.cullingMask;
        m_oldColor = m_camera.backgroundColor;
	}
	
	void Update ()
    {
		//Je place correctement les points du line renderer
		Line_effect.SetPosition(0,Line_effect.transform.position);
		Vector3 Line_end = Line_effect.transform.position;

		if (m_lastRespawnPylone.transform.Find ("Line_End") != null)
			Line_end = m_lastRespawnPylone.transform.Find ("Line_End").position;
		
		Line_effect.SetPosition(1,Line_end);

		//De manière assez crado, je désactive la ligne si elle est activée depuis plus de 2 secondes
		if (Line_effect.enabled == true)
			timerline += Time.deltaTime;

		if (timerline >= 2) {
			Line_effect.enabled = false;
			timerline = 0;
		}

        if (m_crashed)
        {
            if(m_controller.GetMotorInput() < 0.1)
            {
				ScoreMailbox.m_scoremultiplier = 1;
                Respawn();
            }
        }

		if (timer < pause)
			timer += Time.deltaTime;
		
    }

    public bool IsCrashed()
    {
        return m_crashed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mover") && m_controller.GetMotorInput() >= 0.1f)
        {
            RagdollTriggerer triggerer = other.gameObject.GetComponentInParent<RagdollTriggerer>();
            if (triggerer != null)
            {
                triggerer.Trigger(m_controller.GetMotorInput() * transform.forward, other.gameObject.GetInstanceID());

                return;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Props"))
            return;

        AkSoundEngine.PostEvent("YGM_Crash", gameObject);

        Vector3 normalWall = Vector3.zero;

        for(uint i = 0; i < collision.contacts.Length; ++i)
        {
            normalWall += collision.contacts[i].normal;
        }
        normalWall.Normalize();
        
        float angle = Mathf.Abs(Vector3.SignedAngle(normalWall, Quaternion.AngleAxis(180,Vector3.up) * transform.forward, Vector3.up));

        if (angle < 45 && m_controller.GetMotorInput() >= 0.1f)
        {
            m_crashed = true;
            BlackScreen();
            AkSoundEngine.PostEvent("YGM_PoleLoading_Start", gameObject);
        }
    }

    public void SetRespawnPylone(SafePylone pylone)
    {
        if (m_lastRespawnPylone != null)
            m_lastRespawnPylone.SetInactive();
        m_lastRespawnPylone = pylone;
        m_lastRespawnPylone.SetActive();

    }

    void BlackScreen()
    {
		WarnText.SetActive (true);
        m_camera.cullingMask = (1 << LayerMask.NameToLayer("WarnText"));
        m_camera.clearFlags = CameraClearFlags.SolidColor;
        m_camera.backgroundColor = Color.black;
    }

    void BackToNormal()
    {
		WarnText.SetActive (false);
        m_camera.cullingMask = m_oldLayerMask;
        m_camera.clearFlags = CameraClearFlags.Skybox;
        m_camera.backgroundColor = m_oldColor;
    }

    public void Respawn()
    {
		//J'active l'effet de line renderer
		Line_effect.enabled = true;

        m_lastRespawnPylone.Respawn();
        m_particles.Play();
        BackToNormal();
        m_crashed = false;
        AkSoundEngine.PostEvent("YGM_PoleLoading_Stop", gameObject);
        
    }
}
