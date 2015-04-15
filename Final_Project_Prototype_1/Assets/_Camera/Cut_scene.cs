using UnityEngine;
using System.Collections;

public class Cut_scene : MonoBehaviour
{
	public bool active = false;
	public bool transitioning = false; //must be true when adjust_main_camera is running
	public float timer = 0f;

	private float pre_rotation;
	private float pre_height;
	private float pre_distance;
	private Vector3 pre_mdpt;

	public virtual void Update ()
	{
		timer+=Time.deltaTime;
	}

    //--------------------------------------------------------------------------

	//toggles active boolean, turns on player control, remembers current camera configuration
	public void activate()
	{
		active = true;
		timer = 0f;

		//pause
		Camera_follow.stop_following_player();
		Input_reader.toggle_player_controls();
		Actor.actors_paused = true;

		//pre cut scene camera values
		Transform cam = Camera.main.transform;
		pre_mdpt = Camera_follow.calculate_player_midpoint();
		pre_rotation = cam.rotation.y;
		pre_height = cam.position.y - pre_mdpt.y;
		pre_distance = Mathf.Sqrt(Mathf.Pow(cam.position.x-pre_mdpt.x,2) + Mathf.Pow(cam.position.z-pre_mdpt.z,2));

	}

    //--------------------------------------------------------------------------

	//toggles active boolean, turns on player control
	public void deactivate()
	{
		active = false;
		Camera_follow.start_following_player();
		Input_reader.toggle_player_controls();
		Actor.actors_paused = false;
		//print ("cut scene deactivated");
	}

    //--------------------------------------------------------------------------

	//returns camera to pre cut_scene view
	public void return_camera_to_llama_and_ninja()
	{
		Camera_follow.adjust_main_camera (pre_mdpt,pre_rotation,pre_distance,pre_height,5f,my_callback);
		transitioning=true;
	}

    //--------------------------------------------------------------------------

	public void my_callback()
	{
		transitioning = false;
		//print("callback worked!");
	}
}
