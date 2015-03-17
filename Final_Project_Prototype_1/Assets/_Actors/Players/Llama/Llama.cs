using UnityEngine;
using System.Collections;

public class Llama : Player_character
{
    public GameObject spit_prefab;
    public GameObject spit_spawn_point;

    public bool is_charging { get { return is_charging_; } }

    private static Llama instance;

    private static float charge_duration = 1f;
    private float time_spent_charging = 0;
    private bool is_charging_; // TODO: script animation instead

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

    public override void team_up_engage_or_throw()
    {
        gameObject.GetComponent<Throw_animation>().start_animation();
    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    public override void attack()
    {
        GameObject spit = Instantiate(
            spit_prefab, spit_spawn_point.transform.position,
            transform.rotation) as GameObject;
        var direction = (is_locked_on ?
            (lock_on_target_pos - spit.transform.position).normalized :
            transform.forward);
        print(direction);

        spit.GetComponent<Rigidbody>().velocity = direction * 14f;
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

        is_charging_ = true;
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

    //--------------------------------------------------------------------------

    // public override float run_speed
    // {
    //     get
    //     {
    //         return 5;
    //     }
    // }// run_speed

    // //--------------------------------------------------------------------------

    // public override float sprint_speed
    // {
    //     get
    //     {
    //         return 10;
    //     }
    // }// sprint_speed

    //--------------------------------------------------------------------------

    // public override float jump_speed
    // {
    //     get
    //     {
    //         return 15;
    //     }
    // }// jump_speed
}

