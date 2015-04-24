using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Samurai_Attack : Enemy
{
    public GameObject damage_vocals;
    public GameObject death_vocals;

    public enum Samurai_state_e
    {
        WAITING,
        LOOKING,
        ATTACKING,
        PAUSING,
        RETREATING
    }

    public GameObject[] retreat_point_markers;

    public GameObject diamond_drop;
    public GameObject diamond_cut_scene;

    public override float attack_power
    {
        get
        {
            return 3;
        }
    }

    public override float max_health
    {
        get
        {
            return 20;
        }
    }

    private GameObject llama;
    private GameObject ninja;
    public Samurai_state_e cur_state;
    public bool is_charging {
        get { return cur_state == Samurai_state_e.ATTACKING; } }

    protected override float invincibility_flash_duration {
        get { return 0.5f; } }

    public static float max_pause_time = 1.25f;
    private float cur_pause_time;

    private float speed;

    private static Samurai_Attack instance;

    //private CharacterController cc;

    private List<Vector3> retreat_points = new List<Vector3>();
    private Vector3 retreat_destination;
    private Vector3 retreat_start;
    private float retreat_duration;
    private float retreat_time_elapsed;

    private Vector3 starting_location;
    private Quaternion starting_rotation;


    //--------------------------------------------------------------------------

    public static Samurai_Attack get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    void Awake()
    {
        instance = this;
    }

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start ()
    {
        base.Start();

        death_vocals.transform.parent = transform.parent;
        //cc = gameObject.GetComponent<CharacterController>();

        foreach (var obj in retreat_point_markers)
        {
            retreat_points.Add(obj.transform.position);
        }

        speed = Llama.get().charge_speed;
        cur_state = Samurai_state_e.WAITING;
        cur_pause_time = 0f;

        llama = Llama.get().gameObject;
        ninja = Ninja.get().gameObject;

        starting_rotation = body.transform.rotation;
        starting_location = transform.position;

        // snap_to_ground();
    }

    // Update is called once per frame
    protected override void update_impl()
    {
        base.update_impl();
        // if (being_knocked_back)
        // {
        //     return;
        // }

        // var closest_player = look_for_player();


        switch(cur_state)
        {
        case Samurai_state_e.WAITING:
            break;

        case Samurai_state_e.LOOKING:
            look_toward(look_for_player(), 10f);

            cur_pause_time += Time.fixedDeltaTime;
            if(cur_pause_time >= max_pause_time)
            {
                print("looking to attacking");
                cur_pause_time = 0;
                cur_state = Samurai_state_e.ATTACKING;

                // velocity = body.transform.forward * speed;
                // print("attack!");
            }

            // snap_to_ground();
            break;

        case Samurai_state_e.ATTACKING:
            // print("attack");
            // velocity = body.transform.forward * speed;
            var delta_pos = body.transform.forward * speed;
            delta_pos *= Time.fixedDeltaTime;

            move(delta_pos);
            break;

        case Samurai_state_e.PAUSING:
            cur_pause_time += Time.fixedDeltaTime;
            if(cur_pause_time >= max_pause_time)
            {
                cur_pause_time = 0;
                cur_state = Samurai_state_e.LOOKING;
            }
            break;

        case Samurai_state_e.RETREATING:
            look_toward(retreat_destination, 90f);

            var lerp_percent = retreat_time_elapsed / retreat_duration;
            retreat_time_elapsed += Time.deltaTime;
            if (lerp_percent >= 1)
            {
                lerp_percent = 1;
                cur_state = Samurai_state_e.LOOKING;
                retreat_time_elapsed = 0;
            }

            var new_pos = Vector3.Lerp(
                retreat_start, retreat_destination, lerp_percent);
            var step = new_pos - transform.position;
            print("retreat_start: " + retreat_start);
            print("retreat_destination: " + retreat_destination);
            print("step: " + step);
            move(step);

            break;

        default:
            print("Oh no!");
            break;
        }
    }

    public override Sweep_test_summary move(
        Vector3 delta_position, float precision_pad=0.2f)
    {
        var move_summary = base.move(delta_position, precision_pad);

        if (move_summary.hit_x)
        {
            print ("hit_x");
            // stop();
            var pc = move_summary.hit_info_x.transform.GetComponent<Player_character>();
            if (pc == null)
            {
                cur_state = Samurai_state_e.LOOKING;
            }
            else
            {
                resolve_collision_with_player(pc);
            }
        }
        else if (move_summary.hit_z)
        {
            // stop();
            var pc = move_summary.hit_info_z.transform.GetComponent<Player_character>();
            if (pc == null)
            {
                cur_state = Samurai_state_e.LOOKING;
            }
            else
            {
                resolve_collision_with_player(pc);
            }
        }

        return move_summary;
    }

    //--------------------------------------------------------------------------

    public void notify_players_in_arena()
    {
        if (cur_state == Samurai_state_e.WAITING)
        {
            cur_state = Samurai_state_e.LOOKING;
        }
    }// notify_players_in_arena

    //--------------------------------------------------------------------------


    public override void OnTriggerEnter(Collider c)
    {
        var player = c.gameObject.GetComponent<Player_character>();
        if (player == null)
        {
            return;
        }

        // stop();
        resolve_collision_with_player(player);
    }// OnTriggerEnter

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }

//  void OnControllerColliderHit(ControllerColliderHit c)
//  {
//      var player = c.gameObject.GetComponent<Player_character>();
//      if (player != null)
//      {
//          resolve_collision_with_player(player);
//          return;
//      }
//
//      //TODO
//      //bool hit_wall = (cc.collisionFlags & CollisionFlags.Sides) != 0;
//      if (hit_wall)
//      {
//          cur_state = Samurai_state_e.LOOKING;
//          // print("hit wall");
//          return;
//      }
//  }

    //--------------------------------------------------------------------------

    void resolve_collision_with_player(Player_character pc)
    {
        print("resolve_collision_with_player");
        // print(cur_state);
        var knockback_direction = pc.transform.position - transform.position;
        pc.receive_hit(
            attack_power, knockback_direction * attack_power, gameObject);

        if (cur_state == Samurai_state_e.RETREATING)
        {
            return;
        }
        choose_retreat_point();
    }// resolve_collision_with_player

    //--------------------------------------------------------------------------

    public void choose_retreat_point()
    {
        for (int i = 0; i < 10; ++i)
        {
            var index = Random.Range(0, retreat_points.Count);
            // print(index);
            retreat_destination = retreat_points[index];

            var something_in_way = Physics.Raycast(
                transform.position, retreat_destination,
                Vector3.Distance(transform.position, retreat_destination));

            if (!something_in_way)
            {
                print("retreat_destination: " + retreat_destination);
                break;
            }

        }

        retreat_destination.y = transform.position.y;
        retreat_duration = Vector3.Distance(
            retreat_destination, transform.position) / speed;
        retreat_time_elapsed = 0;
        retreat_start = transform.position;

        cur_state = Samurai_state_e.RETREATING;
    }// choose_retreat_point

    //--------------------------------------------------------------------------

    // returns the position of the nearest player to the enemy
    private GameObject look_for_player()
    {
        Vector3 llama_pos = llama.transform.position;
        Vector3 ninja_pos = ninja.transform.position;

        float llama_dist = Vector3.Distance(llama_pos, transform.position);
        float ninja_dist = Vector3.Distance(ninja_pos, transform.position);

        return (llama_dist < ninja_dist) ? llama : ninja;
    }

    //--------------------------------------------------------------------------

    public override void play_damage_vocals()
    {
        damage_vocals.GetComponent<Sound_effect_randomizer>().play();
    }

    public override void on_death(GameObject killer)
    {
        print("da boss mans is ded");
        Camera.main.GetComponent<Camera_follow>().deactivate_boss_mode();
        Player_character.force_team_up = false;

        death_vocals.GetComponent<Sound_effect_randomizer>().play();
        base.on_death(killer);
        diamond_cut_scene.GetComponent<Cut_scene>().activate();
    }

    protected override void drop_item()
    {
        var diamond_pos = transform.position;
        diamond_pos.y += 3;
        diamond_drop.transform.position = diamond_pos;
        diamond_drop.SetActive(true);
    }

    // -------------------------------------------------------------------------

    public void notify_checkpoint_load()
    {
        reset_health();
        transform.position = starting_location;
        body.transform.rotation = starting_rotation;

        cur_state = Samurai_state_e.WAITING;
    }
}
