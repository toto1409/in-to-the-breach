using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove2 : MonoBehaviour {
    public Transform Endpostion2;
    float speed2;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0.75f, -1.4f, 0);
        speed2 = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, Endpostion2.position, speed2 * Time.deltaTime);
		
	}
}
