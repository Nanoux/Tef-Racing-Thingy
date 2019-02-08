using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour {

	public Racer[] racers;
	Checkpoint[] checkpoints;
	public StartPoint[] startPoints;

	public void StartGame(){
		wheels[] wheelsHi = GameObject.FindObjectsOfType<wheels> ();
		racers = new Racer[wheelsHi.Length];
		for (int i = 0; i < wheelsHi.Length; i++) {
			racers[i] = new Racer(wheelsHi[i].gameObject);
		}
		checkpoints = GameObject.FindObjectsOfType<Checkpoint> ();

		startPoints = GameObject.FindObjectsOfType<StartPoint> ();
		startPoints = startPoints.OrderBy(x => x.priority).ToArray();
		List<GameObject> carsToStart = new List<GameObject> ();
		for (int i = 0; i < racers.Length; i++) {
			carsToStart.Add (racers [i].reference);
		}
		for (int i = 0; i < racers.Length; i++) {
			int index = Random.Range (0, carsToStart.Count - 1);
			carsToStart [index].GetComponentInParent<initplaya> ().TargetTeleport (carsToStart [index].GetComponentInParent<initplaya> ().networkConnection, startPoints [i].transform.position, startPoints [i].transform.rotation);
			carsToStart.RemoveAt (index);
		}
	}
	public void Check(GameObject a, GameObject b){
		if (b.GetComponent<Checkpoint> ().isDirectional) {
			if (Vector3.Dot (b.transform.forward, a.GetComponent<Rigidbody> ().velocity) < 0)
				return;
		}
		for(int i = 0; i < racers.Length; i++){
			if (racers[i].reference.Equals (a)) {
				bool contained = false;
				for (int j = 0; j < racers[i].passedPoints.Count; j++){
					if (b.Equals (racers [i].passedPoints [j]))
						contained = true;
				}
				if (!contained) {
					racers [i].passedPoints.Add (b);
				}
			}
		}
	}
	public void OnTriggerEnter(Collider col){
		for(int i = 0; i < racers.Length; i++){
			if (racers[i].reference.Equals (col.GetComponentInParent<wheels>().gameObject)) {
				if (racers [i].passedPoints.Count == checkpoints.Length) {
					racers [i].passedPoints.Clear ();
					racers [i].laps++;
					Debug.Log (racers [i].laps);
					racers [i].reference.GetComponentInParent<initplaya> ().TargetLapUpdate (racers [i].reference.GetComponentInParent<initplaya> ().networkConnection, racers [i].laps + 1);
				}
			}
		}
	}
	public void Start(){
		if (SceneManager.GetActiveScene ().buildIndex != 3) {
			GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<BoxCollider> ().isTrigger = true;
			StartGame ();
		}
	}

}
public struct Racer{
	public GameObject reference;
	public List<GameObject> passedPoints;
	public int laps;

	public Racer (GameObject reference)
	{
		this.reference = reference;
		passedPoints = new List<GameObject> ();
		laps = 0;
	}
	
}
