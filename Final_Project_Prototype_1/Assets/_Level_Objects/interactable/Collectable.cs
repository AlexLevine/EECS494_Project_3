using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.GetComponent<CharacterController>()){
			Player_character p = other.GetComponent<Player_character>();
			p.collectable_count++;
			Destroy(gameObject);
		}
	}
}
