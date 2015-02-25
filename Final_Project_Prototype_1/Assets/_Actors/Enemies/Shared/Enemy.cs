using UnityEngine;
using System.Collections;
using System;

public class Enemy : Actor
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
    }// Update

    public virtual void on_player_hit(){}

    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Actor>().receive_hit(attack_power);
            on_player_hit();
        }
    }// OnCollisionEnter

    //--------------------------------------------------------------------------

    public virtual int attack_power
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// attack_power


    public override void on_death()
    {
        Destroy(gameObject);
    }// on_death
}
