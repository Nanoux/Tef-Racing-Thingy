using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class saveButton : MonoBehaviour {

	public string filename;
	
	// Update is called once per frame
	public void loadLevel(){
		GameObject.Find ("Game Controller").GetComponent<GameController> ().Load (filename);
	}
	public void changeName(string name){
		filename = name;
		GetComponentInChildren<Text> ().text = name;
	}
}
