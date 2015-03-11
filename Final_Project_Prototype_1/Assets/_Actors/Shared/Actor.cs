using UnityEngine;
using System.Collections;
using System;

public class Actor : MonoBehaviour
{
    public int health;

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

    public void look_toward(GameObject obj, float step=360f)
    {
        var target_direction =
                obj.transform.position - transform.position;

        var new_forward = Vector3.RotateTowards(
            transform.forward, target_direction, step, 0f);
        transform.rotation = Quaternion.LookRotation(new_forward);
    }// look_toward

    //--------------------------------------------------------------------------

    public virtual void receive_hit(int damage, GameObject attacker=null)
    {
        health -= damage;
        // GetComponent<Flash_animation>().start_animation();

        if (health <= 0)
        {
            on_death();
        }
    }// receive_hit

    //--------------------------------------------------------------------------

    public virtual void on_death()
    {

    }// on_death

    public virtual void special_ability()
    {

    }

    //--------------------------------------------------------------------------

    public virtual int max_health
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// max_health
}
