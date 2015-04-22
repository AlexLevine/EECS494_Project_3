using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Llama : Player_character
{
    public GameObject team_up_point;

    public GameObject damage_vocals;
    public GameObject charge_and_throw_vocals;
    public GameObject spit_sounds;
    public GameObject death_vocals;

    public GameObject spit_prefab;
    public GameObject spit_spawn_point;

    public override bool animation_controlling_movement
    {
        get
        {
            var cur_state_hash =
                    animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            return cur_state_hash != idle_state_hash ||
                   base.animation_controlling_movement;
        }
    }

    public bool spit_is_cooling_down = false;
    private static float max_spit_cooldown_time = 0.75f;
    private float cur_spit_cooldown_time = 0;

    private Animator animator;
    private int throw_button_pressed_trigger_id;
    private int idle_state_hash;
    private int die_trigger_id;

    private float throw_speed = 20f;

    private enum Charge_state_e
    {
        NOT_CHARGING,
        CHARGING,
        COOLING_DOWN
    }

    private Charge_state_e charge_state;

    public bool is_charging { get {
        return charge_state != Charge_state_e.NOT_CHARGING; } }

    private static Llama instance;

    private static float charge_cooldown_duration = 0.1f;
    private float time_spent_cooling_down_charge = 0;
    public float charge_speed { get { return run_speed * 2; } }
    // private float pre_charge_speed;

    //--------------------------------------------------------------------------

    public static Llama get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public GameObject get_team_up_point()
    {
        return team_up_point;
    }// get_team_up_point

    //--------------------------------------------------------------------------

    public override void Awake()
    {
        // if (instance != null && instance != this)
        // {
        //     print("llama already exists");
        //     Destroy(gameObject);
        //     return;
        // }

        base.Awake();

        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        throw_button_pressed_trigger_id = Animator.StringToHash(
            "throw_button_pressed");
        idle_state_hash = Animator.StringToHash("Idle");
        die_trigger_id = Animator.StringToHash("die");
    }// Start

    //--------------------------------------------------------------------------

    public override void stop()
    {
        base.stop();

        if (is_charging)
        {
            charge_state = Charge_state_e.COOLING_DOWN;
        }
    }

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        base.update_impl();

        // print(velocity.magnitude);
        // if (is_teamed_up)
        // {
        //     print(is_grounded);
        // }

        if(spit_is_cooling_down)
        {
            cur_spit_cooldown_time += Time.deltaTime;
            if(cur_spit_cooldown_time >= max_spit_cooldown_time)
            {
//                print("cooldown ended");
                cur_spit_cooldown_time = 0;
                spit_is_cooling_down = false;
            }
        }

        switch (charge_state)
        {
        case Charge_state_e.NOT_CHARGING:
            break;

        case Charge_state_e.CHARGING:
            break;

        case Charge_state_e.COOLING_DOWN:
            if (time_spent_cooling_down_charge >= charge_cooldown_duration)
            {
                charge_state = Charge_state_e.NOT_CHARGING;
                time_spent_cooling_down_charge = 0;
                break;
            }

            time_spent_cooling_down_charge += Time.deltaTime;
            break;
        }
    }// update_impl

    //--------------------------------------------------------------------------

    public override Sweep_test_summary move(
        Vector3 delta_position, float precision_pad=0.1f)
    {
        // if (gameObject.GetComponent<Throw_animation>().is_playing)
        // {
        //     return new Sweep_test_summary();
        // }

        // cc.Move(delta_position);
        // update_rotation(delta_position);
        // if (is_teamed_up)// && delta_position.y > 0)
        // {
        //     Ninja.get().move(delta_position);//.y * Vector3.up);
        // }

        var summary = base.move(delta_position);

        if (is_teamed_up)
        {
            Ninja.get().transform.position += summary.distance_moved;
        }

        if (summary.hit_x)
        {
            var breakable_wall = summary.hit_info_x.transform.GetComponent<Breakable_wall>();
            if (breakable_wall != null && is_charging)
            {
                breakable_wall.break_wall();
                // Prevent the player from stopping briefly when they hit the wall.
                var nudged_pos = transform.position;
                nudged_pos.x += delta_position.x;
                transform.position = nudged_pos;
            }
        }

        if (summary.hit_z)
        {
            var breakable_wall = summary.hit_info_z.transform.GetComponent<Breakable_wall>();
            if (breakable_wall != null && is_charging)
            {
                breakable_wall.break_wall();
                // Prevent the player from stopping briefly when they hit the wall.
                var nudged_pos = transform.position;
                nudged_pos.z += delta_position.z;
                transform.position = nudged_pos;
            }
        }

        if (!summary.hit_y)
        {
            return summary;
        }

        var ninja = summary.hit_info_y.transform.GetComponent<Ninja>();
        if (ninja != null && summary.hit_ground)
        {
            ninja.toggle_shrunk();
            jump();
        }

        return summary;
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

        //var angle = Vector3.Angle(body.transform.forward, target_velocity);

        //SCIENCE!
        var dot_product = body.transform.forward.x * target_velocity.x +
                          body.transform.forward.z * target_velocity.z;
        var determinant = body.transform.forward.x * target_velocity.z -
                          body.transform.forward.z * target_velocity.x;
        //gives an angle in (-180,180]
        var angle = Mathf.Rad2Deg * Mathf.Atan2(determinant, dot_product);
        // print(angle);

        var charge_direction = body.transform.forward;

        // print(angle);
        // print(target_velocity.magnitude);
        if (Mathf.Abs(angle) > 20 && target_velocity.magnitude > 1f)
        {
            var rotate_amount = Quaternion.AngleAxis(
                angle < 0 ? 2f : -2f, Vector3.up);
            // print(rotate_amount);
            charge_direction = rotate_amount * charge_direction;
        }

        // print(angle);
        target_velocity = charge_direction.normalized * charge_speed;
        base.update_movement_velocity(target_velocity);
    }// update_movement_velocity

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (animation_controlling_movement)
        {
            return;
        }

        animator.SetTrigger(throw_button_pressed_trigger_id);
        charge_and_throw_vocals.GetComponent<Sound_effect_randomizer>().play();

        if (!is_teamed_up)
        {
            return;
        }

        // print("teamed up: " + transform.up);
        Ninja.get().being_thrown = true;
        team_up_disengage();
        // Ninja.get().on_thrown();
        Ninja.get().velocity = (
            body.transform.up + body.transform.forward) * throw_speed;
    }// team_up_engage_or_throw

    public void on_throw_animation_end()
    {
        animator.ResetTrigger(throw_button_pressed_trigger_id);
    }

    //--------------------------------------------------------------------------

    public override void attack()
    {
        if(spit_is_cooling_down)
        {
            return;
        }

        if (is_teamed_up)
        {
            // Input reader calls charge
            return;
        }

        spit_sounds.GetComponent<Sound_effect_randomizer>().play();

        GameObject spit = Instantiate(
            spit_prefab, spit_spawn_point.transform.position,
            transform.rotation) as GameObject;
        var direction = (is_locked_on ?
            (lock_on_target_pos - spit.transform.position).normalized :
            body.transform.forward);
//        print(direction);

        spit.GetComponent<Rigidbody>().velocity = direction * 40f;
        spit_is_cooling_down = true;
    }// attack

    //--------------------------------------------------------------------------

    public override void charge()
    {
        if (!is_teamed_up)
        {
            return;
        }

        if (!is_charging)
        {
            charge_and_throw_vocals.GetComponent<Sound_effect_randomizer>().play();
        }

        charge_state = Charge_state_e.CHARGING;
        var new_velocity = body.transform.forward * charge_speed;
        new_velocity.y = velocity.y;
        apply_momentum(new_velocity);
    }// charge

    public override void stop_charge()
    {
        if (!is_charging)
        {
            return;
        }

        charge_state = Charge_state_e.COOLING_DOWN;
    }// stop_charge

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

    public override bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker,
        float knockback_duration)
    {
        if(!is_teamed_up)
        {
            return base.receive_hit(
                damage, knockback_velocity, attacker,
                knockback_duration);
        }

        if (Ninja.get().sword_swing_invincibility_active)
        {
            return false;
        }

        damage /= 2;
        Ninja.get().receive_hit(
            damage, Vector3.zero, attacker, knockback_duration);

        return base.receive_hit(
            damage, knockback_velocity, attacker, knockback_duration);

    }// receive_hit

    protected override void play_death_animation()
    {
        animator.SetTrigger(die_trigger_id);
    }// play_death_animation

    public void play_death_vocals()
    {
        death_vocals.GetComponent<Sound_effect_randomizer>().play();
    }//

    //--------------------------------------------------------------------------

	// public override void on_death(GameObject killer)
 //    {
	// 	Ninja.get().reset_health();
	// 	base.on_death(killer);
	// }

    //--------------------------------------------------------------------------

    protected override void play_damage_vocals()
    {
        damage_vocals.GetComponent<Sound_effect_randomizer>().play();
    }// play_damage_vocals
}

