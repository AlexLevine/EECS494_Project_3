using UnityEngine;
using System.Collections;

public class Ninja : Player_character
{
    public GameObject team_up_point;

    public GameObject projectile_prefab;
    public GameObject jousting_pole;
    public GameObject sword_obj;
    public Material out_of_range;
    private Material normal;
    private float o_o_r=2;

    //--------------------------------------------------------------------------

    private static Ninja instance;

    //--------------------------------------------------------------------------

    public static Ninja get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public override void Awake()
    {
        base.Awake();

        instance = this;
    }// Awake
    // // Use this for initialization
    // public override void Start()
    // {
    //     base.Start();
    // }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!is_teamed_up)
        {
            jousting_pole.SetActive(false);
            sword_obj.SetActive(true);

            //out_of_range
            // print(o_o_r);
            if (o_o_r++==1){
                GetComponent<Renderer>().material = normal;
            }

            return;
        }

        // on_ground = true;
        // velocity.y = 0;
        transform.position = team_up_point.transform.position;

        if (!Llama.get().gameObject.GetComponent<Throw_animation>().is_playing)
        {
            transform.rotation = team_up_point.transform.parent.rotation;
        }
    }// Update

    //--------------------------------------------------------------------------

    public override void attack()
    {
        // if(is_teamed_up)
        // {
        //     toggle_jousting_pole();
        // }
        // else
        // {
            GetComponent<Sword_swing>().swing();
        // }
    }// physical_attack

    //--------------------------------------------------------------------------

    public override void adjust_jousting_pole(
        float vertical_tilt, float horizontal_tilt)
    {
        if(jousting_pole == null)
        {
            print("Ninja_Jousting_Pole does not exist for some reason");
            return;
        }

        if(!jousting_pole.activeSelf)
        {
            return;
        }

        if(!is_teamed_up)
        {
            return;
        }

        float adjusted_vert = vertical_tilt * -10;   // some float from -1 to 1,
        float adjusted_horz = horizontal_tilt * 45; // max angle is 45 degrees
        // Adjust the tilt that the jousting pole is pointing

        jousting_pole.transform.position = transform.position;
        jousting_pole.transform.rotation = transform.rotation;

        jousting_pole.transform.RotateAround(
            transform.position, transform.up, adjusted_horz);
        jousting_pole.transform.RotateAround(
            transform.position, transform.right, adjusted_vert);

    }// adjust_jousting_pole

    //--------------------------------------------------------------------------

    private void toggle_jousting_pole()
    {
        if(!is_teamed_up)
        {
            return;
        }

        if(!jousting_pole.activeSelf)
        {
            sword_obj.SetActive(false);
            jousting_pole.SetActive(true);
            return;
        }

        if (!is_teamed_up)
        {
            sword_obj.SetActive(true);
        }

        jousting_pole.SetActive(false);


    }// toggle_jousting_pole

    //--------------------------------------------------------------------------

    public override void move(Vector3 delta_position, bool apply_rotation)
    {
        // if (GetComponent<Sword_swing>().is_swinging || is_teamed_up)
        if (is_teamed_up)

        {
            return;
        }

        // if (is_teamed_up)
        // {
        //     // stop();
        //     // delta_position.y = 0;
        //     return;
        // }

        base.move(delta_position, apply_rotation);
    }// move

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (is_teamed_up)
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

        var blocked = hit && hit_info.collider.gameObject.tag != "Player";

        if (distance > 10f || blocked)
        {
            print("out of range");
            normal = GetComponent<Renderer>().material;
            GetComponent<Renderer>().material = out_of_range;
            o_o_r = 0;
            return;
        }

        // print("teaming up");
        team_up_engage();
        // transform.position = team_up_point.transform.position;
        sword_obj.SetActive(false);
        jousting_pole.SetActive(true);

    }// team_up_engage_or_throw

    public override void collision_safe_rotate_towards(
        Vector3 direction, float step)
    {
        if (!GetComponent<Sword_swing>().is_swinging)
        {
            base.collision_safe_rotate_towards(direction, step);
        }
    }

    //--------------------------------------------------------------------------

    // public void on_thrown()
    // {
    //     on_ground = false;
    // }// on_thrown

    //--------------------------------------------------------------------------

    protected override void on_team_up_engage()
    {
        cc.enabled = false;
    }// team_up_engage

    //--------------------------------------------------------------------------

    protected override void on_team_up_disengage()
    {
        cc.enabled = true;
    }// team_up_disengage

    //--------------------------------------------------------------------------

    public override void jump()
    {
        base.jump();

        if (is_teamed_up)
        {
            team_up_disengage();
        }
    }// jump


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

    // //--------------------------------------------------------------------------

    // public override float jump_speed
    // {
    //     get
    //     {
    //         return 15;
    //     }
    // }// jump_speed
}


