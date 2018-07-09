using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode_selector : MonoBehaviour {

	public enum MyPlayMode
	{
		NULL,
		POINTS,
		LEISURE,
		STORY
	}

	public static MyPlayMode m_defaultPlayMode = MyPlayMode.NULL;
	public Text Text_points, Text_story, Text_leisure;
	public RectTransform Aiguille_Points, Aiguille_Leisure, Aiguille_Story;
	public KeybordController m_Controller_Mouse;
	public PlayerController m_Controller_VR;
	private SpeedMeter LeCompteur;

	public GameObject Coordinator;
	private MailboxCoordinator Mail_Coordinator;
	private ScoreManager TheScoreManager;

	private bool doonce;

	[HideInInspector]
	public Transform entities;

	// Use this for initialization
	void Start () {

		m_defaultPlayMode = MyPlayMode.NULL;
		LeCompteur = this.GetComponent<SpeedMeter> ();
		Mail_Coordinator = Coordinator.GetComponent<MailboxCoordinator> ();
		TheScoreManager = Coordinator.GetComponent<ScoreManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (m_defaultPlayMode == MyPlayMode.POINTS) {
			TheScoreManager.m_timeText = Text_points;
			LeCompteur.m_transform = Aiguille_Points;
			if (!doonce) {
				doonce = true;
				m_Controller_VR.SetPanne (false);
				m_Controller_Mouse.SetPanne (false);
			}
		} else if (m_defaultPlayMode == MyPlayMode.LEISURE) {
			
			TheScoreManager.m_timeText = Text_leisure;
			LeCompteur.m_transform = Aiguille_Leisure;

			if (!doonce) {
				
				//Je désactive une partie des piétons et voitures
				Transform[] all_movers = entities.GetComponentsInChildren<Transform>();
				foreach (Transform mover in all_movers) {
					
					//Si c'est bien un robot et pas une partie d'un robot
					if (mover.gameObject.GetComponent<Can_deactivate> () != null) {
						if (Random.Range (0, 5) < 4)
							//Je destroy pas parce que sinon ça bug avec les listes déjà crées avec les robots dedans
							mover.gameObject.SetActive (false);
					}						
				}

				//Je charge les boites aux lettres déjà livrées dans la sauvegarde
				GetComponent<Load>().Load_Leisure_Mailboxes();

				doonce = true;
				m_Controller_VR.SetPanne (false);
				m_Controller_Mouse.SetPanne (false);

			}
		} else if (m_defaultPlayMode == MyPlayMode.STORY) {
			TheScoreManager.m_timeText = Text_story;
			LeCompteur.m_transform = Aiguille_Story;
			if (!doonce) {
				doonce = true;
				m_Controller_VR.SetPanne (false);
				m_Controller_Mouse.SetPanne (false);
			}
		} else if (m_defaultPlayMode == MyPlayMode.NULL) {
			m_Controller_VR.SetPanne (true);
			m_Controller_Mouse.SetPanne (true);
		}
	}

	public void SetMode (string lemode) {
		if (lemode == "POINTS")
			m_defaultPlayMode = MyPlayMode.POINTS;
		if (lemode == "LEISURE")
			m_defaultPlayMode = MyPlayMode.LEISURE;
		if (lemode == "STORY")
			m_defaultPlayMode = MyPlayMode.STORY;
	}

	public void SetFirstObjectiveMailbox () {
		
		Mail_Coordinator.m_aVacuumMailboxes[0].SetAsCurrentTarget();
	}
}
