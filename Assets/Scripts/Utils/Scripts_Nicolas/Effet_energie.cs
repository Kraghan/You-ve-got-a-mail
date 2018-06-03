using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effet_energie : MonoBehaviour {

    List<NavigationFollower> m_lines;
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
        m_lines = new List<NavigationFollower>();
        m_parent = new GameObject("Line Parent " + name).transform;
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
			
		for (int i = 0; i < m_lines.Count;)
        {
            NavigationFollower salve = m_lines[i];
            //Si cette salve est arrivée au bout du chemin, je la détruit
            if (salve.GetNextTarget() == endpoint)
                Destroy(salve);
            else
                i++;

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
