using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour
{
    // public static float min_camera_distance = 15f;
    // public static float max_camera_distance = 15f;


    // public GameObject players_midpoint_marker;

    public float camera_follow_distance_ = 15f;
    public float camera_hover_height_ = 15f;

    public float y_rotation_ = 0;
    // public static float camera_y_distance = 4f;

    // static public bool in_boss_arena = false;

    private float lerp_speed = 5f;
    private Vector3 player_midpoint;

    private bool is_transitioning = false;

    //--------------------------------------------------------------------------

    public static void adjust_main_camera(
        float? y_rotation=null, float? camera_follow_distance=null,
        float? camera_hover_height=null)
    {
        Camera.main.gameObject.GetComponent<Camera_follow>().adjust(
            y_rotation: y_rotation,
            camera_follow_distance: camera_follow_distance,
            camera_hover_height: camera_hover_height);
    }// adjust_main_camera

    public void adjust(
        float? y_rotation=null, float? camera_follow_distance=null,
        float? camera_hover_height=null)
    {
        if (y_rotation != null)
        {
            y_rotation_ = (float) y_rotation;
            is_transitioning = true;
        }

        if (camera_follow_distance != null)
        {
            camera_follow_distance_ = (float) camera_follow_distance;
            is_transitioning = true;
        }

        if (camera_hover_height != null)
        {
            camera_hover_height_ = (float) camera_hover_height;
            is_transitioning = true;
        }
    }

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        // calculate_player_midpoint();
        // snap_camera_to_position();

        // transform.LookAt(players_midpoint_marker.transform);
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void LateUpdate()
    {
        player_midpoint = calculate_player_midpoint();
        // var desired_rotation = transform.rotation.eulerAngles;
        // desired_rotation.y = y_rotation_;
        // transform.rotation = Quaternion.Euler(desired_rotation);

        var new_pos = calculate_new_position();
        var reached_pos = Vector3.Distance(transform.position, new_pos) < 0.01f;

        var new_rotation = update_rotation();
        var reached_rotation = Mathf.Abs(
            Quaternion.Angle(transform.rotation, new_rotation)) < 0.01f;

        if (is_transitioning && reached_pos && reached_rotation)
        {
            is_transitioning = false;
        }

        transform.position = new_pos;
        transform.rotation = new_rotation;

        // transform.LookAt(players_midpoint_marker.transform);

        // var llama = Llama.get().gameObject;
        // var ninja = Ninja.get().gameObject;

        // var distance = llama.transform.position - ninja.transform.position;

        // var camera_distance = Mathf.Max(new float[] {
        //     Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z)
        // });

        // camera_distance = Mathf.Max(min_camera_distance, camera_distance);
        // camera_distance = Mathf.Min(max_camera_distance, camera_distance);

        // var midpoint = Vector3.Lerp(
        //     llama.transform.position, ninja.transform.position, 0.5f);

        // if (in_boss_arena)
        // {
        //     var new_rotation = transform.rotation.eulerAngles;
        //     new_rotation.x = 90f;
        //     transform.rotation = Quaternion.Euler(new_rotation);
        //     camera_distance = max_camera_distance * 3;
        //     // return;
        // }

        // var new_camera_pos = midpoint;
        // new_camera_pos.z -= camera_distance;
        // new_camera_pos.y += camera_distance;

        // transform.position = Vector3.Lerp(transform.position, new_camera_pos, 1f * Time.deltaTime);
        // transform.position = new_camera_pos;
    }

    Vector3 calculate_player_midpoint()
    {
        var llama = Llama.get().gameObject;
        var ninja = Ninja.get().gameObject;

        var midpoint = Vector3.Lerp(
            llama.transform.position, ninja.transform.position, 0.5f);
        return midpoint;
        // players_midpoint_marker.transform.position = midpoint;
    }// calculate_player_midpoint

    //--------------------------------------------------------------------------

    // void snap_camera_to_position()
    // {
    //     var wanted_forward = Quaternion.AngleAxis(y_rotation_, Vector3.up);
    //     var camera_offset = -(wanted_forward * Vector3.forward);
    //     camera_offset.y = 0;
    //     camera_offset = camera_offset.normalized;
    //     camera_offset *= camera_follow_distance_;

    //     camera_offset.y += camera_hover_height_;

    //     transform.position = player_midpoint + camera_offset;
    // }// snap_camera_to_position

    Vector3 calculate_new_position()
    {
        var wanted_forward = Quaternion.AngleAxis(y_rotation_, Vector3.up);
        var camera_offset = -(wanted_forward * Vector3.forward);
        camera_offset.y = 0;
        camera_offset = camera_offset.normalized;
        camera_offset *= camera_follow_distance_;

        camera_offset.y += camera_hover_height_;

        var target_pos = calculate_player_midpoint() + camera_offset;
        if (!is_transitioning)
        {
            return target_pos;
        }

        return Vector3.Lerp(
            transform.position, target_pos,
            lerp_speed * Time.deltaTime);
    }// calculate_new_position

    //--------------------------------------------------------------------------

    Quaternion update_rotation()
    {
        var look_direction = player_midpoint - transform.position;
        var new_forward = Vector3.RotateTowards(
            transform.forward, look_direction, 360f, 0f);

        var target_rotation = Quaternion.LookRotation(new_forward);
        if (!is_transitioning)
        {
            return transform.rotation;
        }

        return Quaternion.Lerp(
            transform.rotation, target_rotation, lerp_speed * Time.deltaTime);
    }// update_rotation

    //--------------------------------------------------------------------------

    // public bool both_points_in_viewport(Vector3 first, Vector3 second)
    // {
    //     return false;
    // }// both_points_in_viewport

    //--------------------------------------------------------------------------

    public static bool point_step_would_leave_viewport(
        Vector3 point, Vector3 delta_position)
    {
        // if (delta_position.y != 0)
        // {
        //     return false;
        // }

        var desired_position = point + delta_position;
        var viewport_pos = Camera.main.WorldToViewportPoint(desired_position);

        // print("direction: " + direction);
        // print("step: " + step);
        // print("desired_position: " + desired_position);
        // print("viewport_pos: " + viewport_pos);

        var leave_z_far_edge = delta_position.z > 0 && viewport_pos.y > 1f;
        var leave_z_near_edge = delta_position.z < 0 && viewport_pos.y < 0f;
        var leave_x_left_edge = delta_position.x < 0 && viewport_pos.x < 0f;
        var leave_x_right_edge = delta_position.x > 0 && viewport_pos.x > 1f;

        return leave_z_far_edge || leave_z_near_edge ||
               leave_x_left_edge || leave_x_right_edge;


        // if (!point_inside_viewport(viewport_pos))
        // {
        //     return true;
        // }
        // return point.x < 0.9f && point.x > 0.1f &&
        //        point.y < 0.9f && point.y > 0.1f;
    }// point_step_would_leave_viewport

    //--------------------------------------------------------------------------

    public static bool point_in_viewport(Vector3 point)
    {
        var viewport_pos = Camera.main.WorldToViewportPoint(point);

        return viewport_pos.y > 0 && viewport_pos.y < 1f &&
               viewport_pos.x > 0f && viewport_pos.x < 1f;

    }// point_in_viewport
}
