using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove1 : MonoBehaviour {
    public Transform Endpostion2;
    float speed1;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0.76f, -1.4f, 0);
        speed1 = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, Endpostion2.position, speed1 * Time.deltaTime);
	}
}
