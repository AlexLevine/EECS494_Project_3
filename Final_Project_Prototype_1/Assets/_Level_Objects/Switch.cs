using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	public GameObject switchee;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Renderer r = this.GetComponent<Renderer>();
		Switchee sw = switchee.GetComponent<Switchee>();
		if (sw.on) r.material = sw.switch_on_material;
		else r.material = sw.switch_off_material;
	
	}
	
	void OnTriggerEnter(Collider other){
		Switchee sw = switchee.GetComponent<Switchee>();
		sw.on = true;
	}
}
