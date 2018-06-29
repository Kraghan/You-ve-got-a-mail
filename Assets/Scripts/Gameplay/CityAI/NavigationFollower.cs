using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationFollower : MonoBehaviour {

    static uint s_numberOfObjectCreated = 0; 

    [SerializeField]
    NavigationWaypoint m_startPoint;
    [SerializeField]
    float m_triggerOffset = 0.25f;
    [SerializeField]
    float m_triggerRotationOffset = 0.75f;
    [SerializeField]
    float m_speed = 2;
    [SerializeField]
    float m_distanceBetweenObjects = 1.5f; 

    public NavigationWaypoint m_target;
	public NavigationWaypoint m_nextTarget;

    bool m_rotationStarted = false;
	public bool sensrotation = false;

    float m_angleTarget = 0;
    float m_angleRotated = 0;
    [SerializeField]
    float m_rotationSpeed = 90 * 1.5f;
    [SerializeField]
    Transform m_front;

    bool m_stopped = false;
	protected bool m_stoppoint = false;
    NavigationFollower m_isBlockedBy = null;

    uint m_id;

    Animator m_animator;

    [SerializeField]
    float m_animatorSpeedModifier = 1.25f;

    List<Transform> m_objectInFrontOf = new List<Transform>();

	//Pour le restart
	private void Awake () {
		s_numberOfObjectCreated = 0;
	}

    // Use this for initialization
    protected virtual void Start ()
    {
        // Manage id
        s_numberOfObjectCreated++;
        m_id = s_numberOfObjectCreated;

        // Set the initial position
        transform.position = m_startPoint.transform.position;
        m_target = m_startPoint.GetRandomNeighbour();
        if (m_target == null)
        {
            return;

            string str = m_startPoint.gameObject.name;
            Transform tr = m_startPoint.transform.parent;
            while (tr != null)
            {
                str += tr.gameObject.name + " > " + str;
                tr = tr.parent;
            }

            Debug.LogError("Error in navigation waypoint " + str + " : null reference !");
        }

        m_animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        if (m_target == null)
            return;

		//La distance entre l'objet et sa destination
        float distance = Vector3.Distance(transform.position, m_target.transform.position);

        if (ConditionToChooseNextTarget(distance))
        {
			Stop_Mailbox ();
			
            m_nextTarget = m_target.GetRandomNeighbour();
            if(m_nextTarget == null)
            {
                return;
                string str = m_nextTarget.gameObject.name;
                Transform tr = m_nextTarget.transform.parent;
                while(tr != null)
                {
                    str += tr.gameObject.name + " >> " + str;
                    tr = tr.parent;
                }

                Debug.LogError("Error in navigation waypoint "+str+" : null reference !");
            }
            
			//Si je suis pas à un point d'arrêt
			if (!m_stoppoint) {

				//Pour la boite finale, quand le sens de rotation passe d'un sens des aiguilles d'une montre à l'autre
				if (m_target.ReverseRotation)
					sensrotation = !sensrotation;
				
				//Calcul de l'angle de rotation vers le prochain point
				float angle = (int)Vector3.SignedAngle (transform.forward.normalized, (m_nextTarget.transform.position - m_target.transform.position).normalized, Vector3.up);
				if (sensrotation) {
					angle += 180;
					if (angle >= 180)
						angle -= 360;
				}
				
				//Si l'angle fait moins de 10° je ne tourne pas l'objet, je supprime les angles improbables aussi
				if (Mathf.Abs (angle) >= 178 || Mathf.Abs (angle) <= 2) { //|| angle == 90 || angle == -90 || angle == 180) {
					angle = 0;
				}

				//Si je suis à un point où je veux pas que le joueur tourne, je tourne pas
				if (m_target.NoRotation) {
					angle = 0;
				}

				//Sinon, j'affecte mon angle en l'arrondissant et je dis que j'ai tourné de 0° pour l'instant
				if (angle != 0) {
					m_angleTarget = angle;

					m_angleTarget = Mathf.Round (m_angleTarget);

					m_angleRotated = 0;
				}

				//Ma rotation a commencé
				m_rotationStarted = true;

				//Ce que je fait quand j'atteint mon objectif (rien?)
				DoWhenReachTarget();
			}

        }

		//Si je suis pas à un point d'arrêt
		if (!m_stoppoint) {
			//Je check si j'ai assez tourné, si non, je tourne
			if ((m_angleRotated < m_angleTarget && m_angleTarget > 0) || (m_angleRotated > m_angleTarget && m_angleTarget < 0)) {
				float angle = 0;
				if (m_angleTarget > 0)
					angle = m_rotationSpeed * Time.deltaTime;
				else
					angle = -m_rotationSpeed * Time.deltaTime;

				if (Mathf.Abs (m_angleRotated + angle) > Mathf.Abs (m_angleTarget)) {
					angle -= (m_angleRotated + angle - m_angleTarget);
				}

				transform.Rotate (angle * Vector3.up);
				m_angleRotated += angle;
			}


			if (distance <= m_triggerOffset) {
				m_rotationStarted = false;
				m_target = m_nextTarget;
			}

			if (m_target == null)
				return;
        
			//La direction du mouvement
			Vector3 direction = (m_target.transform.position - transform.position).normalized;

			//Si j'ai pas truc en face de moi je bouge
			if (m_objectInFrontOf.Count == 0) {
				transform.position += direction * m_speed * Time.deltaTime;
				m_stopped = false;
			}
		//Sinon je m'arrête mais sans me bloquer
        else {
				NavigationFollower follower = m_objectInFrontOf [0].gameObject.GetComponent<NavigationFollower> ();
				if (follower == null)
					follower = GetComponentInParent<NavigationFollower> ();

				if (follower != null) {
					// if blocked by myself, don't stop
					if (follower.m_stopped && m_isBlockedBy != null && follower.m_isBlockedBy.m_id == m_id) {
						m_stopped = false;
						m_isBlockedBy = null;
					} else {
						m_stopped = true;
						m_isBlockedBy = follower;
					}
				} else {
					m_stopped = false;
					m_isBlockedBy = null;
				}
 
			}

			//Si je suis pas bloqué, je bouge avec une animation (DU COUP CA BOUGE DEUX FOIS...A CORRIGER)
			if (!m_stopped) {
				transform.position += direction * m_speed * Time.deltaTime;
				if (m_animator != null) {
					if (HasParameter ("Moving", m_animator))
						m_animator.SetBool ("Moving", true);
					m_animator.speed = m_speed * m_animatorSpeedModifier;
				}
			}
		//Sinon je reste immobile
        else {
				if (m_animator != null) {
					if (HasParameter ("Moving", m_animator))
						m_animator.SetBool ("Moving", false);
				}
			}
		}

    }

    protected virtual bool ConditionToChooseNextTarget(float distance)
    {
        return distance <= m_triggerRotationOffset && !m_rotationStarted;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_target == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_target.transform.position, 1);
        Vector3 direction = (m_target.transform.position - transform.position).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_front.transform.position, m_front.transform.position + transform.forward * m_distanceBetweenObjects);
    }

	public void SetSpeed (float speed) {

		m_speed = speed;

	}

	public void SetStartPoint (NavigationWaypoint startpoint) {

		m_startPoint = startpoint;

	}
    
    public NavigationWaypoint GetNextTarget()
    {
        return m_nextTarget;
    }

    protected virtual void DoWhenReachTarget()
    {

    }

	protected virtual void Stop_Mailbox() {


	}

    public bool HasParameter(string parameter, Animator animator)
    {
        foreach(AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameter)
                return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Mover") || other.CompareTag("Player")) && !other.isTrigger)
        {
            m_objectInFrontOf.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Mover") || other.CompareTag("Player")) && !other.isTrigger)
        {
            m_objectInFrontOf.Remove(other.transform);
        }
    }

}
