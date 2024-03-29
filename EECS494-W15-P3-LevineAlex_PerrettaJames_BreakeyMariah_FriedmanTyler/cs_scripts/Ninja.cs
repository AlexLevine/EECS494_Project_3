﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Ninja : Player_character
{
    // public GameObject swing_sound_player;
    public GameObject basic_attack_vocals;
    public GameObject damage_vocals;
    public GameObject death_vocals;
    public GameObject spin_attack_vocals;
    public GameObject aerial_attack_vocals;
    // public GameObject spin_attack_sound;
    public GameObject squish_sound;
    public GameObject unsquish_sound;
    public GameObject health_item_pickup_sound;

    public GameObject projectile_prefab;
    public GameObject sword_obj;
    public GameObject aerial_attack_shockwave;

    public bool being_thrown = false;

    public override float run_speed { get { return 13f; } }

    // The Ninja is immune to attacks while his sword is able to deal damage.
    public bool sword_swing_invincibility_active
    {
        get { return sword_is_spinning || sword_is_swinging; }
    }

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

    private Animator animator;
    private int attack_button_pressed_trigger_id;
    private int idle_state_hash;
    private int on_ground_param_id;
    private int die_trigger_id;

    public override bool can_jump
    {
        get
        {
            return !animation_controlling_movement &&
                   ((is_teamed_up && !force_team_up) || base.can_jump);
        }
    }

    private GameObject team_up_point;

    private bool is_shrunk = false;
    private Vector3 original_scale;
    private Vector3 shrunk_scale;

    private bool sword_is_swinging = false;
    private bool sword_is_spinning = false;
    private bool is_diving = false;

    //--------------------------------------------------------------------------

    private static Ninja instance;

    //--------------------------------------------------------------------------

    public static Ninja get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public static Ninja_sword get_sword()
    {
        return instance.sword_obj.GetComponent<Ninja_sword>();
    }// get_sword

    //--------------------------------------------------------------------------

    void Awake()
    {
        // base.Awake();

        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        attack_button_pressed_trigger_id = Animator.StringToHash("attack_button_pressed");
        idle_state_hash = Animator.StringToHash("Idle");
        on_ground_param_id = Animator.StringToHash("on_ground");
        die_trigger_id = Animator.StringToHash("die");

        team_up_point = Llama.get().get_team_up_point();

        original_scale = transform.localScale;
        shrunk_scale = original_scale;
        shrunk_scale.y /= 2f;
    }// Start

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        base.update_impl();

        if (is_teamed_up)
        {
            velocity = Vector3.zero;
            transform.position = team_up_point.transform.position;
        }
        else if (is_grounded)
        {
            GetComponent<Collider>().isTrigger = false;
        }

        // // on_sword_swing sets a target velocity. this allows that motion
        // // to take place without being interrupted by the player.
        if (animation_controlling_movement)
        {
            update_physics();
            move(velocity * Time.deltaTime);
        }
    }// update_impl

    //--------------------------------------------------------------------------

    public override void attack()
    {
        if (being_knocked_back)
        {
            return;
        }

        animator.SetTrigger(attack_button_pressed_trigger_id);
    }// physical_attack

    public void on_sword_swing_start()
    {
        velocity = body.transform.forward * run_speed;
        sword_is_swinging = true;
        get_sword().GetComponent<Collider>().enabled = true;
        get_sword().GetComponent<TrailRenderer>().enabled = true;

        basic_attack_vocals.GetComponent<Sound_effect_randomizer>().play();
        // swing_sound_player.GetComponent<Sound_effect_randomizer>().play();
    }// on_sword_swing_start

    public void on_sword_swing_end()
    {
        stop();
        sword_is_swinging = false;
        get_sword().GetComponent<Collider>().enabled = false;
        get_sword().GetComponent<TrailRenderer>().enabled = false;
    }// on_sword_swing_end()

    public void on_sword_spin_start()
    {
        sword_is_spinning = true;

        get_sword().GetComponent<Collider>().enabled = true;
        get_sword().GetComponent<TrailRenderer>().enabled = true;

        spin_attack_vocals.GetComponent<Sound_effect_randomizer>().play();
        // spin_attack_sound.GetComponent<Sound_effect_randomizer>().play();
    }// on_sword_spin_start

    public void on_sword_spin_end()
    {
        animator.ResetTrigger(attack_button_pressed_trigger_id);
        sword_is_spinning = false;
        get_sword().GetComponent<Collider>().enabled = false;
        get_sword().GetComponent<TrailRenderer>().enabled = false;
    }// on_sword_spin_end

    public void on_aerial_attack_wind_up_start()
    {
        print("on_aerial_attack_wind_up_start");
        stop();
        acceleration = -gravity * Vector3.up;
    }// on_aerial_attack_wind_up_start

    public void on_aerial_attack_dive_start()
    {
        print("on_aerial_attack_dive_start");
        // acceleration = Vector3.zero;
        is_diving = true;
        velocity = Vector3.down * 20f;
        get_sword().GetComponent<Collider>().enabled = true;

        aerial_attack_vocals.GetComponent<Sound_effect_randomizer>().play();
    }// on_aerial_attack_dive_start

    public void on_aerial_attack_dive_end()
    {
        print("on_aerial_attack_dive_end");
        animator.ResetTrigger(attack_button_pressed_trigger_id);
        stop();
        is_diving = false;
        aerial_attack_shockwave.SetActive(true);
        aerial_attack_shockwave.GetComponent<Shockwave>().start_shockwave();
        get_sword().GetComponent<Collider>().enabled = false;
    }// on_aerial_attack_dive_end

    //--------------------------------------------------------------------------

    public override Sweep_test_summary move(
        Vector3 delta_position, float precision_pad)
    {
        if (is_teamed_up)
        {
            is_grounded_ = true;
            animator.SetBool(on_ground_param_id, true);
			return new Sweep_test_summary();
        }

        var move_data = base.move(delta_position);

        if (landed_on_llama(move_data))
        {
            team_up_engage_or_throw();
        }

        animator.SetBool(on_ground_param_id, is_grounded);
        if (is_grounded && being_thrown)
        {
            being_thrown = false;
        }

        return move_data;
    }// move

    bool landed_on_llama(Sweep_test_summary move_info)
    {
        if (!move_info.hit_y || move_info.distance_moved.y > 0)
        {
            return false;
        }

        var llama = move_info.hit_info_y.transform.GetComponent<Llama>();
        return llama != null;
    }// landed_on_llama

    //--------------------------------------------------------------------------

    // public override void update_rotation(
    //     Vector3 delta_position, float rotation_speed)
    // {
    //     base.update_rotation(delta_position, 360f);
    // }// update_rotation

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (animation_controlling_movement)
        {
            return;
        }

        if (is_teamed_up)
        {
            jump();
            return;
        }

        var llama_pos = Llama.get().gameObject.transform.position;
        var distance = Vector3.Distance(transform.position, llama_pos);

        RaycastHit hit_info;
        var hit = Physics.Raycast(
            transform.position, llama_pos - transform.position, out hit_info,
            distance);

        Debug.DrawRay(transform.position, llama_pos - transform.position, Color.blue, 4f);

        var blocked = hit && hit_info.collider.gameObject.tag != "Player" &&
                      !hit_info.collider.isTrigger;

        if (distance > 10f)
        {
            // print("out of range");
            // normal = body.GetComponent<Renderer>().material;
            // body.GetComponent<Renderer>().material = out_of_range;
            // o_o_r = 0;
            return;
        }

        if (blocked)
        {
            print("blocked by: " + hit_info.collider.gameObject.name);
            return;
        }

        // print("teaming up");
        // transform.position = team_up_point.transform.position;
//        sword_obj.SetActive(false);
//        jousting_pole.SetActive(true);

        GetComponent<Team_up_animation>().start_animation();
        team_up_engage();
    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    public override bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker,
        float knockback_duration)
    {
        if (sword_is_spinning || sword_is_swinging || is_diving)
        {
            return false;
        }
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
    }// play_death_vocals

    //--------------------------------------------------------------------------

    public override void update_movement_velocity(Vector3 target_velocity)
    {
        if (being_thrown)
        {
            return;
        }

        if (sword_is_spinning)
        {
            // print("updating movment velocity for spinning sword");
            target_velocity.y = velocity.y;
            velocity = target_velocity;
            return;
        }

        base.update_movement_velocity(target_velocity);
    }// update_movement_velocity

    //--------------------------------------------------------------------------

    public void toggle_shrunk()
    {
        is_shrunk = !is_shrunk;
        // var old_center = GetComponent<Collider>().bounds.center;
        transform.localScale = is_shrunk ? shrunk_scale : original_scale;

        if (!is_shrunk)
        {
            var adjusted_pos = transform.position;
            adjusted_pos.y += GetComponent<Collider>().bounds.extents.y;
            transform.position = adjusted_pos;

            unsquish_sound.GetComponent<Sound_effect_randomizer>().play();
            return;
        }

        squish_sound.GetComponent<Sound_effect_randomizer>().play();
    }// toggle_shrunk

    //--------------------------------------------------------------------------

    protected override void on_team_up_engage()
    {
        notify_on_ground();
        GetComponent<Collider>().isTrigger = true;
    }// team_up_engage

    //--------------------------------------------------------------------------

    protected override void on_team_up_disengage()
    {
    }// on_team_up_disengage

    //--------------------------------------------------------------------------

    protected override void on_jump()
    {
        if (is_teamed_up)
        {
            team_up_disengage();
        }

        base.on_jump();
    }// jump

    //--------------------------------------------------------------------------

	// public override void on_death(GameObject killer)
 //    {
	// 	Llama.get ().reset_health();
	// 	base.on_death(killer);
	// }

    //--------------------------------------------------------------------------

    public override void play_damage_vocals()
    {
        damage_vocals.GetComponent<Sound_effect_randomizer>().play();
    }// play_damage_vocals

    // HACK
    public void play_damage_vocal_for_death_animation()
    {
        damage_vocals.GetComponent<Sound_effect_randomizer>().play();
    }

    protected override void play_health_item_sound()
    {
        health_item_pickup_sound.GetComponent<Sound_effect_randomizer>().play();
    }
}


