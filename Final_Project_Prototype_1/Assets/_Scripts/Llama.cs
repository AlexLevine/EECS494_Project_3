using UnityEngine;
using System.Collections;

public class Llama : Player_character
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }// Update

    //--------------------------------------------------------------------------

    protected override void get_input()
    {
        base.get_input();
        print("llama get input");
    }// get_input

    //--------------------------------------------------------------------------

    void pick_up_or_throw_ninja()
    {

    }// pick_up_or_throw_ninja

    //--------------------------------------------------------------------------

    protected override void elemental_attack()
    {
        print("thermite!");
    }// elemental_attack

    //--------------------------------------------------------------------------

    protected override int max_health
    {
        get
        {
            return 100;
        }
    }// max_health

    //--------------------------------------------------------------------------

    protected override int run_speed
    {
        get
        {
            return 5;
        }
    }// run_speed

    //--------------------------------------------------------------------------

    protected override int sprint_speed
    {
        get
        {
            return 10;
        }
    }// sprint_speed

    //--------------------------------------------------------------------------

    protected override int jump_speed
    {
        get
        {
            return 5;
        }
    }// jump_speed
}

