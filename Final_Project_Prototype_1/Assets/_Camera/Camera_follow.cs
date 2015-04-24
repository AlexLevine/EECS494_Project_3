using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour//, Checkpoint_load_subscriber
{
    public delegate void Camera_callback();

    public Vector3 point_of_interest { get { return point_of_interest_; } }
    public float camera_follow_distance { get { return camera_follow_distance_; } }
    public float camera_hover_height { get { return camera_hover_height_; } }
    public float y_rotation { get { return y_rotation_; } }

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

    private Vector3 velocity = Vector3.zero;
    private bool following_player_ = true;
    private bool is_transitioning_ = false;

    private Vector3 transition_start_position;
    private Quaternion transition_start_rotation;
    private float transition_time_elapsed = 0;
    private float current_transition_duration = 1f;
    private Camera_callback current_transition_callback = null;
    private bool boss_mode = false;
    private float smooth = 0.5f;
    private float y_rotation_speed = 0.0f; // used by SmoothDampAngle

    private static float boss_hover_height = 10f;
    public static float boss_follow_distance = 20f;

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        // Checkpoint.subscribe(this);
        // following_player_ = false;

        // point_of_interest_ = calculate_player_midpoint();

        // transform.position = calculate_target_camera_position();
        // transform.rotation = calculate_target_camera_rotation(
        //     transform.position);

        StartCoroutine(LateFixedUpdate());
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    IEnumerator LateFixedUpdate()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            if (following_player_)
            {
                point_of_interest_ = calculate_player_midpoint();
            }

            var target_position = calculate_target_camera_position();
            var target_rotation = calculate_target_camera_rotation(
                target_position);

            if (!is_transitioning_)
            {
                update_smooth_follow(target_position, target_rotation);
                continue;
            }

            update_transition(target_position, target_rotation);
        }
    }// LateUpdate

    // void LateUpdate()
    // {
    //     if (!is_transitioning_)
    //     {
    //         return;
    //     }

    //     var target_position = calculate_target_camera_position();
    //     var target_rotation = calculate_target_camera_rotation(
    //         target_position);

    //     update_transition(target_position, target_rotation);
    // }

    //--------------------------------------------------------------------------

    // public void notify_checkpoint_load()
    // {
    //     is_transitioning = false;
    // }// notify_checkpoint_load

    //--------------------------------------------------------------------------

    void update_smooth_follow(
        Vector3 target_position, Quaternion target_rotation)
    {
        if (!following_player_)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position, target_position, ref velocity, 0.5f);

        var rotation_step = Mathf.SmoothDampAngle(
            Camera.main.transform.eulerAngles.y,
            target_rotation.eulerAngles.y,
            ref y_rotation_speed,
            smooth);
        var temp = target_rotation.eulerAngles;
        temp.y = rotation_step;
        target_rotation = Quaternion.Euler(temp);

        transform.rotation = target_rotation;
    }// update_smooth_follow

    //--------------------------------------------------------------------------

    void update_transition(Vector3 target_position, Quaternion target_rotation)
    {
        var lerp_percent = (
            current_transition_duration <= 0 ? 1f :
                transition_time_elapsed / current_transition_duration);
        // cubic function for ease in and out
        lerp_percent =
            Mathf.Pow(lerp_percent, 3) * (lerp_percent * (6f * lerp_percent - 15f) + 10f);
        // print(lerp_percent);
        if (lerp_percent >= 1) //HACK
        {
            lerp_percent = 1;
            is_transitioning_ = false;
            if (current_transition_callback != null)
            {
                current_transition_callback();
            }
        }

        transform.rotation = Quaternion.Lerp(
            transition_start_rotation, target_rotation, lerp_percent);
        transform.position = Vector3.Lerp(
            transition_start_position, target_position, lerp_percent);

        transition_time_elapsed += Time.deltaTime;
    }// update_transition

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
        float? y_rotation=null,
        float? camera_follow_distance=null,
        float? camera_hover_height=null,
        float transition_duration=1.5f,
        Camera_callback callback=null)
    {
        // print("adjust_main_camera");
        var camera_follow =
                Camera.main.gameObject.GetComponent<Camera_follow>();

        camera_follow.current_transition_callback = callback;

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
        // print("start_transition");

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

        // if (is_transitioning_)
        // {
        //     print("already transitioning");
        //     return;
        // }

        // StartCoroutine(do_camera_transition(
        //     target_position, target_rotation, transition_duration));

        transition_start_position = transform.position;
        transition_start_rotation = transform.rotation;
        is_transitioning_ = true;
        transition_time_elapsed = 0;
        current_transition_duration = transition_duration;

        if (transition_duration >= 0.1f)
        {
            return;
        }

        transform.position = calculate_target_camera_position();
        transform.rotation = calculate_target_camera_rotation(
            transform.position);
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

    //needs to know if in boss mode
    //new_forward=b.pos-p.pos
    Vector3 calculate_target_camera_position()
    {
        if (!boss_mode)
        {
            var wanted_forward = Quaternion.AngleAxis(y_rotation_, Vector3.up);

            var camera_offset = -(wanted_forward * Vector3.forward);
            camera_offset.y = 0;
            camera_offset = camera_offset.normalized;
            camera_offset *= camera_follow_distance_;

            camera_offset.y += camera_hover_height_;

            return point_of_interest_ + camera_offset;
        }

        return calculate_target_boss_mode_pos();
        // if (!is_transitioning)
        // {
        //     return target_pos;
        // }

        // return Vector3.Lerp(
        //     transform.position, target_pos,
        //     lerp_speed * Time.deltaTime);
    }// calculate_target_camera_position

    Vector3 calculate_target_boss_mode_pos()
    {
        var ninja_pos = Ninja.get().transform.position;
        var samurai_pos = Samurai_Attack.get().transform.position;
        var offset = ninja_pos - samurai_pos;
        offset = offset + boss_follow_distance * offset.normalized;
        offset.y = boss_hover_height;

        return samurai_pos + offset;
    }

    //--------------------------------------------------------------------------

    Quaternion calculate_target_camera_rotation(Vector3 target_position)
    {
        if (!boss_mode){
            var look_direction = point_of_interest_ - target_position;
            var new_forward = Vector3.RotateTowards(
                transform.forward, look_direction, 360f, 0f);

            var target_rotation = Quaternion.LookRotation(new_forward);

            return target_rotation;
        }

        return calculate_target_boss_mode_rot(target_position);
    }

    Quaternion calculate_target_boss_mode_rot(Vector3 target_position)
    {
        var ninja_pos = Ninja.get().transform.position;
        var samurai_pos = Samurai_Attack.get().transform.position;

        var direction = samurai_pos - ninja_pos;

        var desired_forward = Vector3.RotateTowards(
            Camera.main.transform.forward, direction, 360f, 0f);

        var new_rotation = Quaternion.LookRotation(desired_forward).eulerAngles;
        new_rotation.x = 30f;

        return Quaternion.Euler(new_rotation);
    }

    //--------------------------------------------------------------------------

    public static bool point_in_viewport(Vector3 point)
    {
        var viewport_pos = Camera.main.WorldToViewportPoint(point);

        return viewport_pos.y > 0 && viewport_pos.y < 1f &&
               viewport_pos.x > 0f && viewport_pos.x < 1f;

    }// point_in_viewport

    //--------------------------------------------------------------------------

    public void activate_boss_mode()
    {
        boss_mode = true;
        velocity = Vector3.zero;
        y_rotation_speed = 0f;

        // Samurai_Attack.get().notify_players_in_arena();

        // following_player_ = false;

        var target_boss_camera_pos = calculate_target_boss_mode_pos();
        var target_boss_camera_rot = calculate_target_boss_mode_rot(
            target_boss_camera_pos);
        var horizontal_offset =
                target_boss_camera_pos - Samurai_Attack.get().transform.position;
        horizontal_offset.y = 0;

        adjust_main_camera(
            new_point_of_interest: Samurai_Attack.get().transform.position,
            y_rotation: target_boss_camera_rot.eulerAngles.y,
            camera_follow_distance: horizontal_offset.magnitude,
            camera_hover_height: boss_hover_height,
            transition_duration: 1f,
            callback: () => Samurai_Attack.get().notify_players_in_arena());

        //         Vector3? new_point_of_interest=null,
        // float? y_rotation=null,
        // float? camera_follow_distance=null,
        // float? camera_hover_height=null,
        // float transition_duration=1.5f,
        // Camera_callback callback=null)
    }

    //--------------------------------------------------------------------------

    public void deactivate_boss_mode()
    {
        boss_mode = false;
        camera_hover_height_ = 10f;
        camera_follow_distance_ = 10f;
    }
}
