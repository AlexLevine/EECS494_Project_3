using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {
	public Vector3 pit_refresh_point;
	public int push_back = 10;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name.Contains ("Llama")){
			var temp = other.transform.parent.transform.position;
			//temp.y = pit_refresh_point.y;
			temp.z = temp.z - push_back;
			other.transform.parent.transform.position = temp;
		}
		else{
			var temp = other.transform.position;
			//temp.y = pit_refresh_point.y;
			temp.z = temp.z - push_back;
			other.transform.position = temp;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
