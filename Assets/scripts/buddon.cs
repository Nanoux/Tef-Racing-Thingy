using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class buddon : MonoBehaviour {


	public void loadNet(){
		SceneManager.LoadScene (2);
	}
	public void loadSelect(){
		SceneManager.LoadScene (1);
	}
	public void loadMain() {
		SceneManager.LoadScene (0);
	}
	public void loadEditor(){
		SceneManager.LoadScene (3);
	}
	public void car1(){
		NetworkManager.singleton.GetComponent<NetworkCustom>().chosenCharacter = 0;
		loadNet ();
	}
	public void car2(){
		NetworkManager.singleton.GetComponent<NetworkCustom>().chosenCharacter = 1;
		loadNet ();
	}
	public void car3(){
		NetworkManager.singleton.GetComponent<NetworkCustom>().chosenCharacter = 2;
		loadNet ();
	}
	public void map1(){
		NetworkManager.singleton.GetComponent<NetworkCustom> ().onlineScene = "test";
		StartMatch ();
	}
	public void map2(){
		NetworkManager.singleton.GetComponent<NetworkCustom> ().onlineScene = "test2";
		StartMatch ();
	}
	public void StartMatch(){
		NetworkManager.singleton.GetComponent<Host> ().CreateRoom ();
	}
	public void MatchName(string match){
		NetworkManager.singleton.GetComponent<Host> ().SetRoomName (match);
	}
}
