using UnityEngine;
using System.Collections;

public class Bridge_Switch : MonoBehaviour {
	public Material on;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update (){
	}
	
	void OnTriggerEnter(Collider other){
		Bridge b = GameObject.Find("Bridge").GetComponent<Bridge>();
		b.on = true;
		Renderer r = this.GetComponent<Renderer>();
		r.material = on;
	}
}
