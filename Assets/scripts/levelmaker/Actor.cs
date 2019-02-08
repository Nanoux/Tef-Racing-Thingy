using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour {

	public ActorData data = new ActorData();
	public string prefab = "prefabs/Plane";

	public void StoreData(){
		data.n = prefab;
		if (GetComponent<TerrainGenerator> () != null) {
			data.p = Vector3.zero;
			data.s = Vector3.zero;
			data.c = Color.white;
			data.r.w = GetComponent<TerrainGenerator> ().heightMapSettings.noiseSettings.seed;
			data.r.x = GetComponent<TerrainGenerator> ().heightMapSettings.noiseSettings.offset.x;
			data.r.y = GetComponent<TerrainGenerator> ().heightMapSettings.noiseSettings.offset.y;
			data.r.z = GetComponent<TerrainGenerator> ().currentPreset;
		} else if (GetComponent<BezierBit> () != null) {
			data.p = transform.position;
			data.r = transform.rotation;
			data.s = new Vector3 (GetComponent<BezierBit> ().ID, GetComponent<BezierBit> ().child, transform.localScale.z);
			data.c = Color.white;
		}else{
			data.p = transform.position;
			data.r = transform.rotation;
			data.s = transform.localScale;
			data.c = GetComponent<MeshRenderer> ().material.color;
		}
	}
	public void LoadData(){
		prefab = data.n;
		if (GetComponent<TerrainGenerator> () != null) {
			GetComponent<TerrainGenerator> ().heightMapSettings.noiseSettings.seed = (int)data.r.w;
			GetComponent<TerrainGenerator> ().heightMapSettings.noiseSettings.offset = new Vector2 (data.r.x, data.r.y);
			GetComponent<TerrainGenerator> ().changePreset (Mathf.RoundToInt (data.r.z));
			GetComponent<TerrainGenerator> ().ReSpawn ();
		} else if (GetComponent<BezierBit> () != null) {
			transform.position = data.p;
			transform.rotation = data.r;
			transform.localScale = Vector3.one * data.s.z;
			GetComponent<BezierBit> ().ID = (int)data.s.x;
			GetComponent<BezierBit> ().child = (int)data.s.y;
		}else{
			transform.position = data.p;
			transform.rotation = data.r;
			transform.localScale = data.s;
			GetComponent<MeshRenderer> ().material.color = data.c;
		}
	}

	public void ApplyData(){
		SaveData.AddActorData (data);
	}

	void OnEnable(){
		SaveData.OnLoaded += LoadData;
		SaveData.OnBeforeSave += StoreData;
		SaveData.OnBeforeSave += ApplyData;
	}
	void OnDisable(){
		SaveData.OnLoaded -= LoadData;
		SaveData.OnBeforeSave -= StoreData;
		SaveData.OnBeforeSave -= ApplyData;
	}
}

[Serializable]
public class ActorData{
	public string n;
	public Quaternion r;
	public Vector3 p;
	public Vector3 s;
	public Color c;
}
