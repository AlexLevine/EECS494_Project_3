using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Transform[] children = transform.GetComponentsInChildren<Transform>();
		for (int i=1; i<=transform.childCount; i++){
			children[i].renderer.material.color = Color.magenta;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
