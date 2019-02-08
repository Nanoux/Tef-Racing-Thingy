using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour {

	public Transform car;
	public float cardist,carheight,rotdamp,heightdamp,zoomratio,defaultfov, distovr;
	private Vector3 rotVect;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float wantedangle = car.eulerAngles.y + Input.GetAxis("Look") * 90;
		if (Input.GetAxis ("Flip") > 0f)
			wantedangle += 180f;
		float wantedheight = car.position.y + carheight;
		float camangle = transform.eulerAngles.y;
		float camheight = transform.position.y;
		camangle = Mathf.LerpAngle (camangle,wantedangle,rotdamp * Time.deltaTime);
		camheight = Mathf.Lerp (camheight,wantedheight,heightdamp * Time.deltaTime);
		Quaternion currentrot = Quaternion.Euler (0f, camangle, 0f);
		transform.position = car.position;
		transform.position -= currentrot * Vector3.forward * cardist;
		transform.position = new Vector3 (transform.position.x, camheight, transform.position.z);
		transform.LookAt (car.position + car.transform.up * distovr);

		float speed = car.gameObject.GetComponent<Rigidbody> ().velocity.magnitude;
		GetComponent<Camera> ().fieldOfView = defaultfov + speed * zoomratio * Time.deltaTime;
	}
}
