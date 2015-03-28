using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Samurai_Attack : Enemy {
    public enum Samurai_state_e
    {
        WAITING,
        LOOKING,
        ATTACKING,
        PAUSING,
        RETREATING
    }

    public GameObject[] retreat_points;

    public GameObject llama;
    public GameObject ninja;
    public Samurai_state_e cur_state;
    public float look_thresh;
    public float attack_thresh;
    public bool is_charging {
        get { return cur_state == Samurai_state_e.ATTACKING; } }

    public static float max_pause_time = 2f;
    private float cur_pause_time;

    private float speed;

    private static Samurai_Attack instance;

    private CharacterController cc;

    private GameObject retreat_destination;

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

        llama = Llama.get().gameObject;
        ninja = Ninja.get().gameObject;

        if(llama == null || ninja == null)
        {
            print("Could not find llama or ninja!!!!!");
            print("OH NOOOOOOOOOOO");
            Destroy(gameObject);
        }

        snap_to_ground();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // base.Update();
        if (being_knocked_back)
        {
            return;
        }

        var closest_player = look_for_player();
        // float dist_to_closest_player = Vector3.Distance(
        //     closest_player.transform.position, transform.position);

        // var new_rot = transform.rotation.eulerAngles;
        // new_rot.x = 0;
        // new_rot.z = 0;
        // gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(new_rot));

        switch(cur_state){
            case Samurai_state_e.WAITING:
                // if(dist_to_closest_player <= look_thresh)
                // {
                //     cur_state = Samurai_state_e.LOOKING;
                //     return;
                // }
                break;

            case Samurai_state_e.LOOKING:
                // if(dist_to_closest_player > look_thresh)
                // {
                //     print(dist_to_closest_player);
                //     cur_state = Samurai_state_e.WAITING;
                // }
                // else
                // {
                look_toward(closest_player);
                    // if(closest_player == llama.transform.position)
                    // {
                    //     look_toward(llama);
                    // }
                    // else
                    // {
                    //     look_toward(ninja);
                    // }
                // }

                // if(dist_to_closest_player > attack_thresh)
                // {
                //     break;
                // }

                cur_pause_time += Time.fixedDeltaTime;
                if(cur_pause_time >= max_pause_time / 2)
                {
                    cur_pause_time = 0;
                    cur_state = Samurai_state_e.ATTACKING;
                    print("attack!");
                }

                snap_to_ground();
                break;

                // RaycastHit hit_info;
                // var hit = Physics.Raycast(
                //     transform.position, transform.forward, out hit_info);
                // if (!hit)
                // {
                //     break;
                // }

                // if(hit_info.collider.gameObject.tag == "Player")
                // {
                //     cur_state = Samurai_state_e.ATTACKING;
                // }
                // break;

            case Samurai_state_e.ATTACKING:
                var delta_pos = transform.forward * speed;
                // delta_pos.y = -20f;
                delta_pos *= Time.fixedDeltaTime;

                move(delta_pos, true);

                // var new_velocity = transform.forward * speed;
                // new_velocity.y = 0;
                // GetComponent<Rigidbody>().velocity = new_velocity;
                break;

            case Samurai_state_e.PAUSING:
                // GetComponent<Rigidbody>().velocity = Vector3.zero;
                cur_pause_time += Time.fixedDeltaTime;
                if(cur_pause_time >= max_pause_time)
                {
                    cur_pause_time = 0;
                    cur_state = Samurai_state_e.LOOKING;
                }

                break;

            case Samurai_state_e.RETREATING:
                look_toward(retreat_destination);
                move(transform.forward * speed * 0.65f * Time.fixedDeltaTime,
                     false);

                var distance_to_dest = Vector3.Distance(
                    transform.position, retreat_destination.transform.position);
                if (distance_to_dest < 0.2f)
                {
                    cur_state = Samurai_state_e.LOOKING;
                }
                break;
            default:
                print("Oh no!");
                break;
        }
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

    // void OnCollisionEnter(Collision collision)
    // {
    //     if(cur_state != Samurai_state_e.ATTACKING)
    //     {
    //         return;
    //     }
    //     if(collision.gameObject.name.Contains("Arena Wall"))
    //     {
    //         cur_state = Samurai_state_e.PAUSING;
    //     }
    // }

    // void OnCollisionStay(Collision collision)
    // {
    //     OnCollisionEnter(collision);
    // }

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

        bool hit_wall = (cc.collisionFlags & CollisionFlags.Sides) != 0;
        if (hit_wall)
        {
            cur_state = Samurai_state_e.LOOKING;
            print("hit wall");
            return;
        }

        // if (player == null)
        // {
        //     return;
        // }

        // cur_state = Samurai_state_e.LOOKING;
        // player.receive_hit(attack_power, transform.forward * attack_power);
    }

    //--------------------------------------------------------------------------

    void resolve_collision_with_player(Player_character pc)
    {
        print(cur_state);
        pc.receive_hit(attack_power, transform.forward * attack_power);

        if (cur_state == Samurai_state_e.RETREATING)
        {
            return;
        }

        cur_state = Samurai_state_e.RETREATING;
        retreat_destination = retreat_points[(int) Random.Range(
            0, retreat_points.Length)];
    }// resolve_collision_with_player

    //--------------------------------------------------------------------------

    // returns the position of the nearest player to the enemy
    private GameObject look_for_player()
    {
        Vector3 llama_pos = llama.transform.position;
        Vector3 ninja_pos = ninja.transform.position;

        float llama_dist = Vector3.Distance(llama_pos, transform.position);
        float ninja_dist = Vector3.Distance(ninja_pos, transform.position);

        return (llama_dist < ninja_dist) ? llama : ninja;
        // {
        //     return llama_pos;
        // }
        // else
        // {
        //     return ninja_pos;
        // }
    }

    void snap_to_ground()
    {
        cc.Move(Vector3.down);
    }// snap_to_ground

    public override int attack_power
    {
        get
        {
            return 5;
        }
    }

    public override int max_health
    {
        get
        {
            return 20;
        }
    }

    public override void on_hit_spit(int damage, Vector3 knockback_velocity)
    {
        // default behavior
        // receive_hit (damage);
    }// on_hit_spit

    //--------------------------------------------------------------------------

    public override void on_hit_sword(int damage, Vector3 knockback_velocity)
    {
        Ninja.get().receive_hit(0, transform.forward * attack_power);
    }// on_hit_sword
}
