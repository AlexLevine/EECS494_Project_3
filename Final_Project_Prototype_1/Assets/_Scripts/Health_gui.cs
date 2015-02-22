using UnityEngine;
using System.Collections;

public class Health_gui : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Player_script playerObj = GameObject.Find ("Player").GetComponent<Player_script> ();
		GUIText gt = this.GetComponent<GUIText> ();

//		GUIStyle style = new GUIStyle ();
//		style.richText = true;
//		GUILayout.Label("<size=30>Some <color=yellow>RICH</color> text</size>",style);

		gt.text = "Health: " + playerObj.health + "\nHydration: " + playerObj.hydration +
			"\nHunger: " + playerObj.hunger;
		
		
		//		PE_Engine pe = GameObject.Find ("Main Camera").GetComponent<PE_Engine> ();
		//		Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		//		Vector3 cam = pe.transform.position;
		//		Vector3 offset = new Vector3 (-1.3f, 6.8f, 0f);
		//		this.transform.position = camera.WorldToViewportPoint(cam + offset);
	}
}
