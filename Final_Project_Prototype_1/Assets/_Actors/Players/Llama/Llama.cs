using UnityEngine;
using System.Collections;

public class Llama : Player_character
{
    public GameObject team_up_point;

    public GameObject damage_vocals;

    public GameObject spit_prefab;
    public GameObject spit_spawn_point;

    public bool spit_is_cooling_down = false;
    private static float max_spit_cooldown_time = 0.5f;
    private float cur_spit_cooldown_time = 0;

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
        base.Awake();

        instance = this;
    }// Awake

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

    public override void move(Vector3 delta_position, bool apply_rotation)
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

        base.move(delta_position, apply_rotation);
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

        //var angle = Vector3.Angle(transform.forward, target_velocity);

        //SCIENCE!
        var dot_product = transform.forward.x * target_velocity.x +
                          transform.forward.z * target_velocity.z;
        var determinant = transform.forward.x * target_velocity.z -
                          transform.forward.z * target_velocity.x;
        //gives an angle in (-180,180]
        var angle = Mathf.Rad2Deg * Mathf.Atan2(determinant, dot_product);
        // print(angle);

        var charge_direction = transform.forward;

        print(angle);
        print(target_velocity.magnitude);
        if (Mathf.Abs(angle) > 20 && target_velocity.magnitude > 1f)
        {
            var rotate_amount = Quaternion.AngleAxis(
                angle < 0 ? 2f : -2f, Vector3.up);
            print(rotate_amount);
            charge_direction = rotate_amount * charge_direction;
        }

        // print(angle);
        target_velocity = charge_direction.normalized * charge_speed;
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
        if(spit_is_cooling_down)
        {
            return;
        }

        if (is_teamed_up)
        {
            // Input reader calls charge
            return;
        }

        GameObject spit = Instantiate(
            spit_prefab, spit_spawn_point.transform.position,
            transform.rotation) as GameObject;
        var direction = (is_locked_on ?
            (lock_on_target_pos - spit.transform.position).normalized :
            transform.forward);
//        print(direction);

        spit.GetComponent<Rigidbody>().velocity = direction * 14f;
        spit_is_cooling_down = true;
    }// projectile_attack



    //--------------------------------------------------------------------------

    public override void charge()
    {
        if (!is_teamed_up)
        {
            return;
        }

        charge_state = Charge_state_e.CHARGING;
        var new_velocity = transform.forward * charge_speed;
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

    public override bool receive_hit(
            float damage, Vector3 knockback_velocity, GameObject attacker)
    {
        if(is_teamed_up)
        {
            damage /= 2;
            Ninja.get().receive_hit(damage, Vector3.zero, attacker);
        }

        return base.receive_hit(damage, knockback_velocity, attacker);
    }

    //--------------------------------------------------------------------------

    protected override void play_damage_vocals()
    {
        damage_vocals.GetComponent<Sound_effect_randomizer>().play();
    }// play_damage_vocals

    //--------------------------------------------------------------------------

    // protected override void on_enemy_gone(GameObject enemy)
    // {
    //     if (enemy == lock_on_target)
    //     {

    //         lock_on_target = null;
    //     }
    // }// on_enemy_gone

    //--------------------------------------------------------------------------

    void OnControllerColliderHit(ControllerColliderHit c)
    {
        var ninja = c.gameObject.GetComponent<Ninja>();
        if (ninja == null)
        {
            return;
        }

        if (c.moveDirection.y < 0)
        {
            ninja.toggle_shrunk();
            bounce = true;
        }
    }
}

