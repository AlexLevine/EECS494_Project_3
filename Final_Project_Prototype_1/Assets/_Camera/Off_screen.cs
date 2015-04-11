using UnityEngine;
using System.Collections;

public class Off_screen : MonoBehaviour {
	private Llama llama;
	private Ninja ninja;
	
	// Use this for initialization
	void Start () {
		llama = Llama.get();
		ninja = Ninja.get();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Camera_follow.point_in_viewport(llama.transform.position) && llama.is_visible){
			print("where did the llama go!");
		}
		if (!Camera_follow.point_in_viewport(ninja.transform.position) && ninja.is_visible){
			print("where did the ninja go!");
		}
	}
}
