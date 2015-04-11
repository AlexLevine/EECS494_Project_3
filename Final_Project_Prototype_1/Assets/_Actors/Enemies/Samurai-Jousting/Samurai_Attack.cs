using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Samurai_Attack : Enemy {
    public enum Samurai_state_e
    {
        WAITING,
        ATTACKING,
        RETREATING
    }

    public Samurai_state_e cur_state;

    public override float attack_power { get { return 1; } }
    public override float max_health { get { return 50; } }

    public bool is_charging {
        get { return cur_state == Samurai_state_e.ATTACKING; } }

    private float speed;

    private static Samurai_Attack instance;

    private CharacterController cc;

    private Vector3 retreat_destination;
    private Vector3 starting_location;
    private Quaternion starting_rotation;

    //--------------------------------------------------------------------------

    public static Samurai_Attack get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public override void Awake()
    {
        base.Awake();
        instance = this;
    }

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start ()
    {
        base.Start();

        cc = gameObject.GetComponent<CharacterController>();

        speed = Llama.get().charge_speed;
        cur_state = Samurai_state_e.WAITING;
        cur_pause_time = 0f;

        starting_rotation = transform.rotation;
        starting_location = transform.position;

        snap_to_ground();
    }// start

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        // base.Update();
        if (being_knocked_back)
        {
            return;
        }

        var closest_player = look_for_player();


        switch(cur_state){
            case Samurai_state_e.WAITING:
                snap_to_ground();
                break;

            case Samurai_state_e.ATTACKING:
                look_toward(Llama.get().gameObject);
                var delta_pos = transform.forward * speed * Time.deltaTime;
                move(delta_pos, false);
                break;

            case Samurai_state_e.RETREATING:
                // look_toward(retreat_destination);
                // move(transform.forward * speed * 0.65f * Time.deltaTime,
                //      false);

                // var distance_to_dest = Vector3.Distance(
                //     transform.position, retreat_destination);
                // // print(distance_to_dest);
                // if (distance_to_dest < 3f)
                // {
                //     cur_state = Samurai_state_e.LOOKING;
                // }

                break;

            default:
                print("Oh no!");
                break;
        }

        fix_rotation();
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

        resolve_collision_with_player(player);
    }// OnTriggerEnter

    void OnControllerColliderHit(ControllerColliderHit c)
    {
        var player = c.gameObject.GetComponent<Player_character>();
        if (player != null)
        {
            resolve_collision_with_player(player);
            return;
        }
    }// OnControllerColliderHit

    //--------------------------------------------------------------------------

    void resolve_collision_with_player(Player_character pc)
    {
        // print(cur_state);
        pc.receive_hit(attack_power, transform.forward * attack_power, gameObject);

        if (cur_state == Samurai_state_e.RETREATING)
        {
            return;
        }

        cur_state = Samurai_state_e.RETREATING;

        choose_retreat_point();
    }// resolve_collision_with_player

    //--------------------------------------------------------------------------

    void choose_retreat_point()
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
                return;
            }

        }
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

    void snap_to_ground()
    {
        cc.Move(Vector3.down);
    }// snap_to_ground

    void fix_rotation()
    {
        var fixed_rotation = transform.rotation.eulerAngles;

        fixed_rotation.x = 0;
        fixed_rotation.z = 0;

        transform.rotation = Quaternion.Euler(fixed_rotation);
    }// fix_rotation

    // -------------------------------------------------------------------------

    public override bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker)
    {
        if (!attacker.name.Contains(Ninja_sword.global_name))
        {
            return false;
        }

        return base.receive_hit(damage, knockback_velocity, attacker);
    }// receive_hit

    //--------------------------------------------------------------------------

    public void notify_checkpoint_load()
    {
        reset_health();
        transform.position = starting_location;
        transform.rotation = starting_rotation;

        cur_state = Samurai_state_e.WAITING;
    }// notify_checkpoint_load
}
