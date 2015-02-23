using UnityEngine;
using System.Collections;

public class Pole : MonoBehaviour {
	private bool llama;
	private bool ninja;

	// Use this for initialization
	void Start () {
		llama = false;
		ninja = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (llama && ninja){
			renderer.material.color = Color.black;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name=="Ninja"){
			ninja = true;
		}
		if (other.name=="Llama"){
			llama = true;
		}
	}
}
