using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ModeTowardWaypoint()
    {
        var a = transform.InverseTransformPoint(new Vector3(1, 0, 1));
    }
}
