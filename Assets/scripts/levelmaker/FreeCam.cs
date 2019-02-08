using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (1)) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				transform.Translate (Input.GetAxis ("Mouse X") * GetComponentInChildren<Camera> ().transform.localPosition.z / 20f, Input.GetAxis ("Mouse Y") * GetComponentInChildren<Camera> ().transform.localPosition.z / 20f, 0);
			} else {
				transform.Rotate (-Input.GetAxis ("Mouse Y") * 4, Input.GetAxis ("Mouse X") * 4, -transform.eulerAngles.z);
			}
		} else if (Input.GetMouseButton (2)) {
			transform.Translate (Input.GetAxis ("Mouse X") * GetComponentInChildren<Camera> ().transform.localPosition.z / 20f, Input.GetAxis ("Mouse Y") * GetComponentInChildren<Camera> ().transform.localPosition.z / 20f, 0);
		}
		if (Input.GetKey(KeyCode.LeftShift)){
			transform.Translate(0,0,-Input.GetAxis("Mouse ScrollWheel")* GetComponentInChildren<Camera> ().transform.localPosition.z);
		}
		GetComponentInChildren<Camera> ().transform.localPosition = new Vector3 (0f,0f,Mathf.Clamp(GetComponentInChildren<Camera> ().transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * 10,-400f,-1f));
	}
}
