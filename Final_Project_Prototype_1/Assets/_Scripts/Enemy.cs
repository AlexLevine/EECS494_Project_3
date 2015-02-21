using UnityEngine;
using System.Collections;
using System;

public class Enemy : Actor
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }// Update

    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Actor>().receive_hit(attack_power);
        }
    }// OnCollisionEnter

    //--------------------------------------------------------------------------

    protected virtual int attack_power
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// attack_power
}
