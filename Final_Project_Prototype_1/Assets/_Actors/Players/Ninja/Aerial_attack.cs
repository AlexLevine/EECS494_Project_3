using UnityEngine;
using System.Collections;

public class Aerial_attack : MonoBehaviour {
	private float step=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Ninja>().is_aerial){
			var v = Vector3.zero;
			if (step<.3f){
				step+=Time.deltaTime;
			}
			else{
				v.y = -100f;
			}
			Ninja.get().apply_momentum(v);
		}
		else{
			step=0;
		}
	}	
}
