using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData {
	public static ActorContainer actorContainer = new ActorContainer();
	public delegate void SerializeAction();
	public static event SerializeAction OnLoaded;
	public static event SerializeAction OnBeforeSave;

	public static void Load(string path){
		actorContainer = LoadActors (path);

		foreach (ActorData data in actorContainer.O) {
			GameController.CreateActor (data, data.n,data.p,data.r);
		}

		OnLoaded ();

		ClearActorList ();
	}
	public static void LoadJSON(string json){
		actorContainer = JsonUtility.FromJson<ActorContainer> (json);
		foreach (ActorData data in actorContainer.O) {
			GameController.CreateActor (data, data.n,data.p,data.r);
		}

		OnLoaded ();

		ClearActorList ();
	}

	public static void Save (string path, ActorContainer actors){
		OnBeforeSave ();

		SaveActors (path, actors);

		ClearActorList ();
	}

	public static void AddActorData(ActorData data){
		actorContainer.O.Add (data);
	}
	public static void ClearActorList(){
		actorContainer.O.Clear();
	}

	private static ActorContainer LoadActors(string path){
		string json = File.ReadAllText (path);

		return JsonUtility.FromJson<ActorContainer> (json);
	}

	private static void SaveActors(string path, ActorContainer actors){
		string json = JsonUtility.ToJson (actors);

		StreamWriter sw = File.CreateText (path);
		sw.Close ();

		File.WriteAllText (path, json);
	}
	public static string getJSON(string path){
		return File.ReadAllText (path);
	}
}
