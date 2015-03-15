using UnityEngine;
using System.Collections;

public class Ninja : Player_character
{

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

    void Awake()
    {
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
			//print(o_o_r);
			if (o_o_r++==1){
				GetComponent<Renderer>().material = normal;
			}

            return;
        }

        var llama = GameObject.Find("Llama_Torso");

        var new_ninja_pos = GetComponent<Collider>().bounds.center;

        var y_offset =
                GetComponent<Collider>().bounds.extents.y + llama.GetComponent<Collider>().bounds.extents.y;
        new_ninja_pos.y = llama.GetComponent<Collider>().bounds.center.y + y_offset;
        new_ninja_pos.z = llama.GetComponent<Collider>().bounds.center.z;
        new_ninja_pos.x = llama.GetComponent<Collider>().bounds.center.x;

        transform.position = new_ninja_pos;
    }// Update

    //--------------------------------------------------------------------------

    // public override void projectile_attack()
    // {
    //     GameObject projectile = Instantiate(
    //         projectile_prefab, transform.position,
    //         transform.rotation) as GameObject;
    //     projectile.GetComponent<Rigidbody>().velocity = transform.forward * 12;


    // }// projectile_attack

    //--------------------------------------------------------------------------

    public override void attack()
    {
        if(is_teamed_up)
        {
            toggle_jousting_pole();
        }
        else{
            GetComponent<Sword_swing>().swing();
        }
    }// physical_attack

    //--------------------------------------------------------------------------

    public override void adjust_jousting_pole(float vertical_tilt, float horizontal_tilt)
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

        float adjusted_vert = vertical_tilt * 10;   // some float from -1 to 1,
        float adjusted_horz = horizontal_tilt * -45; // max angle is 45 degrees
        // Adjust the tilt that the jousting pole is pointing

        // print(adjusted_horz);
        // print(adjusted_vert);

        // jousting_pole.transform.position = jousting_pole_start_pos;
        jousting_pole.transform.position = this.transform.position;
        // jousting_pole.transform.rotation = jousting_pole_start_rot;
        jousting_pole.transform.rotation = this.transform.rotation;


        jousting_pole.transform.RotateAround(transform.position, transform.up, adjusted_horz);
        jousting_pole.transform.RotateAround(transform.position, transform.right, adjusted_vert);

    }

    public override void toggle_jousting_pole()
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


    }

    //--------------------------------------------------------------------------

    public override void move(Vector3 delta_position)
    {
        if (GetComponent<Sword_swing>().is_swinging)
        {
            return;
        }

        base.move(delta_position);
    }

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (is_teamed_up)
        {
            return;
        }

        GameObject llama_go = Llama.get().gameObject;
        var distance = Vector3.Distance(
            transform.position, llama_go.transform.position);

        if (distance > 4f)
        {
			normal = GetComponent<Renderer>().material;
           	GetComponent<Renderer>().material = out_of_range;
           	o_o_r = 0;
            return;
        }

        team_up_engage();
        sword_obj.SetActive(false);

    }// team_up_engage_or_throw

    public override void jump()
    {
        base.jump();

        if (!is_teamed_up)
        {
            return;
        }

        team_up_disengage();
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


