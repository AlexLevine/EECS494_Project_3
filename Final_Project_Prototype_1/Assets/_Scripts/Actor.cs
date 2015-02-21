using UnityEngine;
using System.Collections;
using System;

public class Actor : MonoBehaviour
{
    private int health;

    //--------------------------------------------------------------------------

    protected virtual void Start()
    {
        health = max_health;
    }// Start()

    //--------------------------------------------------------------------------

    protected virtual void Update()
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

    protected virtual void on_death()
    {

    }// on_death

    //--------------------------------------------------------------------------

    protected virtual int max_health
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// max_health
}
