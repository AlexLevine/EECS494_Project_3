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
        // if (hit_edge_of_screen(GameObject.Find("Llama")) ||
        //     hit_edge_of_screen(GameObject.Find("Ninja")))
        // {
        //     return;
        // }

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

    // public static bool hit_edge_of_screen(GameObject obj)
    // {
    //     var bottom_left = Camera.main.ViewportToWorldPoint(Vector3.zero);
    //     print(bottom_left);
    //     var top_right = Camera.main.ViewportToWorldPoint(Vector3.one);
    //     print(top_right);

    //     var obj_off_left = obj.transform.position.x <= bottom_left.x;
    //     var obj_off_right = obj.transform.position.x >= top_right.x;
    //     var obj_off_front = obj.transform.position.z <= bottom_left.z;
    //     var obj_off_back = obj.transform.position.z >= top_right.z;

    //     return obj_off_left || obj_off_right || obj_off_front || obj_off_back;
    // }// hit_edge_of_screen

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
        if(collision.gameObject.tag.Contains("floor"))
        {
            print("On Ground");
            on_ground = true;
        }
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.tag.Contains("floor"))
        {
            print("Leaving Ground");
            on_ground = false;
        }
    }
}
