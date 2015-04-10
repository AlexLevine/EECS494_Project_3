using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour
{
    public delegate void Camera_callback();

    // public static float min_camera_distance = 15f;
    // public static float max_camera_distance = 15f;

    // public GameObject players_midpoint_marker;
    private Vector3 point_of_interest_;

    private float camera_follow_distance_ = 15f;
    private float camera_hover_height_ = 15f;

    private float y_rotation_ = 0;
    // public static float camera_y_distance = 4f;

    // static public bool in_boss_arena = false;

    // private float lerp_speed = 5;

    private bool following_player_ = true;
    private bool is_transitioning_ = false;

    private float transition_time_elapsed = 0;
    private float current_transition_duration = 1f;
    private Camera_callback current_transition_callback = null;

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        // following_player_ = false;

        // point_of_interest_ = calculate_player_midpoint();

        // transform.position = calculate_target_camera_position();
        // transform.rotation = calculate_target_camera_rotation(
        //     transform.position);
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void LateUpdate()
    {
        if (following_player_)
        {
            point_of_interest_ = calculate_player_midpoint();
        }

        var target_position = calculate_target_camera_position();
        var target_rotation = calculate_target_camera_rotation(
            target_position);

        if (is_transitioning_)
        {
            var lerp_percent =
                    transition_time_elapsed / current_transition_duration;
            lerp_percent =
                Mathf.Pow(lerp_percent, 3) * (lerp_percent * (6f * lerp_percent - 15f) + 10f);
            print(lerp_percent);
            if (lerp_percent >= 1)
            {
                lerp_percent = 1;
                is_transitioning_ = false;
                if (current_transition_callback != null)
                {
                    current_transition_callback();
                }
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation, target_rotation, lerp_percent);
            transform.position = Vector3.Lerp(
                transform.position, target_position, lerp_percent);

            transition_time_elapsed += Time.deltaTime;
            return;
        }

        if (!following_player_)
        {
            return;
        }

        // point_of_interest_ = calculate_player_midpoint();

        // target_position = calculate_target_camera_position();
        // var target_rotation = calculate_target_camera_rotation(
        //     target_position);

        if (Vector3.Distance(transform.position, target_position) > 1f)
        {
            transform.position = Vector3.Lerp(
                transform.position, target_position,
                2f * Time.deltaTime);
        }
    }// LateUpdate

    //--------------------------------------------------------------------------

    public static void start_following_player()
    {
        Camera.main.GetComponent<Camera_follow>().following_player_ = true;
    }// start_following_player

    //--------------------------------------------------------------------------

    public static void stop_following_player()
    {
        Camera.main.GetComponent<Camera_follow>().following_player_ = false;
    }// stop_following_player

    //--------------------------------------------------------------------------

    public static void adjust_main_camera(
        Vector3? new_point_of_interest=null,
        float? y_rotation=null, float? camera_follow_distance=null,
        float? camera_hover_height=null,
        float transition_duration=1f,
        Camera_callback callback=null)
    {
        print("adjust_main_camera");
        var camera_follow =
                Camera.main.gameObject.GetComponent<Camera_follow>();

        camera_follow.start_transition(
            new_point_of_interest: new_point_of_interest,
            y_rotation: y_rotation,
            camera_follow_distance: camera_follow_distance,
            camera_hover_height: camera_hover_height,
            transition_duration: transition_duration);
    }// adjust_main_camera

    void start_transition(
        float transition_duration,
        Vector3? new_point_of_interest=null,
        float? y_rotation=null, float? camera_follow_distance=null,
        float? camera_hover_height=null,
        Camera_callback callback=null)
    {
        print("start_transition");

        if (new_point_of_interest != null)
        {
            point_of_interest_ = (Vector3) new_point_of_interest;
        }

        if (y_rotation != null)
        {
            y_rotation_ = (float) y_rotation;
        }

        if (camera_follow_distance != null)
        {
            camera_follow_distance_ = (float) camera_follow_distance;
        }

        if (camera_hover_height != null)
        {
            camera_hover_height_ = (float) camera_hover_height;
        }

        // var target_position = calculate_target_camera_position();
        // var target_rotation = calculate_target_camera_rotation(
        //     target_position);

        if (is_transitioning_)
        {
            print("already transitioning");
            return;
        }

        // StartCoroutine(do_camera_transition(
        //     target_position, target_rotation, transition_duration));

        is_transitioning_ = true;
        transition_time_elapsed = 0;
        current_transition_duration = transition_duration;

    }// start_transition

    //--------------------------------------------------------------------------

    public static Vector3 calculate_player_midpoint()
    {
        var llama = Llama.get().gameObject;
        var ninja = Ninja.get().gameObject;

        var midpoint = Vector3.Lerp(
            llama.transform.position, ninja.transform.position, 0.5f);
        return midpoint;
        // players_midpoint_marker.transform.position = midpoint;
    }// calculate_player_midpoint

    //--------------------------------------------------------------------------

    Vector3 calculate_target_camera_position()
    {
        var wanted_forward = Quaternion.AngleAxis(y_rotation_, Vector3.up);

        var camera_offset = -(wanted_forward * Vector3.forward);
        camera_offset.y = 0;
        camera_offset = camera_offset.normalized;
        camera_offset *= camera_follow_distance_;

        camera_offset.y += camera_hover_height_;

        return point_of_interest_ + camera_offset;
        // if (!is_transitioning)
        // {
        //     return target_pos;
        // }

        // return Vector3.Lerp(
        //     transform.position, target_pos,
        //     lerp_speed * Time.deltaTime);
    }// calculate_target_camera_position

    //--------------------------------------------------------------------------

    Quaternion calculate_target_camera_rotation(Vector3 target_position)
    {
        var look_direction = point_of_interest_ - target_position;
        var new_forward = Vector3.RotateTowards(
            transform.forward, look_direction, 360f, 0f);

        var target_rotation = Quaternion.LookRotation(new_forward);

        return target_rotation;
    }
    //--------------------------------------------------------------------------

    // // Returns true if any of the parameters passed to this function are not
    // // null.
    // bool update_camera_follow_data(
    //     Vector3? new_point_of_interest=null,
    //     float? y_rotation=null, float? camera_follow_distance=null,
    //     float? camera_hover_height=null)
    // {
    //     if (new_point_of_interest != null)
    //     {
    //         point_of_interest_ = (Vector3) new_point_of_interest;
    //     }

    //     if (y_rotation != null)
    //     {
    //         y_rotation_ = (float) y_rotation;
    //     }

    //     if (camera_follow_distance != null)
    //     {
    //         camera_follow_distance_ = (float) camera_follow_distance;
    //     }

    //     if (camera_hover_height != null)
    //     {
    //         camera_hover_height_ = (float) camera_hover_height;
    //     }

    //     return new_point_of_interest != null || y_rotation != null ||
    //            camera_follow_distance != null || camera_hover_height != null;
    // }// update_camera_follow_data

    //--------------------------------------------------------------------------

    // IEnumerator do_camera_transition(
    //     Vector3 target_position, Quaternion target_rotation,
    //     float transition_duration)
    // {
    //     print("do_camera_transition");

    //     is_transitioning_ = true;

    //     for (float time_elapsed = 0; time_elapsed < transition_duration; time_elapsed += Time.deltaTime)
    //     {
    //         if (following_player_)
    //         {
    //             point_of_interest_ = calculate_player_midpoint();

    //             target_position = calculate_target_camera_position();
    //         }

    //         var lerp_percent = Mathf.Min(1f, time_elapsed / transition_duration);
    //         print(lerp_percent);
    //         transform.rotation = Quaternion.Lerp(
    //             transform.rotation, target_rotation, lerp_percent / 2f);
    //         transform.position = Vector3.Lerp(
    //             transform.position, target_position, lerp_percent);
    //         yield return null;
    //     }

    //     is_transitioning_ = false;

    //     print("transition finished");
    // }// do_camera_transition

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



    //--------------------------------------------------------------------------

    // void update_rotation()
    // {
    //     var look_direction = player_midpoint - transform.position;
    //     var new_forward = Vector3.RotateTowards(
    //         transform.forward, look_direction, 360f, 0f);

    //     var target_rotation = Quaternion.LookRotation(new_forward);
    //     if (!is_transitioning)
    //     {
    //         return transform.rotation;
    //     }

    //     // return Quaternion.Lerp(
    //     //     transform.rotation, target_rotation, lerp_speed * Time.deltaTime);
    // }// update_rotation

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
