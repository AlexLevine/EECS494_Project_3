using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Actor : MonoBehaviour
{
    public static bool actors_paused = false;

    // This should reference the game object that contains the actor's visible body.
    // Since we are using box colliders and don't have a good way to detect
    // collisions resulting from rotating, we will rotate the
    // character's body instead of its colliders.
    public GameObject body;

    public virtual float max_health {
        get { throw new Exception(
            "Derived classes must override this property"); } }
    public float health { get { return health_; } }
    public bool is_grounded { get { return is_grounded_; } }

    public bool being_knocked_back {
        get { return being_knocked_back_; } }
    public bool taking_damage_animation_playing {
        get { return taking_damage_animation_playing_; } }

    public virtual bool animation_controlling_movement {
        get { return being_knocked_back_; } }

    public virtual float gravity { get { return -25f; } }
    public virtual Vector3 acceleration {
        get { return acceleration_; } set { acceleration_ = value; }}
    public Vector3 velocity {
        get { return velocity_; } set { velocity_ = value; } }

    // number of times to sweep test when calling move
    protected virtual int num_sweeps { get { return 1; } }

    protected Rigidbody kinematic_rigidbody;
    protected bool is_grounded_;
    protected virtual float invincibility_flash_duration {
        get { return 0.5f; } }

    private float health_;

    private bool taking_damage_animation_playing_ = false;
    private bool being_knocked_back_ = false;

    private Vector3 velocity_ = Vector3.zero;
    private Vector3 acceleration_ = Vector3.zero;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health_ = max_health;

        // invincibility_animation = GetComponent<Flash_animation>();
        // knockback_animation = GetComponent<Knockback_animation>();

        kinematic_rigidbody = GetComponent<Rigidbody>();
        kinematic_rigidbody.isKinematic = false;
        kinematic_rigidbody.useGravity = false;
    }// Start()

    //--------------------------------------------------------------------------

    void FixedUpdate()
    {
        if (actors_paused)
        {
            return;
        }

        update_impl();
    }// Update()

    //--------------------------------------------------------------------------

    protected virtual void update_impl()
    {
        if (animation_controlling_movement)
        {
            return;
        }

        update_physics();
        move(velocity * Time.deltaTime);
    }// update_impl

    //--------------------------------------------------------------------------

    protected void update_physics()
    {
        var net_acceleration = acceleration;
        net_acceleration.y += gravity;
        // print("net_acceleration: " + net_acceleration);
        velocity_ += net_acceleration * Time.deltaTime;
    }

    //--------------------------------------------------------------------------

    public virtual void stop()
    {
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }// stop

    //--------------------------------------------------------------------------

    // Moves this actor the specified amount, resolves any collisions that
    // occur, and returns information about those collisions.
    public virtual Sweep_test_summary move(
        Vector3 delta_position, float precision_pad=0.2f)
    {
        // print("base_move");
        var summary = new Sweep_test_summary();
        summary.distance_moved = delta_position;

        // TODO: cast and move individually instead of cast all and move all.
        // Check for obstacles.
        summary.hit_x = sweep_test_all_filter(
            delta_position.x * Vector3.right, out summary.hit_info_x,
            delta_position.magnitude + precision_pad);
        if (summary.hit_x)
        {
            summary.distance_moved.x = delta_position.normalized.x * (
                summary.hit_info_x.distance - precision_pad);
        }
        // transform.position += new Vector3(summary.distance_moved.x, 0, 0);

        summary.hit_z = sweep_test_all_filter(
            delta_position.z * Vector3.forward, out summary.hit_info_z,
            delta_position.magnitude + precision_pad);
        if (summary.hit_z)
        {
            summary.distance_moved.z = delta_position.normalized.z * (
                summary.hit_info_z.distance - precision_pad);
        }
        // transform.position += new Vector3(0, 0, summary.distance_moved.z);

        summary.hit_y = sweep_test_all_filter(
            delta_position.y * Vector3.up, out summary.hit_info_y,
            delta_position.magnitude + precision_pad);
        if (summary.hit_y)
        {
            summary.distance_moved.y = delta_position.normalized.y * (
                summary.hit_info_y.distance - precision_pad);
        }
        // transform.position += new Vector3(0, summary.distance_moved.y, 0);

        kinematic_rigidbody.MovePosition(
            kinematic_rigidbody.position + summary.distance_moved);
        // transform.position += summary.distance_moved;

        if (delta_position.y < 0)
        {
            is_grounded_ = summary.hit_y;
            summary.hit_ground = summary.hit_y;
            if (summary.hit_y)
            {
                velocity_.y = 0;
            }
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
        // It appears that sweep test "misses" sometimes. If at first you
        // don't succeed, try again.
        for (int i = 0; i < num_sweeps; ++i)
        {
            var hits = kinematic_rigidbody.SweepTestAll(delta_position, distance);
            if (hits.Length == 0)
            {
                continue;
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
        }

        hit_info = new RaycastHit();
        return false;
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

        // print("direction: " + direction);
        var new_forward = Vector3.RotateTowards(
            body.transform.forward, direction, step, 0f);

        // var rb = gameObject.GetComponent<Rigidbody>();
        var new_rotation = Quaternion.LookRotation(new_forward);

        // HACK to hopefully prevent the heisen bug where llama suddenly rotates
        // backwards around the x axis.
        if (new_rotation.eulerAngles.x != 0 || new_rotation.eulerAngles.z != 0)
        {
            // print("WTF new_rotation: " + new_rotation.eulerAngles);
            var adjusted_rotation = new_rotation.eulerAngles;
            adjusted_rotation.x = 0;
            adjusted_rotation.z = 0;
            new_rotation = Quaternion.Euler(adjusted_rotation);
        }
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
        float damage, Vector3 knockback_velocity, GameObject attacker,
        float knockback_duration=0.5f)
    {
        if (taking_damage_animation_playing_)
        {
            return false;
        }

        health_ -= damage;

        bool should_die = health_ <= 0;

        if (should_die)
        {
            on_death(attacker);
            return should_die;
        }

        play_damage_vocals();

        StartCoroutine(
            apply_knockback(knockback_velocity.normalized * 10f,
                            knockback_duration));
        StartCoroutine(
            invincibility_flash_animation(invincibility_flash_duration));

        return should_die;
    }// receive_hit

    //--------------------------------------------------------------------------

    IEnumerator apply_knockback(
        Vector3 knockback_velocity, float knockback_duration)
    {
        being_knocked_back_ = true;

        stop();
        // velocity = knockback_velocity;

        var time_elapsed = 0f;
        while (time_elapsed < knockback_duration)
        {
            move(knockback_velocity * Time.deltaTime);
            time_elapsed += Time.deltaTime;
            yield return null;
        }

        // stop();

        being_knocked_back_ = false;
    }// apply_knockback_and_flash

    //--------------------------------------------------------------------------

    IEnumerator invincibility_flash_animation(float duration)
    {
        taking_damage_animation_playing_ = true;

        var renderers = new List<MeshRenderer>(
            GetComponentsInChildren<MeshRenderer>());
        renderers.AddRange(
            new List<MeshRenderer>(GetComponents<MeshRenderer>()));

        var main_renderer = GetComponent<MeshRenderer>();
        if (main_renderer != null)
        {
            renderers.Add(main_renderer);
        }

        // toggle_renderers(renderers);

        // var time_to_next_toggle = Time.fixedDeltaTime * 10f;
        // var time_elapsed = 0f;
        for (int i = 0; i < duration / 0.05f; ++i)
        {
            // print(time_to_next_toggle);
            // time_to_next_toggle -= Time.deltaTime;
            // if (time_to_next_toggle > 0)
            // {
            //     continue;
            // }

            // time_to_next_toggle = Time.fixedDeltaTime * 10f;
            toggle_renderers(renderers);

            // time_elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.05f);
        }

        // foreach(var renderer in renderers)
        // {
        //     renderer.enabled = true;
        // }

        taking_damage_animation_playing_ = false;
    }// invincibility_flash_animation

    void toggle_renderers(List<MeshRenderer> renderers)
    {
        foreach(var renderer in renderers)
        {
            renderer.enabled = !renderer.enabled;
        }
    }// toggle_renderers

    //--------------------------------------------------------------------------

    public virtual void play_damage_vocals()
    {
    }

    //--------------------------------------------------------------------------

    public virtual void on_death(GameObject killer)
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

        play_health_item_sound();
    }

    protected virtual void play_health_item_sound()
    {
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

