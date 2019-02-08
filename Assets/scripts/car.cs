using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class car : MonoBehaviour {
	public float maxtorque = 50;
	public float steerforce = 2f;

	public WheelCollider[] WheelCols = new WheelCollider[4];
	public Transform[] Wheelmeshes = new Transform[4];

	bool grounded = false;

	[SerializeField]
	private GameObject menu, speed;
	public GameObject lap;
	public bool isON = false;

	public float steer = 1;

	bool isStarting = false;
	float startTimer = 5f;


	void FixedUpdate(){
		if (!isON ) {
			steer = Input.GetAxis ("Horizontal");
			GetComponentInParent<initplaya> ().steerangle = steer;
			float accelerate = Input.GetAxis ("Vertical");
			float finalangle = steer * 45f;

			//WheelCols [0].steerAngle = finalangle;
			//WheelCols [1].steerAngle = finalangle;


			grounded = false;
			for (int i = 0; i < 4; i++) {
				WheelCols [i].motorTorque = accelerate * maxtorque;
				if (WheelCols [i].isGrounded == true)
					grounded = true;
				if (Input.GetAxis ("HandBrake") > 0 && grounded){
					WheelCols [i].brakeTorque = 100000f;
					GetComponent<Rigidbody> ().AddRelativeTorque (0f,steer * 8000f, 0f);
				}
				else
					WheelCols [i].brakeTorque = 0f;
			}
			if (!grounded) {
				GetComponent<Rigidbody> ().AddRelativeTorque (Input.GetAxis ("SkyTorque") * 6600f, 0f, Input.GetAxis ("Horizontal") * -2200f);
			}
		}
		speed.GetComponent<Text> ().text = "   " + (int)GetComponent<Rigidbody> ().velocity.magnitude + " m/s";
		if (isStarting) {
			startTimer -= Time.fixedDeltaTime;
			for (int i = 0; i < 4; i++) {
				WheelCols [i].brakeTorque = 100000000000f;
			}
			if (startTimer < 0f) {
				isStarting = false;
				startTimer = 3f;
			}
		}
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7)){
			menu.SetActive(!menu.activeSelf);
			isON = !isON;
		}
	}
	public void Leave(){
		MatchInfo match = NetworkManager.singleton.matchInfo;
		NetworkManager.singleton.matchMaker.DropConnection (match.networkId, match.nodeId, 0, NetworkManager.singleton.OnDropConnection);
		NetworkManager.singleton.StopHost ();
		NetworkManager.singleton.StopClient ();
		NetworkManager.singleton.StopMatchMaker ();
	}
	public void StartRace(){
		isStarting = true;
		lap.GetComponent<Text> ().text = "Lap 1";
	}
}