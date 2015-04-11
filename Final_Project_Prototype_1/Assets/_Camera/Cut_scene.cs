using UnityEngine;
using System.Collections;

public class Cut_scene : MonoBehaviour {
	public bool active = false;
	public GameObject cut_scene_object;
	public bool transitioning = false;
	public float timer = 0f;

	public float rotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
		timer+=Time.deltaTime;
	}
			
	public void activate()
	{
		active = true;
		timer = 0f;
		Camera_follow.stop_following_player();
		Input_reader.toggle_player_controls();
		print ("cut scene activated");
		rotation = GameObject.Find ("Main Camera").transform.rotation.y;
	}
	
	public void deactivate()
	{
		active = false;
		Camera_follow.start_following_player();
		Input_reader.toggle_player_controls();
		print ("cut scene deactivated");
	}
	
	public void my_callback(){
		transitioning = false;
		print("callback worked!");
	}
}
