using UnityEngine;
using System.Collections;
using System;

public class Player_character : Actor
{
    public bool on_ground;
    public bool teamed_up = false;

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

    public override void on_death()
    {
        print("you die!");
    }// on_death

    //--------------------------------------------------------------------------

    public virtual void elemental_attack()
    {
        throw new Exception("Derived classes must override this method");
    }// elemental_attack

    //--------------------------------------------------------------------------

    public void run(Vector3 tilt)
    {
        float horiz_speed = run_speed * tilt.x;
        float z_speed = run_speed * tilt.z;

        Vector3 new_speed = rigidbody.velocity;

        new_speed.x = horiz_speed;
        new_speed.z = z_speed;

        rigidbody.velocity = new_speed; 

        if(tilt == Vector3.zero)
        {
            return;
        }

        var turn_to = Mathf.Atan2(tilt.x, tilt.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, turn_to, 0); 

    }// run

    //--------------------------------------------------------------------------

    public virtual int run_speed
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// run_speed

    //--------------------------------------------------------------------------

    public void sprint()
    {

    }// sprint

    //--------------------------------------------------------------------------

    public virtual int sprint_speed
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// sprint_speed

    //--------------------------------------------------------------------------

    public virtual void jump()
    {

        Vector3 new_speed = rigidbody.velocity;
        new_speed.y = jump_speed; 
        rigidbody.velocity = new_speed;

    }// jump

    //--------------------------------------------------------------------------

    public virtual int jump_speed
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// jump_speed

    //--------------------------------------------------------------------------

    public virtual void throw_ninja()
    {
        
    }// throw_ninja

    //--------------------------------------------------------------------------

    public virtual void team_up_engage()
    {
        
    }// team_up_engage

    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.name.Contains("Floor"))
        {
            print("On Ground");
            on_ground = true; 
        }
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.name.Contains("Floor"))
        {
            print("Leaving Ground");
            on_ground = false; 
        }
    }
}
