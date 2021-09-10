using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour {
    public Transform EndPostion1;
    float speed = 0.3f;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0.86f, 1.52f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, EndPostion1.position, speed * Time.deltaTime);
		
	}
}
