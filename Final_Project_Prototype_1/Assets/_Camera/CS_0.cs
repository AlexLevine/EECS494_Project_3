using UnityEngine;
using System.Collections;

public class CS_0 : Cut_scene {
	int num_transitions=0;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if (!active || transitioning) return;
		
		if (num_transitions==0 && timer>=.2f){
			print ("first transition");
			Camera_follow.adjust_main_camera(cut_scene_object.transform.position,120f,25f,5f,5f,my_callback);
			transitioning = true;
			num_transitions++;
		}
		
		else if (num_transitions==1){ 
			print ("have i printed yet?");
			Switchee sw = cut_scene_object.GetComponent<Switchee>();
			sw.activate();
			num_transitions++;
		}
		
		else if (num_transitions==2){
			print (rotation);
			Camera_follow.adjust_main_camera (Camera_follow.calculate_player_midpoint(),rotation,15f,15f,5f,my_callback);
			transitioning=true;
			num_transitions++;
		}
		else if (num_transitions==3){
			deactivate();
			num_transitions++;
		}
		
	}
}
