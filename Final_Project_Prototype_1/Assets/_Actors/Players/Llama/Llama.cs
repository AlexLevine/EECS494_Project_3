using UnityEngine;
using System.Collections;
using InControl;


public class Llama : Player_character
{
    public GameObject spit_prefab;

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }// Update

    //--------------------------------------------------------------------------

    public override void jump()
    {
        if(!on_ground)
        {
            return;
        }

        base.jump();
    }

    // public override void special_ability()
    public void special_ability()
    { // Tackle / Sprint
    }

    //--------------------------------------------------------------------------

    public override void throw_ninja()
    {
        if (!teamed_up)
        {
            return;
        }

        var ninja = GameObject.Find("Ninja");

        teamed_up = false;
        ninja.GetComponent<Player_character>().teamed_up = false;

        var new_ninja_velocity = Vector3.one;// transform.forward;
        // new_ninja_velocity.x *= 5;
        new_ninja_velocity.y *= 10;
        // new_ninja_velocity.z *= 5;

        ninja.GetComponent<Rigidbody>().velocity = new_ninja_velocity;

    }// throw_ninja

    //--------------------------------------------------------------------------

    public override void projectile_attack()
    {
        var projectile_start_pos = transform.position;
        projectile_start_pos += transform.forward * 1f;
        projectile_start_pos.y += 1f;

        GameObject spit = Instantiate(
            spit_prefab, projectile_start_pos,
            transform.rotation) as GameObject;
        spit.GetComponent<Rigidbody>().velocity = transform.forward * 12;
    }// projectile_attack

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
            return 7;
        }
    }// jump_speed
}

