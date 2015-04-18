﻿using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody)),
 RequireComponent(typeof(Flash_animation)),
 RequireComponent(typeof(Knockback_animation))]
public class Actor : MonoBehaviour
{
    public static bool actors_paused = false;

    // This should reference the game object that contains the actor's visible body.
    // Since we are using box colliders and don't have a good way to detect
    // collisions resulting from rotating, we will instead rotate the
    // character's body rather than its colliders.
    public GameObject body;

    public virtual float max_health {
        get { throw new Exception(
            "Derived classes must override this property"); } }
    public float health { get { return health_; } }
    public bool is_grounded { get { return is_grounded_; } }

    public bool being_knocked_back {
        get { return knockback_animation.is_playing; } }

    public virtual bool animation_controlling_movement {
        get { return knockback_animation.is_playing; } }

    public virtual float gravity { get { return -25f; } }

    public virtual Vector3 acceleration { get { return acceleration_; } }
    public Vector3 velocity { get { return velocity_; } set { velocity_ = value; }}

    private float health_;
    protected Rigidbody kinematic_rigidbody;
    protected bool is_grounded_;

    private Flash_animation invincibility_animation;
    private Knockback_animation knockback_animation;

    private Vector3 velocity_ = Vector3.zero;
    private Vector3 acceleration_ = Vector3.zero;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health_ = max_health;

        invincibility_animation = GetComponent<Flash_animation>();
        knockback_animation = GetComponent<Knockback_animation>();

        kinematic_rigidbody = GetComponent<Rigidbody>();
    }// Start()

    //--------------------------------------------------------------------------

    void Update()
    {
        if (actors_paused || animation_controlling_movement)
        {
            return;
        }

        update_impl();
    }// Update()

    //--------------------------------------------------------------------------

    protected virtual void update_impl()
    {
        update_physics();
        move(velocity * Time.deltaTime);
    }// update_impl

    //--------------------------------------------------------------------------

    void update_physics()
    {
        var net_acceleration = acceleration;
        net_acceleration.y += gravity;
        // print("net_acceleration: " + net_acceleration);
        velocity_ += net_acceleration * Time.deltaTime;
    }

    //--------------------------------------------------------------------------

    // Moves this actor the specified amount, resolves any collisions that
    // occur, and returns information about those collisions.
    public virtual Sweep_test_summary move(
        Vector3 delta_position, float precision_pad=0.1f)
    {
        // print("base_move");
        var summary = new Sweep_test_summary();
        summary.distance_moved = delta_position;

        // Check for obstacles.
        summary.hit_x = sweep_test_all_filter(
            delta_position.x * Vector3.right, out summary.hit_info_x,
            delta_position.magnitude + precision_pad);
        summary.hit_y = sweep_test_all_filter(
            delta_position.y * Vector3.up, out summary.hit_info_y,
            delta_position.magnitude + precision_pad);
        summary.hit_z = sweep_test_all_filter(
            delta_position.z * Vector3.forward, out summary.hit_info_z,
            delta_position.magnitude + precision_pad);

        if (summary.hit_x)
        {
            summary.distance_moved.x = delta_position.normalized.x * Mathf.Max(
                summary.hit_info_x.distance - precision_pad, 0);
        }
        if (summary.hit_y && !summary.hit_info_y.collider.isTrigger)
        {
            summary.distance_moved.y = delta_position.normalized.y * Mathf.Max(
                summary.hit_info_y.distance - precision_pad, 0);
        }
        if (summary.hit_z && !summary.hit_info_z.collider.isTrigger)
        {
            summary.distance_moved.z = delta_position.normalized.z * Mathf.Max(
                summary.hit_info_z.distance - precision_pad, 0);
        }

        transform.position += summary.distance_moved;

        if (delta_position.y < 0 && summary.hit_y)
        {
            is_grounded_ = true;
            summary.hit_ground = true;
            velocity_.y = 0;
        }
        if (delta_position.y > 0)
        {
            if (summary.hit_y)
            {
                velocity_ = Vector3.zero;
            }
            is_grounded_ = false;
        }

        return summary;
    }// move

    //--------------------------------------------------------------------------

    bool sweep_test_all_filter(
        Vector3 delta_position, out RaycastHit hit_info, float distance)
    {
        var hits = kinematic_rigidbody.SweepTestAll(delta_position, distance);
        if (hits.Length == 0)
        {
            hit_info = new RaycastHit();
            return false;
        }

        bool found_valid_hit = false;
        RaycastHit closest_non_trigger_hit = new RaycastHit();
        foreach (var hit in hits)
        {
            if (hit.collider.isTrigger)
            {
                continue;
            }

            if (!found_valid_hit ||
                hit.distance < closest_non_trigger_hit.distance)
            {
                closest_non_trigger_hit = hit;
                found_valid_hit = true;
            }
        }

        hit_info = closest_non_trigger_hit;
        return found_valid_hit;
    }// sweep_test_all_filter

    //--------------------------------------------------------------------------

    public void look_toward(GameObject obj, float step=10f)
    {
        if (obj == null)
        {
            return;
        }

        look_toward(obj.transform.position, step);
    }// look_toward

    //--------------------------------------------------------------------------

    public void look_toward(Vector3 point, float step=10f)
    {
        // TODO: should we be multiplying by deltaTime in the above overload?
        step *= Time.deltaTime;

        var target_direction =
                point - transform.position;

        collision_safe_rotate_towards(target_direction, step);
    }

    //--------------------------------------------------------------------------

    public virtual void collision_safe_rotate_towards(
        Vector3 direction, float step=10f)
    {
        direction.y = body.transform.forward.y;

        var new_forward = Vector3.RotateTowards(
            body.transform.forward, direction, step, 0f);

        // var rb = gameObject.GetComponent<Rigidbody>();
        var new_rotation = Quaternion.LookRotation(new_forward);
        // if (rb == null)
        // {
        // GetComponent<Rigidbody>().MoveRotation(new_rotation);
        body.transform.rotation = new_rotation;
        //     return;
        // }

        // print("spam");
        // rb.MoveRotation(new_rotation);
    }// collision_safe_rotate_towards

    //--------------------------------------------------------------------------

    // returns true if the hit is fatal
    public virtual bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker)
    {
        if (invincibility_animation.is_playing)
        {
            return false;
        }
        // if(Llama.get().is_charging)
        // {
        //     return false;
        // }


        health_ -= damage;
        play_damage_vocals();

        bool should_die = health_ <= 0;

        // HACK: this lets you use this function for damageless knockback
        if (damage != 0)
        {
            invincibility_animation.start_animation();
        }

        // HACK
        knockback_velocity = knockback_velocity.normalized * 10;

        knockback_animation.apply_knockback(knockback_velocity, should_die);

        return should_die;
    }// receive_hit

    protected virtual void play_damage_vocals()
    {
    }

    //--------------------------------------------------------------------------

    public virtual void on_death()
    {

    }// on_death

    public void reset_health()
    {
        health_ = max_health;
    }

    public void add_health(int hp)
    {
        health_ += hp;
        if(health_ > max_health)
        {
            health_ = max_health;
        }
    }
}

//------------------------------------------------------------------------------
//------------------------------------------------------------------------------

public class Sweep_test_summary
{
    public bool hit_x;
    public bool hit_y;
    public bool hit_z;

    public RaycastHit hit_info_x;
    public RaycastHit hit_info_y;
    public RaycastHit hit_info_z;

    public Vector3 distance_moved;
    public bool hit_ground;
}// Sweep_test_summary

