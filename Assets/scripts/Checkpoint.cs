using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {

	public bool isDirectional;

	public void OnTriggerEnter(Collider col){
		GameObject.FindObjectOfType<FinishLine> ().Check (col.GetComponentInParent<wheels>().gameObject, this.gameObject);
	}
	public void Start(){
		if (SceneManager.GetActiveScene ().buildIndex != 3) {
			GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<BoxCollider> ().isTrigger = true;
			if (transform.childCount > 0)
				transform.GetChild (0).GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
