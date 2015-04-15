using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Samurai_Attack : Enemy, Checkpoint_load_subscriber
{
    public enum Samurai_state_e
    {
        WAITING,
        ATTACKING,
        RETREATING
    }

    public Samurai_state_e cur_state;

    public override float attack_power { get { return 2f; } }
    public override float max_health { get { return 25; } }
    public bool ready_to_charge {
        get { return cur_state == Samurai_state_e.WAITING; } }

    public bool is_charging {
        get { return cur_state == Samurai_state_e.ATTACKING; } }

    private float speed;

    private static Samurai_Attack instance;

    // private CharacterController cc;

    public Vector3 retreat_destination;
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

        Checkpoint.subscribe(this);

        // cc = gameObject.GetComponent<CharacterController>();

        speed = Llama.get().charge_speed;
        cur_state = Samurai_state_e.WAITING;

        starting_rotation = transform.rotation;
        starting_location = transform.position;

        snap_to_ground();
    }// start

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        if (being_knocked_back)
        {
            return;
        }

        switch(cur_state){
            case Samurai_state_e.WAITING:
                look_toward(Llama.get().gameObject);
                snap_to_ground();
                break;

            case Samurai_state_e.ATTACKING:
                var direction_to_player =
                    Llama.get().transform.position - transform.position;
                var angle_to_player = Vector3.Angle(
                    transform.forward, direction_to_player);

                // if (angle_to_player > 90)
                // {
                //     look_toward(retreat_destination, 360f);
                // }

                // print(retreat_destination);
                var distance_to_retreat_point = Vector3.Distance(
                    transform.position, retreat_destination);
                // print(distance_to_retreat_point);
                if (angle_to_player > 120f || distance_to_retreat_point <= 10f)
                {
                    Boss_fight_controller.get().notify_fighters_passed();
                    break;
                }

                var distance_to_player = Vector3.Distance(
                    transform.position, Llama.get().transform.position);

                if (distance_to_player >= 10f)
                {
                    // print(angle_to_player);
                    look_toward(Llama.get().gameObject, 360f);
                }

                var delta_pos = transform.forward * speed * Time.deltaTime;
                delta_pos.y = -0.05f;
                move(delta_pos, false);
                break;

            case Samurai_state_e.RETREATING:
                look_toward(retreat_destination);
                var pos_step = speed * 0.85f * Time.deltaTime;

                var cur_pos = transform.position;
                cur_pos.y = 0;

                var desired_position = Vector3.MoveTowards(
                    cur_pos, retreat_destination, pos_step);
                var adjusted_step = desired_position - cur_pos;
                move(adjusted_step, false);

                cur_pos = transform.position;
                cur_pos.y = 0;
                var reached_retreat_point = Vector3.Distance(
                    cur_pos, retreat_destination) < 1f;
                if (reached_retreat_point)
                {
                    cur_state = Samurai_state_e.WAITING;
                }

                break;
        }

        fix_rotation();
    }// update_impl

    //--------------------------------------------------------------------------

    public void start_charge()
    {
        cur_state = Samurai_state_e.ATTACKING;
    }// start_charge

    //--------------------------------------------------------------------------

    public void retreat(Vector3 destination)
    {
        snap_to_ground();
        destination.y = 0;
        retreat_destination = destination;
        cur_state = Samurai_state_e.RETREATING;
    }// retreat

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
        if (cur_state == Samurai_state_e.RETREATING)
        {
            return;
        }

        pc.receive_hit(attack_power, transform.forward * attack_power, gameObject);
        Boss_fight_controller.get().notify_fighters_passed();
    }// resolve_collision_with_player

    //--------------------------------------------------------------------------

    void snap_to_ground()
    {
        // cc.Move(Vector3.down);
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

        Boss_fight_controller.get().notify_fighters_passed();
        return base.receive_hit(damage, knockback_velocity, attacker);
    }// receive_hit

    //--------------------------------------------------------------------------

    public override void on_death()
    {
        Checkpoint.unsubscribe(gameObject);
        Boss_fight_controller.get().notify_boss_defeated();
        base.on_death();
    }// on_death

    //--------------------------------------------------------------------------

    public void notify_checkpoint_load()
    {
        reset_health();
        transform.position = starting_location;
        transform.rotation = starting_rotation;

        cur_state = Samurai_state_e.WAITING;
    }// notify_checkpoint_load
}
