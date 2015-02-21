using UnityEngine;
using System.Collections;
using System;

public class Player_character : Actor
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

        get_input();
    }// Update

    //--------------------------------------------------------------------------

    protected override void on_death()
    {
        print("you die!");
    }// on_death

    //--------------------------------------------------------------------------

    protected virtual void get_input()
    {
        throw new Exception("Derived classes must override this method");
    }// get_input

    //--------------------------------------------------------------------------

    protected virtual void elemental_attack()
    {
        throw new Exception("Derived classes must override this method");
    }// elemental_attack

    //--------------------------------------------------------------------------

    protected void run()
    {

    }// run

    //--------------------------------------------------------------------------

    protected virtual int run_speed
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// run_speed

    //--------------------------------------------------------------------------

    protected void sprint()
    {

    }// sprint

    //--------------------------------------------------------------------------

    protected virtual int sprint_speed
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// sprint_speed

    //--------------------------------------------------------------------------

    protected void jump()
    {

    }// jump

    //--------------------------------------------------------------------------

    protected virtual int jump_speed
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// jump_speed
}
