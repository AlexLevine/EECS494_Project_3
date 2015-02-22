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

    void pick_up_or_throw_ninja()
    {

    }// pick_up_or_throw_ninja

    //--------------------------------------------------------------------------

    public override void elemental_attack()
    {
        print("thermite!");
        var flame_thrower = transform.Find("fire_breath");

        flame_thrower.gameObject.active = true;
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

