using UnityEngine;
using System.Collections;

public class Event_cut_scene : Cut_scene
{
	public GameObject target_object;

	public bool manually_set_rotation = false;
	public float target_rotation;
	public float target_hover_height = 5f;
	public float target_follow_distance = 10f;
	public float camera_transition_duration = 1.5f;
	public float post_event_delay = 0.5f;

	public bool destroy_when_finished = true;

    //--------------------------------------------------------------------------

	private float original_rotation;
	private float original_height;
	private float original_distance;
	private Vector3 original_point_of_interest;

	private Cut_scene_callback callback_;

	private bool activated = false;

    //--------------------------------------------------------------------------

    void Start()
    {
        if (!manually_set_rotation)
        {
        	target_rotation = transform.eulerAngles.y;
        }
    }// Start

    //--------------------------------------------------------------------------

	public override void activate(Cut_scene_callback callback=null)
	{
		if (activated)
		{
			return;
		}

		activated = true;

		var camera_follow = Camera.main.GetComponent<Camera_follow>();

		original_point_of_interest = camera_follow.point_of_interest;
		original_rotation = camera_follow.y_rotation;
		original_height = camera_follow.camera_hover_height;
		original_distance = camera_follow.camera_follow_distance;

		callback_ = callback;
		pause_all();

		move_to_target();

	}// activate

    //--------------------------------------------------------------------------

	void move_to_target()
	{
		Camera_follow.adjust_main_camera(
			new_point_of_interest: target_object.transform.position,
			y_rotation: target_rotation,
			camera_follow_distance: target_follow_distance,
			camera_hover_height: target_hover_height,
			transition_duration: camera_transition_duration,
			callback: trigger_event);
	}// move_to_target

    //--------------------------------------------------------------------------

	void trigger_event()
	{
		var sw = target_object.GetComponent<Switchee>();
		// if (sw == null)
		// {
		// 	sw = target_object.GetComponentInChildren<Switchee>();
		// }
		if (sw == null)
		{
			sw = target_object.transform.parent.GetComponent<Switchee>();
		}

		sw.activate(
			callback: return_to_start_position,
			callback_delay: post_event_delay);
	}// trigger_event

    //--------------------------------------------------------------------------

    void return_to_start_position()
    {
		Camera_follow.adjust_main_camera(
			new_point_of_interest: original_point_of_interest,
			y_rotation: original_rotation,
			camera_hover_height: original_height,
			camera_follow_distance: original_distance,
			transition_duration: camera_transition_duration,
			callback: finish_and_clean_up);
    }// return_to_start_position

    //--------------------------------------------------------------------------

    void finish_and_clean_up()
    {
    	unpause_all();
    	if (callback_ != null)
    	{
    		callback_();
    	}

    	if (destroy_when_finished)
    	{
    		Destroy(gameObject);
    		return;
    	}

    	activated = false;
    }// finish_and_clean_up

    //--------------------------------------------------------------------------

}
