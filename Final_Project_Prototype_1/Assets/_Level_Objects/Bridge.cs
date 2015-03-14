using UnityEngine;
using System.Collections;

public class Bridge : MonoBehaviour {
	public bool on;
	private Vector3 rotate;
	
	// Use this for initialization
	void Start () {
		on = false;
		rotate = transform.position-new Vector3(0,transform.localScale.y/2f,0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float temp = Quaternion.Angle (transform.rotation,Quaternion.Euler(0,0,0));
		if (temp>=0 && temp<=90){
			transform.RotateAround(rotate,new Vector3(1,0,0),20*Time.fixedDeltaTime);
		}
	}
}
