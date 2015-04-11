using UnityEngine;
using System.Collections;

public class Cut_scene : MonoBehaviour {
	public bool active = false;
	public GameObject cut_scene_object;
	public bool transitioning = false;
	public float timer = 0f;

	public float pre_rotation;
	public float pre_height;
	public float pre_distance;

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
		//print ("cut scene activated");
		Transform cam = GameObject.Find ("Main Camera").transform;
		Vector3 mdpt = Camera_follow.calculate_player_midpoint();
		
		//camera values to return to for return_camera_to_llama_and_ninja()
		pre_rotation = cam.rotation.y;
		pre_height = cam.position.y - mdpt.y;
		pre_distance = Mathf.Sqrt(Mathf.Pow(cam.position.x-mdpt.x,2) + Mathf.Pow(cam.position.z-mdpt.z,2));
	}
	
	public void deactivate()
	{
		active = false;
		Camera_follow.start_following_player();
		Input_reader.toggle_player_controls();
		//print ("cut scene deactivated");
	}
	
	public void return_camera_to_llama_and_ninja()
	{
		var mdpt = Camera_follow.calculate_player_midpoint(); //HACK players sometimes move after transition starts
		Camera_follow.adjust_main_camera (mdpt,pre_rotation,pre_distance,pre_height,5f,my_callback);
		transitioning=true;
	}
	
	public void my_callback(){
		transitioning = false;
		//print("callback worked!");
	}
}
