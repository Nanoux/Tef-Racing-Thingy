using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject levelBut;
	public Transform saveList;

	private static string dataPath = string.Empty;

	public string saveName = "Untitled_Level";

	void Start(){
		
			dataPath = System.IO.Path.Combine (Application.persistentDataPath, saveName + ".json");
			if (SceneManager.GetActiveScene ().buildIndex == 3) {

			foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath)) {
				//Debug.Log (System.IO.Path.GetFileNameWithoutExtension (file.ToString ()));
				GameObject buddon = Instantiate (levelBut) as GameObject;
				buddon.transform.SetParent (saveList);
				buddon.GetComponent<saveButton> ().changeName (System.IO.Path.GetFileNameWithoutExtension (file.ToString ()));
			}

		}
	}
	public static Actor CreateActor(string path, Vector3 position, Quaternion rotation){
		GameObject prefab = Resources.Load<GameObject> (path);
		GameObject go = Instantiate (prefab, position, rotation) as GameObject;

		Actor actor = go.GetComponent<Actor> () ?? go.AddComponent<Actor> ();
		return actor;
	}
	public static Actor CreateActor(ActorData data, string path, Vector3 position, Quaternion rotation){
		if (!path.Equals ("TERRAIN")) {
			Actor actor = CreateActor (path, position, rotation);
			actor.data = data;
			return actor;
		} else {
			GameObject.FindObjectOfType<TerrainGenerator> ().GetComponent<Actor> ().data = data;
			return GameObject.FindObjectOfType<TerrainGenerator>().GetComponent<Actor>();
		}
	}
	public void Save(){
		if (saveName != "" && saveName != null) {
			bool isRepeat = false;
			foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath)) {
				if (System.IO.Path.GetFileNameWithoutExtension (file.ToString ()).Equals (saveName)) {
					isRepeat = true;
					print (file);
				}
			}
			dataPath = System.IO.Path.Combine (Application.persistentDataPath, saveName + ".json");
			SaveData.Save (dataPath, SaveData.actorContainer);
			if (!isRepeat) {
				GameObject buddon = Instantiate (levelBut) as GameObject;
				buddon.transform.SetParent (saveList);
				buddon.GetComponent<saveButton> ().changeName (saveName);
			}
		}


	}
	public void Load(string fileName){
		if (fileName != "" && fileName != null) {
			dataPath = System.IO.Path.Combine (Application.persistentDataPath, fileName + ".json");
			if (SceneManager.GetActiveScene ().buildIndex != 3) {
				initplaya[] players = GameObject.FindObjectsOfType<initplaya> ();
				for (int i = 0; i < players.Length; i++) {
					players [i].RpclowerMenu (SaveData.getJSON (dataPath));
				}
				//initplaya.RpclowerMenu(SaveData.getJSON(dataPath));
			} else {
				newSave();
				SaveData.Load (dataPath);
			}
		}
	}
	public void changeName(string name){
		saveName = name;
	}
	public void newSave(){
		Actor[] actors = GameObject.FindObjectsOfType<Actor> ();
		foreach (Actor hey in actors) {
			if (hey.gameObject.GetComponent<TerrainGenerator>() == null) {
				DestroyImmediate (hey.gameObject);
			}
		}
	}
	public void Buddonize(){
		foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath)) {
			Debug.Log (System.IO.Path.GetFileNameWithoutExtension (file.ToString ()));
			GameObject buddon = Instantiate (levelBut) as GameObject;
			buddon.transform.SetParent (saveList);
			buddon.GetComponent<saveButton> ().changeName (System.IO.Path.GetFileNameWithoutExtension (file.ToString ()));
		}
	}
	public string getPath(){
		return dataPath;
	}


}
