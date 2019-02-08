using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Host : MonoBehaviour {

	uint roomSize = 6;
	string roomName;
	NetworkManager netman;
	public int carindex;

	public void Start(){
		netman = NetworkManager.singleton;
		if (netman.matchMaker == null) {
			netman.StartMatchMaker ();
		}
		//CreateRoom ();
	}
	public void SetRoomName(string enteredname){
		roomName = enteredname;
	}
	public void CreateRoom(){
		if (roomName != "" && roomName != null) {
			netman.matchMaker.CreateMatch (roomName, roomSize, true,"","","",0,0,NetworkManager.singleton.OnMatchCreate);
		}
	}
	public void Cycle(){
		netman.StopMatchMaker ();
		netman.StartMatchMaker ();
	}
}
