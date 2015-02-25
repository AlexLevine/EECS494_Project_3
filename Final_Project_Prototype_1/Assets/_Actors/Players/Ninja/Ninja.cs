using UnityEngine;
using System.Collections;

public class Ninja : Player_character
{

    public GameObject projectile_prefab;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
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

        var new_ninja_pos = collider.bounds.center;

        var y_offset =
                collider.bounds.extents.y + llama.collider.bounds.extents.y;
        new_ninja_pos.y = llama.collider.bounds.center.y + y_offset;
        new_ninja_pos.z = llama.collider.bounds.center.z;
        new_ninja_pos.x = llama.collider.bounds.center.x;

        transform.position = new_ninja_pos;
    }// Update

    //--------------------------------------------------------------------------

    public override void projectile_attack()
    {
        GameObject projectile = Instantiate(
            projectile_prefab, transform.position,
            transform.rotation) as GameObject;
        projectile.rigidbody.velocity = transform.forward * 12;


    }// projectile_attack

    //--------------------------------------------------------------------------
    public override void team_up_engage()
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
    }// team_up_engage

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


