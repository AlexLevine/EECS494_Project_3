using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Flash_animation)),
 RequireComponent(typeof(Knockback_animation)),
 RequireComponent(typeof(Rigidbody))]
public class Actor : MonoBehaviour
{
    public float min_move_distance = 0.001f;
    public float skin_width = 0.1f;

    public Vector3 velocity { get { return velocity_; } }
    public bool is_grounded { get { return on_ground; } }
    public int health { get { return health_; } }
    public bool being_knocked_back {
        get { return knockback_animation.is_playing; } }

    private int health_;
    protected bool on_ground = false;
    protected Vector3 velocity_ = Vector3.zero;

    private Flash_animation invincibility_animation;
    private Knockback_animation knockback_animation;
    private Rigidbody kinematic_rigidbody;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health_ = max_health;

        invincibility_animation = GetComponent<Flash_animation>();
        knockback_animation = GetComponent<Knockback_animation>();
        kinematic_rigidbody = GetComponent<Rigidbody>();
        kinematic_rigidbody.useGravity = false;
        kinematic_rigidbody.isKinematic = true;
    }// Start()

    //--------------------------------------------------------------------------

    public virtual void Update()
    {

    }// Update()

    //--------------------------------------------------------------------------

    public virtual void move(Vector3 delta_position, bool apply_rotation)
    {
        // print(amount);
        step_axis_direction(Vector3.right, delta_position.x);
        // print("delta_position.y: " + delta_position.y);
        var y_collision = step_axis_direction(Vector3.up, delta_position.y);
        // step_axis_direction(Vector3.forward, delta_position.z);

        if (delta_position.y < 0)
        {
            // is_jumping = false;
            on_ground = y_collision;
            if (is_grounded)
            {
                velocity_.y = 0;
            }
        }

    //     var cc = gameObject.GetComponent<CharacterController>();
    //     if (cc != null)
    //     {
    //         cc.Move(delta_position);
    //         if (apply_rotation)
    //         {
    //             collision_safe_rotate_towards(delta_position);
    //         }
    //         return;
    //     }

    //     // HACK
    //     transform.position += delta_position;
    //     if (apply_rotation)
    //     {
    //         collision_safe_rotate_towards(delta_position);
    //     }

        if (is_grounded)
        {
            velocity_.y = 0;
        }

        if (apply_rotation)
        {
            update_rotation(delta_position);
        }
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

        transform.position += move_increment;

        return hit;
    }// step_axis_direction

    //--------------------------------------------------------------------------

    public virtual void update_rotation(Vector3 delta_position)
    {
        // if (is_locked_on)
        // {
        //     look_toward(lock_on_target);
        //     return;
        // }

        if (delta_position.x == 0)// && delta_position.z == 0)
        {
            return;
        }

        collision_safe_rotate_towards(delta_position);
    }// update_rotation

    //--------------------------------------------------------------------------

    public void look_toward(GameObject obj, float step=360f)
    {
        if (obj == null)
        {
            return;
        }

        step *= Time.deltaTime;

        var target_direction =
                obj.transform.position - transform.position;

        collision_safe_rotate_towards(target_direction, step);


        // var new_forward = Vector3.RotateTowards(
        //     transform.forward, target_direction, step, 0f);
        // // only allow rotation around y axis
        // // new_forward.x = obj.transform.eulerAngles.x;
        // // new_forward.z = obj.transform.eulerAngles.z;

        // transform.rotation = Quaternion.LookRotation(new_forward);
    }// look_toward

    //--------------------------------------------------------------------------

    public virtual void collision_safe_rotate_towards(
        Vector3 direction, float step=360f)
    {
        direction.y = transform.forward.y;

        var new_forward = Vector3.RotateTowards(
            transform.forward, direction, step, 0f);

        // var rb = gameObject.GetComponent<Rigidbody>();
        var new_rotation = Quaternion.LookRotation(new_forward);
        // if (rb == null)
        // {
        transform.rotation = new_rotation;
        //     return;
        // }

        // print("spam");
        // rb.MoveRotation(new_rotation);
    }// collision_safe_rotate_towards

    //--------------------------------------------------------------------------

    public virtual void apply_momentum(Vector3 new_velocity)
    {
        velocity_ = new_velocity;
        if (new_velocity.y > 0)
        {
            on_ground = false;
        }
    }// apply_momentum

    //--------------------------------------------------------------------------

    public virtual void receive_hit(int damage, Vector3 knockback_velocity)
    {
        if (invincibility_animation.is_playing)
        {
            return;
        }

        health_ -= damage;

        bool should_die = health_ <= 0;

        invincibility_animation.start_animation();

        // HACK
        knockback_velocity = knockback_velocity.normalized * 10;

        knockback_animation.apply_knockback(knockback_velocity, should_die);
    }// receive_hit

    //--------------------------------------------------------------------------

    public virtual void on_death()
    {

    }// on_death

    //--------------------------------------------------------------------------

    public virtual int max_health
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// max_health
}
