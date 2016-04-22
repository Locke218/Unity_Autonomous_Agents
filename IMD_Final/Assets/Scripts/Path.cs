using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {

    public Vector3 startP;
    public Vector3 endP;
    public Vector3 B;

	// Use this for initialization
	void Start () {

        Vector3 midPoint = startP + endP;
        midPoint /= 2;

        transform.position = midPoint;
        
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(startP, endP, Color.red);
        B = endP - startP;
	}

}
