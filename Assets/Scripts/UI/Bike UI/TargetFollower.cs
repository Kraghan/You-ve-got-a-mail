using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetFollower : MonoBehaviour
{

    [SerializeField]
    Transform m_target;
    [SerializeField]
    Text m_text;

	Text m_text_Leisure;
	Vector3 Closest_Mailbox;

    Transform m_objectToRotate;

	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = m_target.position - m_objectToRotate.position;
        m_objectToRotate.rotation = Quaternion.LookRotation(direction);

        if(m_text)
            m_text.text = Mathf.Round(Vector3.Distance(m_target.position, m_objectToRotate.position)) + " m";

		if (m_text_Leisure) {
			Closest_Mailbox = Find_Closest ();

			if (Mathf.Round (Vector3.Distance (Closest_Mailbox, m_objectToRotate.position)) <= 50) {
				m_text_Leisure.text = "Less than 50 m";
			}
			else
				m_text_Leisure.text = Mathf.Round (Vector3.Distance (Closest_Mailbox, m_objectToRotate.position)) + " m";
		}
	}

	Vector3 Find_Closest () {

		float mindist = 10000f;
		Vector3 theclosest = Vector3.zero;

		foreach (VacuumMailBox mailbox in this.GetComponent<ScoreManager>().The_Mailboxes) {
			if ((Mathf.Round (Vector3.Distance (mailbox.transform.position, m_objectToRotate.position)) < mindist) && (!mailbox.GetComponent<VacuumMailBox>().IsDelivered())) {
				mindist = Mathf.Round (Vector3.Distance (mailbox.transform.position, m_objectToRotate.position));
				theclosest = mailbox.transform.position;
			}
		}

		return theclosest;
	}

    public void SetTarget(Transform transf)
    {
        m_target = transf;
    }

    public void SetObjectToRotate(Transform transf)
    {
        m_objectToRotate = transf;
    }

    public void SetText(Text text)
    {
        m_text = text;
    }

	public void SetText_Leisure(Text text)
	{
		m_text_Leisure = text;
	}
}
