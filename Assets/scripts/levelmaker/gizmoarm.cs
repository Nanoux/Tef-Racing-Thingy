using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmoarm : MonoBehaviour {

	Vector3 camdirection;
	Quaternion temprot;

	public int id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (id < 4 || id > 6) {
			if (GetComponent<Collider> ().enabled == true) {
				temprot = transform.rotation;
				transform.LookAt (GameObject.Find ("Cam").GetComponentInChildren<Camera> ().transform.position);
				camdirection = transform.forward;
				transform.rotation = temprot;
				transform.LookAt (transform.position + transform.forward, camdirection);
			}
		}
	}
}
