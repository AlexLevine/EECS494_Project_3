using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Flash_animation)),
 RequireComponent(typeof(Knockback_animation))]
public class Actor : MonoBehaviour
{
    public int health { get { return health_; } }
    public bool being_knocked_back {
        get { return knockback_animation.is_playing; } }

    private int health_;

    private Flash_animation invincibility_animation;
    private Knockback_animation knockback_animation;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health_ = max_health;

        invincibility_animation = GetComponent<Flash_animation>();
        knockback_animation = GetComponent<Knockback_animation>();
    }// Start()

    //--------------------------------------------------------------------------

    public virtual void Update()
    {

    }// Update()

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
