using UnityEngine;
using System.Collections;

public class Ninja : Player_character
{

    public GameObject ice_projectile_prefab;

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

    public override void elemental_attack()
    {
        print("ice!!");
        GameObject shooty_ice_box = Instantiate(ice_projectile_prefab, transform.position, transform.rotation) as GameObject;
        shooty_ice_box.rigidbody.velocity = transform.forward * 12;


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


