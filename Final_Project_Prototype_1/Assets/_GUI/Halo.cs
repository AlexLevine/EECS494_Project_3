using UnityEngine;
using System.Collections;

public class Halo : MonoBehaviour {
	public float dist_to_ground;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var sphere = transform.parent.Find ("Sphere");
		var x = sphere.position.x;
		var y = sphere.position.y;
		var z = sphere.position.z;
		transform.position = new Vector3(x,y-dist_to_ground,z);
		//print(y);
	}
}
