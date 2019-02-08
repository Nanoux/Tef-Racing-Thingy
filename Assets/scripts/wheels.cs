using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class wheels : MonoBehaviour {

	public Transform COM;

	public float syncsteer = 1;
	Vector3 lastpos = Vector3.zero;
	float rotamount;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<car> ().enabled) {
			for (int i = 0; i < 4; i++) {
				Quaternion rot;
				Vector3 pos;
				gameObject.GetComponent<car> ().WheelCols [i].GetWorldPose (out pos, out rot);
				gameObject.GetComponent<car> ().Wheelmeshes [i].position = pos;
				gameObject.GetComponent<car> ().Wheelmeshes [i].rotation = rot;
			}
		} else {
			rotamount = ((lastpos - transform.position)/Time.deltaTime).magnitude / GetComponent<car> ().WheelCols [0].radius * 180f / Mathf.PI * Time.deltaTime;
			//Debug.Log (Vector3.Dot(transform.forward,(lastpos - transform.position)/Time.deltaTime));
			if (Vector3.Dot(transform.forward,(lastpos - transform.position)/Time.deltaTime) > 0)
				rotamount *= -1;
			for (int i = 0; i < 4; i++) {
				Quaternion rot;
				Vector3 pos;
				gameObject.GetComponent<car> ().WheelCols [i].GetWorldPose (out pos, out rot);
				gameObject.GetComponent<car> ().Wheelmeshes [i].position = pos;
				gameObject.GetComponent<car> ().Wheelmeshes [i].GetComponentInChildren<MeshRenderer>().transform.Rotate(new Vector3(rotamount,0f,0f),Space.Self);
			}
			/*float wheel1, wheel2;
			if (gameObject.GetComponent<car> ().Wheelmeshes [0].transform.localEulerAngles.y > 180)
				wheel1 = gameObject.GetComponent<car> ().Wheelmeshes [0].transform.localEulerAngles.y - 360;
			else
				wheel1 = gameObject.GetComponent<car> ().Wheelmeshes [0].transform.localEulerAngles.y;
			if (gameObject.GetComponent<car> ().Wheelmeshes [1].transform.localEulerAngles.y > 180)
				wheel2 = gameObject.GetComponent<car> ().Wheelmeshes [1].transform.localEulerAngles.y - 360;
			else
				wheel2 = gameObject.GetComponent<car> ().Wheelmeshes [1].transform.localEulerAngles.y;
			gameObject.GetComponent<car> ().Wheelmeshes [0].Rotate(new Vector3(0f,(syncsteer * 45f) - wheel1,0f),Space.Self);
			gameObject.GetComponent<car> ().Wheelmeshes [1].Rotate(new Vector3(0f,(syncsteer * 45f) - wheel2,0f),Space.Self);*/
			gameObject.GetComponent<car> ().Wheelmeshes [0].transform.localEulerAngles = new Vector3 (0f, syncsteer * 45f, 180f);
			gameObject.GetComponent<car> ().Wheelmeshes [1].transform.localEulerAngles = new Vector3 (0f, syncsteer * 45f, 180f);
			gameObject.GetComponent<car> ().Wheelmeshes [2].transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
			gameObject.GetComponent<car> ().Wheelmeshes [3].transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
			lastpos = transform.position;
		}
	}
	void FixedUpdate(){
		float finalangle = syncsteer * 45f;
		GetComponent<Rigidbody> ().centerOfMass = COM.localPosition;
		GetComponent<car>().WheelCols [0].steerAngle = finalangle;
		GetComponent<car>().WheelCols [1].steerAngle = finalangle;

	}
}
