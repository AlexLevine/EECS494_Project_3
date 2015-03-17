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
		if (ll.transform.position.z >= current_ckpt.z) current_ckpt = checkpoints[current_index];
	}
}
