using UnityEngine;
using System.Collections;

public class Llama : Player_character
{
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
        new_ninja_velocity.y *= 15;
        // new_ninja_velocity.z *= 5;

        ninja.rigidbody.velocity = new_ninja_velocity;

    }// throw_ninja

    //--------------------------------------------------------------------------

    public override void elemental_attack()
    {
        print("thermite!");
        var flame_thrower = transform.Find("fire_breath");

        flame_thrower.gameObject.SetActive(true);
    }// elemental_attack

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

