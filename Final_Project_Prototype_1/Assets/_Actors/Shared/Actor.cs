using UnityEngine;
using System.Collections;
using System;

public class Actor : MonoBehaviour
{
    public int health { get { return health_; } }
    private int health_;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health_ = max_health;
    }// Start()

    //--------------------------------------------------------------------------

    public virtual void Update()
    {

    }// Update()

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
        Vector3 direction, float step)
    {
        direction.y = transform.forward.y;

        var new_forward = Vector3.RotateTowards(
            transform.forward, direction, step, 0f);

        transform.rotation = Quaternion.LookRotation(new_forward);
    }// collision_safe_rotate_towards

    //--------------------------------------------------------------------------

    public virtual void receive_hit(int damage, GameObject attacker=null)
    {
        var invincibility_animation = GetComponent<Flash_animation>();
        if (invincibility_animation != null
            && invincibility_animation.is_playing)
        {
            return;
        }

        health_ -= damage;

        if (health_ <= 0)
        {
            on_death();
        }

        if (invincibility_animation != null)
        {
            invincibility_animation.start_animation();
        }
    }// receive_hit

    //--------------------------------------------------------------------------

    public virtual void on_death()
    {

    }// on_death

    public virtual void special_ability()
    {

    }

    //--------------------------------------------------------------------------

    public virtual int max_health
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// max_health
}
