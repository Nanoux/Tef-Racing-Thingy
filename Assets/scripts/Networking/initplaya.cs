using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initplaya : NetworkBehaviour {

	Camera hey;
	GameObject me;
	public GameObject clientmenu, hostmenu,savelisticle;
	public NetworkConnection networkConnection;
	string JSON;

	[SyncVar]
	public float steerangle;

	// Use this for initialization
	void Start () {
		if (NetworkServer.connections != null && NetworkServer.connections.Count > 0) {
			networkConnection = NetworkServer.connections [NetworkServer.connections.Count - 1];
			Debug.Log (networkConnection);
		}

		if (!isLocalPlayer) {

			//GetComponentInChildren<car>().enabled = false;
			//GetComponentInChildren<Camera> ().enabled = false;
			//GetComponentInChildren<AudioListener> ().enabled = false;
			//GetComponentInChildren<Canvas> ().gameObject.SetActive (false);

		} else {
			if (SceneManager.GetActiveScene ().buildIndex == 5) {
				GameObject.FindObjectOfType<TerrainGenerator> ().viewer = GetComponentInChildren<car> ().transform;
			}
			hey = Camera.main;
			if (hey != null) {
				hey.gameObject.SetActive (false);
			}

			if (SceneManager.GetActiveScene ().buildIndex == 5) {
				GetComponentInChildren<car> ().isON = true;
				if (isServer) {
					hostmenu.SetActive (true);
					GameObject.Find ("Game Controller").GetComponent<GameController> ().saveList = savelisticle.transform;
					GameObject.Find ("Game Controller").GetComponent<GameController> ().Buddonize ();
				} else {
					Debug.Log (NetworkManager.singleton.GetComponent<NetworkCustom> ().test);
					clientmenu.SetActive (true);
					GetComponentInChildren<car> ().isON = false;
					CmdgetJSON ();
				}
			} else {
				if (!isServer) {
					GameObject.FindObjectOfType<FinishLine> ().gameObject.SetActive (false);
					Checkpoint[] points = GameObject.FindObjectsOfType<Checkpoint> ();
					for (int i = 0; i < points.Length; i++) {
						points [i].gameObject.SetActive (false);
					}
				}
			}
		}
	}

	void OnDisable(){
		if (hey != null) {
			hey.gameObject.SetActive (true);
		}
	}
	void Update(){
		GetComponentInChildren<wheels> ().syncsteer = steerangle;
		if (hasAuthority)
		CmdWheelSync (GetComponentInChildren<car> ().steer);
	}
	[Command]
	void CmdWheelSync(float a){
		steerangle = a;
	}
	[ClientRpc]
	public void RpclowerMenu(string json){
		hostmenu.SetActive(false);
		clientmenu.SetActive (false);
		GetComponentInChildren<car> ().isON = false;
		GameObject.Find("Game Controller").GetComponent<GameController>().newSave();
		SaveData.LoadJSON (json);
	}
	public void reglowerMenu(string json){
		hostmenu.SetActive(false);
		clientmenu.SetActive (false);
		GetComponentInChildren<car> ().isON = false;
		GameObject.Find("Game Controller").GetComponent<GameController>().newSave();
		SaveData.LoadJSON (json);
	}
	[Command]
	public void CmdgetJSON(){
		if (hostmenu.activeSelf == false)
			TargetgetJSON (NetworkServer.connections[NetworkServer.connections.Count - 1], SaveData.getJSON (GameObject.Find("Game Controller").GetComponent<GameController>().getPath()));
	}
	[TargetRpc]
	public void TargetgetJSON(NetworkConnection target, string json){
		SaveData.LoadJSON (json);
		clientmenu.SetActive (false);
	}
	[TargetRpc]
	public void TargetTeleport(NetworkConnection target, Vector3 position, Quaternion rotation){
		GetComponentInChildren<car>().transform.position = position;
		GetComponentInChildren<car>().transform.rotation = rotation;
		GetComponentInChildren<car> ().StartRace ();
		GetComponentInChildren<Rigidbody> ().velocity = Vector3.zero;
	}
	[TargetRpc]
	public void TargetLapUpdate(NetworkConnection target, int laps){
		GetComponentInChildren<car> ().lap.GetComponent<Text> ().text = "Lap " + laps;
	}
}