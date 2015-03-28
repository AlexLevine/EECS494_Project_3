using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Enemy : Actor
{
    public static List<GameObject> enemies = new List<GameObject>();

    public virtual void Awake()
    {
        enemies.Add(gameObject);
    }

    //--------------------------------------------------------------------------

    // // Use this for initialization
    // public override void Start()
    // {
    //     base.Start();
    // }// Start

    //--------------------------------------------------------------------------

    // // Update is called once per frame
    // public override void Update ()
    // {
    //     base.Update();
    // }// Update

    //--------------------------------------------------------------------------

    public static List<GameObject> get_potential_lock_on_targets(
        GameObject player)
    {
        return enemies.FindAll(
            (GameObject obj) => Vector3.Angle(
                player.transform.position, obj.transform.position) <= 90f);
    }// get_potential_lock_on_targets

    //--------------------------------------------------------------------------

    public static GameObject get_closest_potential_lock_on_target(
        GameObject player)
    {
        var potential_targets = get_potential_lock_on_targets(player);

        if (potential_targets.Count == 0)
        {
            return null;
        }

        // I am currently very angry at C#'s apparent lack of a min() function
        // that does this.
        GameObject closest_target = potential_targets[0];
        var closest_distance = Vector3.Distance(
            player.transform.position, closest_target.transform.position);
        foreach (var obj in potential_targets)
        {
            var distance = Vector3.Distance(
                player.transform.position, obj.transform.position);
            if (distance < closest_distance)
            {
                closest_target = obj;
                closest_distance = distance;
            }
        }

        return closest_target;
        // return potential_targets.MinBy(
        //     (GameObject obj) => Vector3.Distance(
        //         transform.position, obj.transform.position));
    }// get_closest_potential_lock_on_target

    //--------------------------------------------------------------------------

    public virtual void on_player_hit(){}

    //--------------------------------------------------------------------------

    public virtual void OnTriggerEnter(Collider c)
    {
        var player = c.gameObject.GetComponent<Player_character>();
        if (player == null || being_knocked_back)
        {
            return;
        }
        // var parent = c.gameObject.transform.parent;
        // (parent != null ? parent.gameObject :
        //     c.gameObject).GetComponent<Actor>().receive_hit(attack_power);

        // var actor = c.gameObject.GetComponent<Actor>();
        // // print(actor);
        // if (actor == null)
        // {
        //     return;
        // }

        var knockback_direction = (
            c.gameObject.transform.position - transform.position).normalized;
        var knockback_velocity = knockback_direction * 5;

        player.receive_hit(attack_power, knockback_velocity, gameObject);

        on_player_hit();
    }// OnTriggerEnter

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }

    // void OnCollisionEnter(Collision c)
    // {
    //     OnTriggerEnter(c.collider);
    // }

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
        GameObject boom = (GameObject)Instantiate(Resources.Load("dead_enemy"));
        boom.transform.position = transform.position;
        Destroy(gameObject);
    }// on_death


    //--------------------------------------------------------------------------

    void OnDestroy()
    {
        enemies.Remove(gameObject);
        Player_character.notify_enemy_gone(gameObject);
    }// OnDestroy
}
