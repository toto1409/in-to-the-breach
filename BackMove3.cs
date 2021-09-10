using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove3 : MonoBehaviour {
    public Transform EndPostion3;
    float speed = 2;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(9.82f, -0.89f, 0);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, EndPostion3.position, speed * Time.deltaTime);
    }
}
