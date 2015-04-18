﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Ninja : Player_character
{
    // public GameObject body;

    public GameObject swing_sound_player;
    public GameObject basic_attack_vocals;
    public GameObject damage_vocals;

    public GameObject projectile_prefab;
    public GameObject sword_obj;
    public Material out_of_range;
    private Material normal;
    // private float o_o_r=2;

    private Animator animator;
    private int attack_button_pressed_trigger_id;

    public override bool can_jump
    {
        get
        {
            return !get_sword().is_attacking &&
                   ((is_teamed_up && !force_team_up) || base.can_jump);
        }
    }

    private GameObject team_up_point;

    private bool is_shrunk = false;
    private Vector3 original_scale;
    private Vector3 shrunk_scale;

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

    public override void Awake()
    {
        // if (instance != null && instance != this)
        // {
        //     print("ninja already exists");
        //     Destroy(gameObject);
        //     return;
        // }

        base.Awake();

        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        attack_button_pressed_trigger_id = Animator.StringToHash("attack_button_pressed");

        team_up_point = Llama.get().get_team_up_point();

        original_scale = transform.localScale;
        shrunk_scale = original_scale;
        shrunk_scale.y /= 2f;
    }// Start

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        base.update_impl();

        // print(velocity.magnitude);

        if (!is_teamed_up)
        {
            //out_of_range
            // if (o_o_r++==1){
            //     body.GetComponent<Renderer>().material = normal;
            // }

            return;
        }

        // on_ground = true;
        // velocity.y = 0;
        transform.position = team_up_point.transform.position;

//        if (!Llama.get().gameObject.GetComponent<Throw_animation>().is_playing)
//        {
           // transform.rotation = team_up_point.transform.parent.rotation;
//        }
    }// update_impl

    //--------------------------------------------------------------------------

    public override void attack()
    {
        if (get_sword().is_attacking)
        {
            return;
        }

        if (!is_grounded)
        {
            GetComponent<Aerial_attack>().start_attack();
            return;
        }
        animator.SetTrigger(attack_button_pressed_trigger_id);
        // GetComponent<Sword_swing>().swing();
    }// physical_attack

    public void on_sword_swing_start()
    {
        get_sword().GetComponent<Collider>().enabled = true;

        basic_attack_vocals.GetComponent<Sound_effect_randomizer>().play();
        swing_sound_player.GetComponent<Sound_effect_randomizer>().play();
    }// on_sword_swing_start

    public void on_sword_swing_end()
    {
        get_sword().GetComponent<Collider>().enabled = true;
    }// on_sword_swing_end()

    //--------------------------------------------------------------------------

    public override Sweep_test_summary move(
        Vector3 delta_position, float precision_pad)
    {
        // if (GetComponent<Sword_swing>().is_swinging || is_teamed_up)
        update_rotation(delta_position);

        if (is_teamed_up)
        {
			return new Sweep_test_summary();
        }

        // if (is_teamed_up)
        // {
        //     // stop();
        //     // delta_position.y = 0;
        //     return;
        // }

        // if (GetComponent<Aerial_attack>().is_diving)
        // {
        //     // HAAAACK
        //     base.move(delta_position);
        //     if (is_grounded)
        //     {
        //         GetComponent<Aerial_attack>().notify_dive_landed();
        //     }
        //     return new Sweep_test_summary();
        // }

        // if (!GetComponent<Aerial_attack>().is_playing)
        // {
            return base.move(delta_position);
        // }

        // if (is_grounded && GetComponent<Aerial_attack>().is_diving)
        // {
        //     notify_on_ground();
        // }
    }// move

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (is_teamed_up || get_sword().sword_animation_playing)
        {
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
        team_up_engage();
        // transform.position = team_up_point.transform.position;
//        sword_obj.SetActive(false);
//        jousting_pole.SetActive(true);

        GetComponent<Team_up_animation>().start_animation();

    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    // public override void collision_safe_rotate_towards(
    //     Vector3 direction, float step)
    // {
    //     if (!GetComponent<Sword_swing>().is_swinging)
    //     {
    //         base.collision_safe_rotate_towards(direction, step);
    //     }
    // }// collision_safe_rotate_towards

    //--------------------------------------------------------------------------

    public override void update_movement_velocity(Vector3 target_velocity)
    {
        // Aerial attack takes control of movement.
        // if (GetComponent<Aerial_attack>().is_playing)
        // {
        //     return;
        // }

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
        }
    }// toggle_shrunk

    //--------------------------------------------------------------------------

    // public void on_thrown()
    // {
    //     on_ground = false;
    // }// on_thrown

    //--------------------------------------------------------------------------

    protected override void on_team_up_engage()
    {
        notify_on_ground();
    }// team_up_engage

    //--------------------------------------------------------------------------

    protected override void on_team_up_disengage()
    {
    }// on_team_up_disengage

    //--------------------------------------------------------------------------

    public override void jump()
    {
        // print("jump");
        if (force_team_up)
        {
            print("team up being forced");
            return;
        }

        if (is_teamed_up)
        {
            team_up_disengage();
        }

        base.jump();
    }// jump

	public override void on_death() {
		Llama.get ().reset_health();
		base.on_death();
	}

    //--------------------------------------------------------------------------

    protected override void play_damage_vocals()
    {
        damage_vocals.GetComponent<Sound_effect_randomizer>().play();
    }// play_damage_vocals

}


