using UnityEngine;
using System.Collections;

public class Ice : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Transform[] children = transform.GetComponentsInChildren<Transform>();
		for (int i=1; i<=transform.childCount; i++){
			children[i].renderer.material.color = Color.cyan;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
