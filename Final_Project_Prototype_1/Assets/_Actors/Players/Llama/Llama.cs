using UnityEngine;
using System.Collections;

public class Llama : Player_character
{
    public GameObject spit_prefab;
    public GameObject spit_spawn_point;

    public bool is_cooling_down = false; 
    public float max_cooldown_time, cur_cooldown_time; 

    public bool is_charging { get { return is_charging_; } }

    private static Llama instance;

    private static float charge_duration = 1f;
    private float time_spent_charging = 0;
    private bool is_charging_;
    public float charge_speed { get { return run_speed * 3; } }
    // private float pre_charge_speed;

    //--------------------------------------------------------------------------

    public static Llama get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public override void Awake()
    {
        base.Awake();

        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    public override void Update()
    {
        base.Update();

        // if (is_teamed_up)
        // {
        //     print(is_grounded);
        // }

        if(is_cooling_down)
        {
            cur_cooldown_time += Time.deltaTime;
            if(cur_cooldown_time >= max_cooldown_time)
            {
                print("cooldown ended");
                cur_cooldown_time = 0;
                is_cooling_down = false; 
            }
        }

        if (!is_charging)
        {
            return;
        }

        if (time_spent_charging >= charge_duration)
        {
            is_charging_ = false;
            time_spent_charging = 0;
            return;
        }


        time_spent_charging += Time.deltaTime;
    }// Update

    //--------------------------------------------------------------------------

    public override void move(Vector3 delta_position)
    {
        if (gameObject.GetComponent<Throw_animation>().is_playing)
        {
            return;
        }

        // cc.Move(delta_position);
        // update_rotation(delta_position);
        // if (is_teamed_up)// && delta_position.y > 0)
        // {
        //     Ninja.get().move(delta_position);//.y * Vector3.up);
        // }

        base.move(delta_position);
        // print(delta_position.y);
        // print(cc.isGrounded);
    }// move

    //--------------------------------------------------------------------------

    public override void update_movement_velocity(Vector3 target_velocity)
    {
        if (!is_charging)
        {
            base.update_movement_velocity(target_velocity);
            return;
        }

        var angle = Vector3.Angle(transform.forward, target_velocity);
        if (angle > 20)
        {
            return;
        }

        target_velocity = target_velocity.normalized * charge_speed;
        base.update_movement_velocity(target_velocity);
    }// update_movement_velocity

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        gameObject.GetComponent<Throw_animation>().start_animation();
    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    public override void attack()
    {
        if(is_cooling_down)
        {
            return; 
        }

        GameObject spit = Instantiate(
            spit_prefab, spit_spawn_point.transform.position,
            transform.rotation) as GameObject;
        var direction = (is_locked_on ?
            (lock_on_target_pos - spit.transform.position).normalized :
            transform.forward);
        print(direction);

        spit.GetComponent<Rigidbody>().velocity = direction * 14f;
        is_cooling_down = true; 
    }// projectile_attack

    //--------------------------------------------------------------------------

    public override void charge()
    {
        if (!is_teamed_up || is_charging)
        {
            return;
        }

        if (is_locked_on)
        {
            toggle_lock_on();
        }

        // pre_charge_speed = velocity.magnitude;
        // pre_charge_speed.y = 0;
        is_charging_ = true;
        apply_momentum(transform.forward * charge_speed);
    }// charge

    //--------------------------------------------------------------------------

    public override void toggle_lock_on()
    {
        if (is_charging && !is_locked_on)
        {
            return;
        }

        base.toggle_lock_on();
    }// toggle_lock_on

    //--------------------------------------------------------------------------

    public override int max_health
    {
        get
        {
            return 100;
        }
    }// max_health
}

