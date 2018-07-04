using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effet_energie : MonoBehaviour {

	public Transform line_effect;
	public float frequency;
	public float speed;

	public NavigationWaypoint startpoint;
	public NavigationWaypoint endpoint;

	private float temps;
	private Transform lasalve;

    Transform m_parent;

    bool m_disabled = false;

	// Use this for initialization
	void Start () {
        m_parent = new GameObject("Line Parent " + name).transform;
        m_parent.parent = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
        if (!m_disabled)
        {
            //Je calcule le temps qui passe d'ici à la prochaine salve
            if (temps <= frequency)
            {

                temps += Time.deltaTime;

                //Si assez de temps est passé, je génère une salve
            }
            else
            {

                lasalve = Instantiate(line_effect, startpoint.transform.position, Quaternion.identity);
                lasalve.parent = m_parent;
                lasalve.GetComponent<NavigationFollower>().SetStartPoint(startpoint);
                lasalve.GetComponent<NavigationFollower>().SetSpeed(speed);
                temps = 0;

            }
        }
			
		foreach (NavigationFollower salve in m_parent.GetComponentsInChildren<NavigationFollower>())
        {
			
            //Si cette salve est arrivée au bout du chemin, je la détruit
			if (salve.GetNextTarget () == endpoint)
				salve.enabled = false;

		}		
	}

    public void Disable()
    {
        m_disabled = true;
    }

    public void Enable()
    {
        m_disabled = false;
    }
}
