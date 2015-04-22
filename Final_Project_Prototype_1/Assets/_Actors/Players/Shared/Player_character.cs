using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using InControl;

[RequireComponent(typeof(Rigidbody)),
 RequireComponent(typeof(Input_reader))]
public class Player_character : Actor
{
    public static List<GameObject> player_characters = new List<GameObject>();
    public static bool force_team_up = false;

    public static bool controls_enabled = true;

    public InputDevice input_device;

    public GameObject lock_on_bar = null;

    public override float max_health { get { return 10; } }
    // Character state information.
    public bool is_locked_on { get { return lock_on_target != null; } }
    public Vector3 lock_on_target_pos {
        get { return lock_on_target.transform.position; } }
    public bool is_teamed_up { get { return teamed_up; } }
    public bool is_jumping { get { return jumping; } }
    public virtual bool can_jump {
        get { return is_grounded || time_in_air <= max_time_in_air; } }

    // Movement physics
    public float run_speed { get { return 10f; } }
    // public float run_acceleration { get { return 30f; } }
    public override float gravity { get { return -40f; } }
    // Note that if max_jump_ascend_time is too small, the character will jump
    // higher than desired.
    public virtual float max_jump_height { get { return 4f; } }
    public virtual float max_jump_ascend_time { get { return 0.3f; } }
    public float jump_speed
    {
        get
        {
            // x = vt + 1/2 at^2, solved for initial velocity.
            return (max_jump_height -
                    0.5f * gravity * Mathf.Pow(max_jump_ascend_time, 2)) /
                    max_jump_ascend_time;
        }
    }

    protected override float invincibility_flash_duration {
        get {return 1f; } }

    //--------------------------------------------------------------------------

    private bool jumping = false;
    private float time_in_air = 0;
    private float max_time_in_air { get { return 0.1f; } }
    private bool teamed_up = false;
    private GameObject lock_on_target = null;

    //--------------------------------------------------------------------------

    public static void drop_lock_on_targets()
    {
        foreach (var player in player_characters)
        {
            var pc = player.GetComponent<Player_character>();
            pc.release_lock_on();
        }
    }// drop_lock_on_targets

    //--------------------------------------------------------------------------

    public virtual void Awake()
    {
        player_characters.Add(gameObject);
    }// Awake

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        process_input();

        base.update_impl();

        if (being_knocked_back)
        {
            stop();
            return;
        }

        if (is_grounded)
        {
            time_in_air = 0;
        }
        else
        {
            time_in_air += Time.deltaTime;
        }

        if (is_locked_on)
        {
            look_toward(lock_on_target);
            return;
        }

        update_rotation(velocity);
    }// update_impl

    //--------------------------------------------------------------------------

    // TODO: change to apply_controller_tilt
    //
    // Given a target velocity_, updates the player's x and z velocities as
    // appropriate.
    // For example, the player should change direction more slowly while in
    // the air.
    // Also applies gravity.
    public virtual void update_movement_velocity(Vector3 target_velocity)
    {
        // print("pc update_movement_velocity");
        // if (is_teamed_up)
        // {
        //     target_velocity = target_velocity.magnitude *
        //                       Camera.main.transform.forward;
        // }
        // velocity_.y += gravity * Time.deltaTime;
        if (animation_controlling_movement)
        {
            return;
        }

        if (is_grounded || is_jumping)
        {
            target_velocity.y = velocity.y;
            velocity = target_velocity;
            return;
        }

        // // target_velocity *= Time.deltaTime;
        // if (have_opposite_signs(target_velocity.x, velocity_.x) ||
        //     Mathf.Abs(target_velocity.x) > Mathf.Abs(velocity_.x))
        // {
        //     var velocity_step = run_acceleration * Time.deltaTime;
        //     if (target_velocity.x < velocity_.x)
        //     {
        //         velocity_step *= -1;
        //     }

        //     velocity_.x += velocity_step;
        // }

        // if (have_opposite_signs(target_velocity.z, velocity_.z) ||
        //     Mathf.Abs(target_velocity.z) > Mathf.Abs(velocity_.z))
        // {
        //     var velocity_step = run_acceleration * Time.deltaTime;
        //     if (target_velocity.z < velocity_.z)
        //     {
        //         velocity_step *= -1;
        //     }

        //     velocity_.z += velocity_step;
        // }
    }// update_movement_velocity

    private bool have_opposite_signs(float first, float second)
    {
        return first < 0 && second > 0 || first > 0 && second < 0;
    }// have_opposite_signs

    //--------------------------------------------------------------------------

    public virtual void apply_momentum(Vector3 new_velocity)
    {
        velocity = new_velocity;
        // if (new_velocity.y > 0)
        // {
        //     on_ground = false;
        // }
    }// apply_momentum

    //--------------------------------------------------------------------------

    public override void on_death(GameObject killer=null)
    {
        Actor.actors_paused = true;
        play_death_animation();

        // print("you die!");
    }// on_death

    protected virtual void play_death_animation()
    {

    }// play_death_animation

    public virtual void on_death_animation_finished()
    {
        reset_health();
        get_other_player().GetComponent<Player_character>().reset_health();

        Checkpoint.load_last_checkpoint(() => Actor.actors_paused = false);
    }

    //--------------------------------------------------------------------------

    public virtual void attack()
    {
        // throw new Exception("Derived classes must override this method");
    }// physical_attack

    //--------------------------------------------------------------------------

    public virtual void toggle_lock_on()
    {
        if (is_locked_on)
        {
            release_lock_on();
            return;
        }

        var closest_enemy =
                Enemy.get_closest_potential_lock_on_target(gameObject);
        var enemy_on_screen = Camera_follow.point_in_viewport(
            closest_enemy.transform.position);
        var enemy_visible = closest_enemy.GetComponent<Renderer>().isVisible;

        if (closest_enemy == null || !enemy_on_screen || !enemy_visible)
        {
            return;
        }

        lock_on_to_enemy(closest_enemy);

        look_toward(lock_on_target);
    }// toggle_lock_on

    private void lock_on_to_enemy(GameObject enemy)
    {
        lock_on_target = enemy;

        lock_on_bar.GetComponent<Lock_on_health_bar>().target = lock_on_target;
        lock_on_bar.GetComponent<Lock_on_health_bar>().update_position();
        lock_on_bar.SetActive(true);
    }// lock_on_to_enemy

    private void release_lock_on()
    {
        lock_on_bar.GetComponent<Lock_on_health_bar>().target = null;
        lock_on_target = null;
    }// release_lock_on

    //--------------------------------------------------------------------------

    public static void notify_enemy_gone(GameObject enemy)
    {
        foreach (var player in player_characters)
        {
            if (player == null)
            {
                continue;
            }

            player.GetComponent<Player_character>().on_enemy_gone(
                enemy.gameObject);
        }
    }// notify_enemy_gone

    //--------------------------------------------------------------------------

    protected virtual void on_enemy_gone(GameObject enemy)
    {
        if (enemy == lock_on_target)
        {
            lock_on_to_enemy(
                Enemy.get_closest_potential_lock_on_target(gameObject));
        }
    }// on_enemy_gone

    //--------------------------------------------------------------------------

    public override Sweep_test_summary move(
        Vector3 delta_position, float precision_pad=0.1f)
    {
        // print("pc move");
        // // print(amount);
        // step_axis_direction(Vector3.right, delta_position.x);
        // // print("delta_position.y: " + delta_position.y);
        // var y_collision = step_axis_direction(Vector3.up, delta_position.y);
        // step_axis_direction(Vector3.forward, delta_position.z);

        // if (delta_position.y < 0)
        // {
        //     // is_jumping = false;
        //     on_ground = y_collision;
        //     if (is_grounded)
        //     {
        //         velocity_.y = 0;
        //     }
        // }

//        var this_player_would_be_off_camera =
//                Camera_follow.point_step_would_leave_viewport(
//                    transform.position, delta_position);
//        var other_player_would_be_off_camera =
//                Camera_follow.point_step_would_leave_viewport(
//                    get_other_player().transform.position, delta_position * -1);
//        if (this_player_would_be_off_camera || other_player_would_be_off_camera)
//        {
//            return;
//        }

        // print("Player_character move");
        // kinematic_rigidbody.MovePosition(GetComponent<Rigidbody>().position + delta_position);
        // on_ground = delta_position.y < 0;// && kinematic_rigidbody.isGrounded;
        var summary = base.move(delta_position, precision_pad);
        // print(delta_position);
        update_rotation(delta_position);
        // if (bounce)
        // {
        //     jump();
        //     bounce = false;
        // }
        if (is_grounded)
        {
            // velocity_.y = 0;
            jumping = false;
        }

        return summary;
        // if (apply_rotation)
        // {
        //     update_rotation(delta_position);
        // }
    }// move

    //--------------------------------------------------------------------------

    public virtual void update_rotation(Vector3 delta_position)
    {
        // print("pc update_rotation: " + delta_position);
        if (is_locked_on)
        {
            look_toward(lock_on_target);
            return;
        }

        // print(delta_position);
        if (delta_position.x == 0 && delta_position.z == 0)
        {
            // print("nope");
            return;
        }

        collision_safe_rotate_towards(
            delta_position, 10 * Time.deltaTime);
    }// update_rotation

    //--------------------------------------------------------------------------

    // // Returns true if a collision would have occurred.
    // private bool step_axis_direction(Vector3 direction, float step_amount)
    // {
    //     if (Mathf.Abs(step_amount) < min_move_distance)
    //     {
    //         // print("too slow! " + Mathf.Abs(step_amount));
    //         return false;
    //     }

    //     var move_increment = direction * step_amount;

    //     // print("before: " + move_increment);
    //     RaycastHit hit_info;
    //     var hit = kinematic_rigidbody.SweepTest(
    //         move_increment, out hit_info,
    //         move_increment.magnitude + skin_width);
    //     if (hit)
    //     {
    //         // print("hit");
    //         move_increment = move_increment.normalized * Mathf.Max(
    //             hit_info.distance - skin_width, 0);
    //     }

    //     transform.position += move_increment;

    //     return hit;
    // }// step_axis_direction

    //--------------------------------------------------------------------------

    public virtual void charge()
    {

    }// charge

    public virtual void stop_charge()
    {

    }// stop_charge

    //--------------------------------------------------------------------------

    public virtual void jump()
    {
        if (!can_jump)
        {
            // print(kinematic_rigidbody.isGrounded);
            return;
        }

        // print("jump_speed: " + jump_speed);
        var new_velocity = velocity;
        new_velocity.y = jump_speed;
        velocity = new_velocity;
        // print("new_velocity: " + new_velocity);
        // print("jump_speed: " + jump_speed);
        is_grounded_ = false;
        jumping = true;
        // Vector3 new_speed = GetComponent<Rigidbody>().velocity_;
        // new_speed.y = jump_speed;
        // GetComponent<Rigidbody>().velocity_ = new_speed;

    }// jump

    //--------------------------------------------------------------------------

    public virtual void team_up_engage_or_throw()
    {

    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    protected void team_up_engage()
    {
        // print(player_characters.Count);
        foreach (var player_char in player_characters)
        {
            var pc = player_char.GetComponent<Player_character>();
            pc.teamed_up = true;
            pc.on_team_up_engage();
        }
    }// team_up_engage

    //--------------------------------------------------------------------------

    protected virtual void on_team_up_engage()
    {
    }// on_team_up_engage

    //--------------------------------------------------------------------------

    public void team_up_disengage()
    {
        if (force_team_up)
        {
            return;
        }

        foreach (var player_char in player_characters)
        {
            var pc = player_char.GetComponent<Player_character>();
            pc.teamed_up = false;
            pc.on_team_up_disengage();
        }
    }// team_up_disengage

    //--------------------------------------------------------------------------

    protected virtual void on_team_up_disengage()
    {
    }// on_team_up_disengage

    //--------------------------------------------------------------------------

    public void notify_on_ground()
    {
        is_grounded_ = true;
    }// notify_on_ground

    //--------------------------------------------------------------------------

    public GameObject get_other_player()
    {
        foreach (var pc in player_characters)
        {
            if (pc != gameObject)
            {
                return pc;
            }
        }
        print("COULDN'T FIND OTHER PLAYER");
        return null;
    }

    //--------------------------------------------------------------------------

    private void process_input()
    {
        if (!controls_enabled)
        {
            return;
        }

        if (input_device.GetControl(InputControlType.Action1).WasPressed)
        {
            jump();
        }

        if (input_device.GetControl(InputControlType.Action2).WasPressed)
        {
            team_up_engage_or_throw();
        }

        if (input_device.GetControl(InputControlType.Action3).WasPressed)
        {
            attack();
        }

        if (input_device.GetControl(InputControlType.Action3).IsPressed)
        {
            charge();
        }

        if (input_device.GetControl(InputControlType.Action3).WasReleased)
        {
            stop_charge();
        }

        if(input_device.GetControl(InputControlType.LeftBumper).WasPressed ||
           input_device.GetControl(InputControlType.LeftBumper).WasReleased &&
           is_locked_on)
        {
            toggle_lock_on();
        }

        var tilt = Vector3.zero;
        tilt.x = input_device.GetControl(InputControlType.LeftStickX).Value;
        tilt.z = input_device.GetControl(InputControlType.LeftStickY).Value;

        var cam_right = Camera.main.transform.right;
        cam_right.y = 0;
        cam_right = cam_right.normalized;
        cam_right *= tilt.x;

        var cam_forward = Camera.main.transform.forward;
        cam_forward.y = 0;
        cam_forward = cam_forward.normalized;
        cam_forward *= tilt.z;

        var relative_move_dir = cam_right + cam_forward;

        var target_velocity = relative_move_dir * run_speed;
        update_movement_velocity(target_velocity);

    }// process_input

    //--------------------------------------------------------------------------

    // enum Sword_swing_state_e
    // {
    //     IDLE,

    // }
    // void sword_swing_coroutine()
    // {

    // }// sword_swing_coroutine

    //--------------------------------------------------------------------------

}
