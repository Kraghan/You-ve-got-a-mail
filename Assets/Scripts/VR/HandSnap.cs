using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSnap : MonoBehaviour {

    [SerializeField]
    SteamVR_ControllerManager m_VRControllerManager;
    [SerializeField]
    Camera m_VRCamera;
    [SerializeField]
    float m_farDistance = 3;

    [SerializeField]
    Transform m_leftHandSnap;
    [SerializeField]
    Transform m_rightHandSnap;

    [SerializeField]
    Vector3 m_rotationSnap;

    Vector3 m_storedLeftPosition;
    Vector3 m_storedRightPosition;
    Quaternion m_storedLeftRotation;
    Quaternion m_storedRightRotation;

    bool m_rightSnapped = false;
    bool m_leftSnapped = false;
    bool m_orderSnapLeftHand = false;
    bool m_orderSnapRightHand = false;

    Animator m_leftAnimator;
    Animator m_rightAnimator;

    private void Start()
    {
        m_leftAnimator = m_VRControllerManager.left.GetComponentInChildren<Animator>();
        m_rightAnimator = m_VRControllerManager.right.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        Quaternion rotation = Quaternion.Euler(m_rotationSnap.x + 180, m_rotationSnap.y, m_rotationSnap.z);

        if (!m_leftAnimator.GetBool("Gun") && !m_leftAnimator.GetBool("Pointing")
            && (!m_VRControllerManager.left.activeSelf 
            || Vector3.Distance(m_VRControllerManager.left.transform.position, m_VRCamera.transform.position) > m_farDistance
            || m_orderSnapLeftHand))
        {
            if(!m_leftSnapped)
            {
                Transform handModel = m_leftAnimator.transform.parent;
                m_storedLeftPosition = handModel.localPosition;
                m_storedLeftRotation = handModel.localRotation;
                handModel.gameObject.SetActive(true);
                handModel.parent = m_leftHandSnap;
                handModel.localPosition = Vector3.zero;
                handModel.localRotation = rotation;
                m_leftSnapped = true;

                m_leftAnimator.SetBool("Grab",true);

                m_VRControllerManager.left.GetComponent<VRInteractibleController>().enabled = false;
                MailCanon canon = m_VRControllerManager.left.GetComponent<MailCanon>();
                if (canon)
                    canon.enabled = false;
                MailController natural = m_VRControllerManager.left.GetComponent<MailController>();
                if (natural)
                    natural.enabled = false;

            }
        }
        else if(m_leftSnapped)
        {
            Transform handModel = m_leftHandSnap.GetChild(0);
            handModel.parent = m_VRControllerManager.left.transform;    
            handModel.localPosition = m_storedLeftPosition;
            handModel.localRotation = m_storedLeftRotation;
            m_leftSnapped = false;

            m_leftAnimator.SetBool("Grab", false);

            m_VRControllerManager.left.GetComponent<VRInteractibleController>().enabled = true;
            MailCanon canon = m_VRControllerManager.left.GetComponent<MailCanon>();
            if (canon)
                canon.enabled = true;
            MailController natural = m_VRControllerManager.left.GetComponent<MailController>();
            if (natural)
                natural.enabled = true;
        }

        rotation = Quaternion.Euler(m_rotationSnap);

        if (!m_rightAnimator.GetBool("Gun") && !m_rightAnimator.GetBool("Pointing")
            && (!m_VRControllerManager.right.activeSelf 
            || Vector3.Distance(m_VRControllerManager.right.transform.position, m_VRCamera.transform.position) > m_farDistance
            || m_orderSnapRightHand))
        {
            if(!m_rightSnapped)
            {
                Transform handModel = m_rightAnimator.transform.parent;
                m_storedRightPosition = handModel.localPosition;
                m_storedRightRotation = handModel.localRotation;
                handModel.gameObject.SetActive(true);
                handModel.parent = m_rightHandSnap;
                handModel.localPosition = Vector3.zero;
                handModel.localRotation = rotation;
                m_rightSnapped = true;

                m_rightAnimator.SetBool("Grab", true);

                m_VRControllerManager.right.GetComponent<VRInteractibleController>().enabled = false;
                MailCanon canon = m_VRControllerManager.right.GetComponent<MailCanon>();
                if (canon)
                    canon.enabled = false;
                MailController natural = m_VRControllerManager.right.GetComponent<MailController>();
                if (natural)
                    natural.enabled = false;
            }
            
        }
        else if(m_rightSnapped)
        {
            Transform handModel = m_rightHandSnap.GetChild(0);
            handModel.parent = m_VRControllerManager.right.transform;
            handModel.localPosition = m_storedRightPosition;
            handModel.localRotation = m_storedRightRotation;
            m_rightSnapped = false;

            m_rightAnimator.SetBool("Grab", false);

            m_VRControllerManager.right.GetComponent<VRInteractibleController>().enabled = true;
            MailCanon canon = m_VRControllerManager.right.GetComponent<MailCanon>();
            if (canon)
                canon.enabled = true;
            MailController natural = m_VRControllerManager.right.GetComponent<MailController>();
            if (natural)
                natural.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (other.gameObject.GetInstanceID() == m_VRControllerManager.left.GetInstanceID())
            {
                m_orderSnapLeftHand = true;
            }

            if (other.gameObject.GetInstanceID() == m_VRControllerManager.right.GetInstanceID())
            {
                m_orderSnapRightHand = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if(other.gameObject.GetInstanceID() == m_VRControllerManager.left.GetInstanceID())
            {
                m_orderSnapLeftHand = false;
            }
            else if (other.gameObject.GetInstanceID() == m_VRControllerManager.right.GetInstanceID())
            {
                m_orderSnapRightHand = false;
            }
        }
    }
}
