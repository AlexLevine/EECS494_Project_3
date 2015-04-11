using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Flash_animation)),
 RequireComponent(typeof(Knockback_animation))]
public class Actor : MonoBehaviour
{
    public float health { get { return health_; } }
    public bool being_knocked_back {
        get { return knockback_animation.is_playing; } }

    public virtual float gravity { get { return -25f; } }

    private float health_;

    private Flash_animation invincibility_animation;
    private Knockback_animation knockback_animation;

    public static bool actors_paused = false;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health_ = max_health;

        invincibility_animation = GetComponent<Flash_animation>();
        knockback_animation = GetComponent<Knockback_animation>();
    }// Start()

    //--------------------------------------------------------------------------

    void Update()
    {
        if (actors_paused)
        {
            return;
        }

        update_impl();
    }// Update()

    protected virtual void update_impl()
    {

    }// update_impl

    //--------------------------------------------------------------------------

    public virtual void move(Vector3 delta_position, bool apply_rotation)
    {
        var cc = gameObject.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.Move(delta_position);
            if (apply_rotation)
            {
                collision_safe_rotate_towards(delta_position);
            }
            return;
        }

        // HACK
        transform.position += delta_position;
        if (apply_rotation)
        {
            collision_safe_rotate_towards(delta_position);
        }
    }// move

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

    protected void reset_health()
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

    //--------------------------------------------------------------------------

    public virtual float max_health
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// max_health
}
