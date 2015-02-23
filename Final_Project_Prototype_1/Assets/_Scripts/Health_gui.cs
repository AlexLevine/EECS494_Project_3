using UnityEngine;
using System.Collections;

public class Health_gui : MonoBehaviour {

	Ninja ninja;
	Llama llama;
	
	// Use this for initialization
	void Start () {
		ninja = GameObject.Find ("Ninja").GetComponent<Ninja> ();
		llama = GameObject.Find ("Llama").GetComponent<Llama> ();
	}
	
	// Update is called once per frame
	void Update () {
		GUIText gt = this.GetComponent<GUIText> ();

		gt.text = "Ninja Health: " + ninja.health + "\nLlama Health: " + llama.health;

	}
}
