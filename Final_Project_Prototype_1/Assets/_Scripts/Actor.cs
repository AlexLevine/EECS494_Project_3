using UnityEngine;
using System.Collections;
using System;

public class Actor : MonoBehaviour
{
    private int health;

    //--------------------------------------------------------------------------

    public virtual void Start()
    {
        health = max_health;
    }// Start()

    //--------------------------------------------------------------------------

    public virtual void Update()
    {

    }// Update()

    //--------------------------------------------------------------------------

    public virtual void receive_hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            on_death();
        }
    }// receive_hit

    //--------------------------------------------------------------------------

    public virtual void on_death()
    {

    }// on_death

    //--------------------------------------------------------------------------

    public virtual int max_health
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// max_health
}
