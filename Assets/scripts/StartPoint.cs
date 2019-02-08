using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour {

	public int priority;
	void Start(){
		priority = (int)(GetComponent<MeshRenderer> ().material.color.r * 256);
		if (SceneManager.GetActiveScene ().buildIndex != 3) {
			GetComponent<SphereCollider> ().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
