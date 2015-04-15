using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Sword_swing)),
 RequireComponent(typeof(Aerial_attack)),
 RequireComponent(typeof(Team_up_animation))]
public class Ninja : Player_character
{
    public GameObject body;

    public GameObject damage_vocals;

    public GameObject projectile_prefab;
    public GameObject sword_obj;
    public Material out_of_range;
    private Material normal;
    private float o_o_r=2;

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
        if (instance != null && instance != this)
        {
            print("ninja already exists");
            Destroy(gameObject);
            return;
        }

        base.Awake();

        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
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
            if (o_o_r++==1){
                body.GetComponent<Renderer>().material = normal;
            }

            return;
        }

        // on_ground = true;
        // velocity.y = 0;
        transform.position = team_up_point.transform.position;

//        if (!Llama.get().gameObject.GetComponent<Throw_animation>().is_playing)
//        {
//            transform.rotation = team_up_point.transform.parent.rotation;
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
        GetComponent<Sword_swing>().swing();
    }// physical_attack

    //--------------------------------------------------------------------------

    public override void move(Vector3 delta_position, bool apply_rotation)
    {
        // if (GetComponent<Sword_swing>().is_swinging || is_teamed_up)
        if (is_teamed_up)

        {
			if (apply_rotation) update_rotation(delta_position);
			return;
        }

        // if (is_teamed_up)
        // {
        //     // stop();
        //     // delta_position.y = 0;
        //     return;
        // }

        if (GetComponent<Aerial_attack>().is_diving)
        {
            // HAAAACK
            base.move(delta_position, false);
            if (is_grounded)
            {
                GetComponent<Aerial_attack>().notify_dive_landed();
            }
            return;
        }

        if (!GetComponent<Aerial_attack>().is_playing)
        {
            base.move(delta_position, apply_rotation);
        }

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
            print("out of range");
            normal = body.GetComponent<Renderer>().material;
            body.GetComponent<Renderer>().material = out_of_range;
            o_o_r = 0;
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
        if (GetComponent<Aerial_attack>().is_playing)
        {
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
        cc.enabled = false;
    }// team_up_engage

    //--------------------------------------------------------------------------

    protected override void on_team_up_disengage()
    {
        cc.enabled = true;
    }// on_team_up_disengage

    //--------------------------------------------------------------------------

    public override void jump()
    {
        print("jump");
        if (get_sword().is_attacking)
        {
            return;
        }

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

    //--------------------------------------------------------------------------


    // public override int max_health
    // {
    //     get
    //     {
    //         return 10;
    //     }
    // }// max_health

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

    // //--------------------------------------------------------------------------

    // public override float jump_speed
    // {
    //     get
    //     {
    //         return 15;
    //     }
    // }// jump_speed
}


