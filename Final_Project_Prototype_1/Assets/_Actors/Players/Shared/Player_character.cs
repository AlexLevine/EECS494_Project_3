using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Player_character : Actor
{
    // public int player_id = 0;
    public float min_move_distance = 0.001f;
    public float skin_width = 0.01f;

    public bool is_grounded { get { return on_ground; } }
    public virtual float gravity { get { return -25f; } }
    public bool is_locked_on { get { return lock_on_target != null; } }
    public Vector3 lock_on_target_pos {
        get { return lock_on_target.transform.position; } }
    public virtual float jump_speed { get { return 15f; } }
    public virtual float run_speed { get { return 5f; } }
    public virtual float sprint_speed { get { return run_speed * 2f; } }
    public virtual float acceleration { get { return 20f; } }

    public bool is_teamed_up { get { return teamed_up; } }

    //--------------------------------------------------------------------------

    public static List<GameObject> player_characters = new List<GameObject>();

    //--------------------------------------------------------------------------

    // private Player rewired_player;
    private Rigidbody kinematic_rigidbody;
    private Vector3 velocity = Vector3.zero;

    private bool on_ground;
    private float time_in_air = 0;
    private float max_time_in_air { get { return 0.1f; } }
    private bool teamed_up = false;

    private GameObject lock_on_target = null;
    // private float y_velocity = 0;

    //--------------------------------------------------------------------------

    public virtual void Awake()
    {
        // print("awake");
        // rewired_player = ReInput.players.GetPlayer(player_id);
        // print(rewired_player);
        player_characters.Add(gameObject);
    }// Awake

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        kinematic_rigidbody = gameObject.GetComponent<Rigidbody>();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (on_ground)
        {
            // velocity.y = 0;
            time_in_air = 0;
        }
        else
        {
            time_in_air += Time.deltaTime;
        }

        velocity.y += gravity * Time.deltaTime;

        // process_input();
        move(velocity * Time.deltaTime);

        if (is_locked_on)
        {
            look_toward(lock_on_target);
            return;
        }
    }// Update

    //--------------------------------------------------------------------------

    // Given a target velocity, updates the player's x and z velocities as
    // appropriate.
    // For example, the player should change direction more slowly while in
    // the air.
    public virtual void update_movement_velocity(Vector3 target_velocity)
    {
        if (is_grounded)
        {
            velocity = target_velocity;
            return;
        }

        // target_velocity *= Time.deltaTime;
        if (opposite_sign(target_velocity.x, velocity.x) ||
            Mathf.Abs(target_velocity.x) > Mathf.Abs(velocity.x))
        {
            var velocity_step = acceleration * Time.deltaTime;
            if (target_velocity.x < velocity.x)
            {
                velocity_step *= -1;
            }

            velocity.x += velocity_step;
        }

        if (opposite_sign(target_velocity.z, velocity.z) ||
            Mathf.Abs(target_velocity.z) > Mathf.Abs(velocity.z))
        {
            var velocity_step = acceleration * Time.deltaTime;
            if (target_velocity.z < velocity.z)
            {
                velocity_step *= -1;
            }

            velocity.z += velocity_step;
        }
    }// update_movement_velocity

    private bool opposite_sign(float first, float second)
    {
        return first < 0 && second > 0 || first > 0 && second < 0;
    }// opposite_sign

    //--------------------------------------------------------------------------

    public virtual void apply_momentum(Vector3 new_velocity)
    {
        velocity = new_velocity;
        if (new_velocity.y > 0)
        {
            on_ground = false;
        }
    }// apply_momentum

    //--------------------------------------------------------------------------

    public override void on_death()
    {
        print("you die!");
    }// on_death

    //--------------------------------------------------------------------------

    // public virtual void projectile_attack()
    // {
    //     // throw new Exception("Derived classes must override this method");
    // }// projectile_attack

    //--------------------------------------------------------------------------

    public virtual void attack()
    {
        // throw new Exception("Derived classes must override this method");
    }// physical_attack

    //--------------------------------------------------------------------------

    public virtual void toggle_lock_on()
    {
        // if (teamed_up)
        // {
        //     return;
        // }

        if (is_locked_on)
        {
            lock_on_target = null;
            return;
        }

        var closest_enemy = Enemy.get_closest_potential_lock_on_target(
            gameObject);

        if (closest_enemy == null)
        {
            return;
        }

        var distance = Vector3.Distance(
            transform.position, closest_enemy.transform.position);
        // print("distance: " + distance);
        if (distance > 10f)
        {
            return;
        }

        lock_on_target = closest_enemy;
        look_toward(lock_on_target);
    }// toggle_lock_on

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

    private void on_enemy_gone(GameObject enemy)
    {
        if (enemy == lock_on_target)
        {
            lock_on_target = null;
        }
    }// on_enemy_gone

    //--------------------------------------------------------------------------

    public virtual void toggle_jousting_pole()
    {
    }// toggle_jousting_pole

    //--------------------------------------------------------------------------

    public virtual void adjust_jousting_pole(float vertical_tilt, float horizontal_tilt)
    {
    }// adjust_jousting_pole

    //--------------------------------------------------------------------------

    public virtual void move(Vector3 delta_position)
    {
        // print(amount);
        step_axis_direction(Vector3.right, delta_position.x);
        // print("delta_position.y: " + delta_position.y);
        var y_collision = step_axis_direction(Vector3.up, delta_position.y);
        step_axis_direction(Vector3.forward, delta_position.z);

        if (delta_position.y < 0)
        {
            // is_jumping = false;
            on_ground = y_collision;
            if (on_ground)
            {
                velocity.y = 0;
            }
        }

        if (is_locked_on)
        {
            look_toward(lock_on_target);
            return;
        }

        if (delta_position.x == 0 && delta_position.z == 0)
        {
            return;
        }

        collision_safe_rotate_towards(
            delta_position, 10 * Time.deltaTime);
        // TODO: gradual rotation
        // var turn_to = Mathf.Atan2(
        //     velocity.x, velocity.z) * Mathf.Rad2Deg;

        // transform.rotation = Quaternion.Euler(0, turn_to, 0);
    }// move

    //--------------------------------------------------------------------------

    // Returns true if a collision would have occurred.
    private bool step_axis_direction(Vector3 direction, float step_amount)
    {
        if (Mathf.Abs(step_amount) < min_move_distance)
        {
            // print("too slow! " + Mathf.Abs(step_amount));
            return false;
        }

        var move_increment = direction * step_amount;

        // var desired_position = transform.position + move_increment;
        // var viewport_pos = Camera.main.WorldToViewportPoint(desired_position);
        // if (!point_inside_viewport(viewport_pos))
        // {
        //     return true;
        // }

        var this_player_would_be_off_camera =
                Camera_follow.point_step_would_leave_viewport(
                    transform.position, move_increment, step_amount);
        var other_player_would_be_off_camera =
                Camera_follow.point_step_would_leave_viewport(
                    get_other_player().transform.position, move_increment * -1,
                    step_amount);
        if (this_player_would_be_off_camera || other_player_would_be_off_camera)
        {
            return true;
        }

        // print("before: " + move_increment);
        RaycastHit hit_info;
        var hit = kinematic_rigidbody.SweepTest(
            move_increment, out hit_info,
            move_increment.magnitude + skin_width);
        if (hit)
        {
            // print("hit");
            move_increment = move_increment.normalized * Mathf.Max(
                hit_info.distance - skin_width, 0);
        }

        // print("after: " + move_increment);

        // print("move_increment.y: " + move_increment.y);
        transform.position += move_increment;

        return hit;
    }// step_axis_direction

    //--------------------------------------------------------------------------

    // public static bool hit_edge_of_screen(GameObject obj)
    // {
    //     var bottom_left = Camera.main.ViewportToWorldPoint(Vector3.zero);
    //     print(bottom_left);
    //     var top_right = Camera.main.ViewportToWorldPoint(Vector3.one);
    //     print(top_right);

    //     var obj_off_left = obj.transform.position.x <= bottom_left.x;
    //     var obj_off_right = obj.transform.position.x >= top_right.x;
    //     var obj_off_front = obj.transform.position.z <= bottom_left.z;
    //     var obj_off_back = obj.transform.position.z >= top_right.z;

    //     return obj_off_left || obj_off_right || obj_off_front || obj_off_back;
    // }// hit_edge_of_screen

    //--------------------------------------------------------------------------

    public virtual void charge()
    {

    }// charge

    //--------------------------------------------------------------------------

    public virtual void jump()
    {
        if (!on_ground && time_in_air > max_time_in_air)
        {
            return;
        }

        velocity.y = jump_speed;
        // is_jumping = true;
        on_ground = false;
        // Vector3 new_speed = GetComponent<Rigidbody>().velocity;
        // new_speed.y = jump_speed;
        // GetComponent<Rigidbody>().velocity = new_speed;

    }// jump

    //--------------------------------------------------------------------------

    public virtual void team_up_engage_or_throw()
    {

    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    protected void team_up_engage()
    {
        print(player_characters.Count);
        foreach (var player_char in player_characters)
        {
            player_char.GetComponent<Player_character>().teamed_up = true;
        }
    }// team_up_engage

    //--------------------------------------------------------------------------

    public void team_up_disengage()
    {
        foreach (var player_char in player_characters)
        {
            player_char.GetComponent<Player_character>().teamed_up = false;
        }
    }// team_up_disengage

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

    // //--------------------------------------------------------------------------

    // private static bool point_inside_viewport(Vector3 point)
    // {
    //     return point.x < 0.9f && point.x > 0.1f &&
    //            point.y < 0.9f && point.y > 0.1f;
    // }// point_inside_viewport
}
