using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public Vector3[] checkpoints;
	public GameObject ll;
	public GameObject nin;
	public Vector3 current_ckpt;
	public int current_index;
	
	// Use this for initialization
	void Start () {
		current_ckpt = checkpoints[0];
		current_index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate(){
		if (ll.transform.position.z>= checkpoints[current_index+1].z && 
		    nin.transform.position.z>= checkpoints[current_index+1].z){
			current_index++;
			current_ckpt = checkpoints[current_index];
		}
		//print(current_ckpt);
		
		//if fallen off stage
		if (ll.transform.position.y<=-20 || nin.transform.position.y<=-20){
			ll.transform.position = current_ckpt + new Vector3(2,0,0);
			nin.transform.position = current_ckpt - new Vector3(2,0,0);
		}
		
	}
}
