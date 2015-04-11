using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Adjust_camera : MonoBehaviour
{
    public float camera_follow_distance;
    public float camera_hover_height;

    private float camera_y_rotation;

    private bool llama_inside = false;
    private bool ninja_inside = false;

    private bool is_current_camera_angle = false;

    private List<Adjust_camera> adjustment_points = new List<Adjust_camera>();

    void Awake()
    {
        adjustment_points.Add(this);
    }

    void Start()
    {
        camera_y_rotation = transform.rotation.eulerAngles.y;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject == Llama.get().gameObject)
        {
            llama_inside = true;

            if (Llama.get().is_teamed_up)
            {
                ninja_inside = true;
            }
        }

        if (c.gameObject == Ninja.get().gameObject)
        {
            ninja_inside = true;
        }

        if (llama_inside && ninja_inside && !is_current_camera_angle)
        {
            Camera_follow.adjust_main_camera(
                new_point_of_interest: Camera_follow.calculate_player_midpoint(),
                y_rotation: camera_y_rotation,
                camera_follow_distance: camera_follow_distance,
                camera_hover_height: camera_hover_height);

            // set_as_current_camera_angle();
            is_current_camera_angle = true;
        }
    }

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject == Llama.get().gameObject)
        {
            llama_inside = false;
            if (Llama.get().is_teamed_up)
            {
                ninja_inside = false;
            }
        }

        if (c.gameObject == Ninja.get().gameObject)
        {
            ninja_inside = false;
        }

        if (!llama_inside && !ninja_inside)
        {
            is_current_camera_angle = false;
        }
    }

    // public void notify_not_current_camera_angle()
    // {
    //     is_current_camera_angle = false;
    // }

    public void set_as_current_camera_angle()
    {
        foreach (var camera_adj_point in adjustment_points)
        {
            camera_adj_point.is_current_camera_angle = false;
        }

        is_current_camera_angle = true;
    }// set_as_current_camera_angle

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
