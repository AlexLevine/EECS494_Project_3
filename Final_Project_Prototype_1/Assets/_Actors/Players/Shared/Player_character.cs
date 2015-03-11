using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player_character : Actor
{
    public bool on_ground;
    public bool teamed_up = false;

    public GameObject lock_on_target = null;

    public static List<GameObject> players = new List<GameObject>();

    void Awake()
    {
        players.Add(gameObject);
    }
    // Use this for initialization
    // public override void Start()
    // {
    //     base.Start();
    // }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (is_locked_on)
        {
            look_toward(lock_on_target);
            return;
        }
    }// Update

    //--------------------------------------------------------------------------

    public override void on_death()
    {
        print("you die!");
    }// on_death

    //--------------------------------------------------------------------------

    public virtual void projectile_attack()
    {
        throw new Exception("Derived classes must override this method");
    }// projectile_attack

    //--------------------------------------------------------------------------

    public virtual void physical_attack()
    {
        throw new Exception("Derived classes must override this method");
    }// physical_attack

    //--------------------------------------------------------------------------

    public virtual void toggle_lock_on()
    {
        // if (teamed_up)
        // {
        //     return;
        // }

        if (is_locked_on)
        {
            lock_on_target = null;
            return;
        }

        lock_on_target = Enemy.get_closest_potential_lock_on_target(gameObject);

        look_toward(lock_on_target, 360f);
    }// toggle_lock_on

    //--------------------------------------------------------------------------

    public bool is_locked_on
    {
        get
        {
            return lock_on_target != null;
        }
    }

    //--------------------------------------------------------------------------

    public void notify_enemy_killed(GameObject enemy)
    {
        if (enemy == lock_on_target)
        {
            lock_on_target = null;
        }
    }// notify_enemy_killed

    //--------------------------------------------------------------------------

    public virtual void toggle_jousting_pole()
    {
        throw new Exception("Derived classes must override this method");

    }

    //--------------------------------------------------------------------------

    public virtual void adjust_jousting_pole(float vertical_tilt, float horizontal_tilt)
    {
        throw new Exception("Derived classes must override this method");
    }// team_up_attack

    //--------------------------------------------------------------------------

    public virtual void run(Vector3 tilt, bool sprint)
    {
        // if (hit_edge_of_screen(GameObject.Find("Llama")) ||
        //     hit_edge_of_screen(GameObject.Find("Ninja")))
        // {
        //     return;
        // }

        if (!on_ground && !teamed_up)
        {
            return;
        }

        var speed = run_speed;

        if(sprint)
        {
            speed = sprint_speed;
        }


        float horiz_speed = (float)(speed * tilt.x);
        float z_speed = (float)(speed * tilt.z);

        Vector3 new_speed = GetComponent<Rigidbody>().velocity;

        new_speed.x = horiz_speed;
        new_speed.z = z_speed;

        GetComponent<Rigidbody>().velocity = new_speed;

        if(tilt == Vector3.zero)
        {
            return;
        }

        if (is_locked_on)
        {
            look_toward(lock_on_target);
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

        Vector3 new_speed = GetComponent<Rigidbody>().velocity;
        new_speed.y = jump_speed;
        GetComponent<Rigidbody>().velocity = new_speed;

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

    // public virtual void throw_ninja()
    // {

    // }// throw_ninja

    //--------------------------------------------------------------------------

    public virtual void team_up_engage_or_throw()
    {

    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag.Contains("floor"))
        {
            on_ground = true;
        }
    }


    void OnCollisionStay(Collision collision){
        if(collision.gameObject.tag.Contains("floor"))
        {
            on_ground = true;
        }
    }


    void OnCollisionExit(Collision collision){
        if(collision.gameObject.tag.Contains("floor"))
        {
            on_ground = false;
        }
    }

    // void OnTriggerStay(Collider other){
    //     if(other.gameObject.tag.Contains("floor"))
    //     {
    //         print("On Ground");
    //         on_ground = true;
    //     }
    // }

    // void OnTriggerExit(Collider other){
    //     if(other.gameObject.tag.Contains("floor"))
    //     {
    //         print("Leaving Ground");
    //         on_ground = false;
    //     }
    // }
}
