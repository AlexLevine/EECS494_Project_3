using UnityEngine;
using System.Collections;

public class CS_Wall : Cut_scene {
	int state=0;
	public GameObject wall;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if (!active || transitioning) return;
		
		if (state==0 && timer>=.2f){
			//print ("first transition");
			Camera_follow.adjust_main_camera(wall.transform.position,120f,25f,5f,5f,my_callback);
			transitioning = true;
			state++;
		}
		
		else if (state==1){ 
			//print ("have i printed yet?");
			Switchee sw = wall.GetComponent<Switchee>();
			sw.activate();
			state++;
		}
		
		else if (state==2){
			return_camera_to_llama_and_ninja();
			state++;
		}
		else if (state==3){
			deactivate();
			state++;
		}
		
	}
}
