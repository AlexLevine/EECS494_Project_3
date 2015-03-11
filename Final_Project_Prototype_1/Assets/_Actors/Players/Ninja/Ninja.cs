﻿using UnityEngine;
using System.Collections;

public class Ninja : Player_character
{

    public GameObject projectile_prefab;
    public GameObject jousting_pole;

    // private Vector3 jousting_pole_start_pos; 
    // private Quaternion jousting_pole_start_rot; 

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        // jousting_pole_start_pos = jousting_pole.transform.localPosition;
        // jousting_pole_start_rot = jousting_pole.transform.rotation;  
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update()
    {
        if (!teamed_up)
        {
            return;
        }

        base.Update();

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

    public override void projectile_attack()
    {
        GameObject projectile = Instantiate(
            projectile_prefab, transform.position,
            transform.rotation) as GameObject;
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * 12;


    }// projectile_attack

    //--------------------------------------------------------------------------

    public override void physical_attack()
    {
        GetComponent<Sword_swing>().swing();
    }// physical_attack

    //--------------------------------------------------------------------------

    public override void adjust_jousting_pole(float vertical_tilt, float horizontal_tilt)
    {
        if(jousting_pole == null)
        {
            print("Ninja_Jousting_Pole does not exist for some reason");
            return;
        }

        // if(!teamed_up)
        // {
        //     return;
        // }

        float adjusted_vert = vertical_tilt * 45;   // some float from -1 to 1, 
        float adjusted_horz = horizontal_tilt * 45; // max angle is 45 degrees
        // Adjust the tilt that the jousting pole is pointing

        print(adjusted_horz);
        print(adjusted_vert);

        // jousting_pole.transform.position = jousting_pole_start_pos;
        jousting_pole.transform.position = this.transform.position;
        // jousting_pole.transform.rotation = jousting_pole_start_rot;
        jousting_pole.transform.rotation = this.transform.rotation;


        jousting_pole.transform.RotateAround(transform.position, transform.up, adjusted_horz);
        jousting_pole.transform.RotateAround(transform.position, transform.right, adjusted_vert);

    }

    public override void toggle_jousting_pole()
    {
        if(!jousting_pole.activeSelf)
        {
            jousting_pole.SetActive(true);
        }
        else
        {            
            jousting_pole.SetActive(false);
        }

    }

    //--------------------------------------------------------------------------

    public override void run(Vector3 tilt, bool sprint)
    {
        if (GetComponent<Sword_swing>().is_swinging)
        {
            return;
        }

        base.run(tilt, sprint);
    }

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (teamed_up)
        {
            return;
        }

        GameObject llama_go = GameObject.Find("Llama");
        var distance = Vector3.Distance(
            transform.position, llama_go.transform.position);

        if (distance > 2f)
        {
            print("out of range, doofus");
            return;
        }

        teamed_up = true;
        llama_go.GetComponent<Llama>().teamed_up = true;
        // Turn on jousting pole

    }// team_up_engage_or_throw

    public override void jump()
    {
        if(!on_ground && !teamed_up)
        {
            return;
        }

        base.jump();

        if (!teamed_up)
        {
            return;
        }

        teamed_up = false;
        GameObject llama_go = GameObject.Find("Llama");
        llama_go.GetComponent<Llama>().teamed_up = false;

    }// team_up_disengage


    //--------------------------------------------------------------------------


    public override int max_health
    {
        get
        {
            return 100;
        }
    }// max_health

    //--------------------------------------------------------------------------

    public override int run_speed
    {
        get
        {
            return 5;
        }
    }// run_speed

    //--------------------------------------------------------------------------

    public override int sprint_speed
    {
        get
        {
            return 10;
        }
    }// sprint_speed

    //--------------------------------------------------------------------------

    public override int jump_speed
    {
        get
        {
            return 5;
        }
    }// jump_speed
}


