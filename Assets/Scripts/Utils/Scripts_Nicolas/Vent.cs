using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour {

    public Vector3 Direction;
    public float Force;
    public int long_affich =10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Le gizmo pour la direction du vent
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Vector3 direction = Direction;
        Color color = Color.red;
        float arrowHeadLength = 0.25f;
        float arrowHeadAngle = 20;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, (direction * long_affich));
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + (direction * long_affich), right * arrowHeadLength * long_affich);
        Gizmos.DrawRay(pos + (direction * long_affich), left * arrowHeadLength * long_affich);
    }

    //Quand un objet rentre dans la zone, le vent y applique une force
    void OnTriggerStay(Collider other)
    {
        //Je normalize ma direction
        Direction = Vector3.Normalize(Direction);

        if (other.attachedRigidbody)
            other.attachedRigidbody.AddForce(Direction * Force);
    }
}
