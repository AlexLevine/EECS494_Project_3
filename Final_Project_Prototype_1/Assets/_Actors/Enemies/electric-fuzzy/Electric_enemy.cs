﻿using UnityEngine;
using System.Collections;
using System;

public class Electric_enemy : Point_lerp_enemy
{
    public override float attack_power
    {
        get
        {
            return 1;
        }
    }

    public override float max_health
    {
        get
        {
            return 3;
        }
    }

    //--------------------------------------------------------------------------

    // public override void Start()
    // {
    //     base.Start();
    //     transform.position = path_nodes[destination_index].transform.position;
    //     ++destination_index;
    //     // stunned = false;
    // }// Start

    // //--------------------------------------------------------------------------

    // void FixedUpdate()
    // {
    //     //HACK
    //     if (actors_paused)
    //     {
    //         return;
    //     }

    //     transform.position = Vector3.MoveTowards(
    //         transform.position,
    //         path_nodes [destination_index].transform.position,
    //         speed * Time.fixedDeltaTime);

    //     var distance_to_dest = Vector3.Distance(
    //         transform.position,
    //         path_nodes [destination_index].transform.position);
    //     var reached_destination = distance_to_dest < 0.1f; Mathf.Approximately (distance_to_dest, 0);

    //     if (!reached_destination)
    //     {
    //         return;
    //     }

    //     ++destination_index;
    //     destination_index %= path_nodes.Length;
    //      // } //else {
    //     //     if (stunned_timer <= 0) {
    //     //         stunned_timer = init_stunned_timer;
    //     //         stunned = false;
    //     //     } else --stunned_timer;
    //     // }


    // }// Update

    public override bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker)
    {
        return base.receive_hit(damage, Vector3.zero, attacker);
    }// receive_hit

    //--------------------------------------------------------------------------

    // public override void on_hit_sword(int damage, Vector3 knockback_velocity)
    // {
    //     // immune to sword - electricute Ninja
    //     Ninja.get().receive_hit(attack_power, -knockback_velocity);
    // }// on_hit_sword

    // public override void on_hit_by_jousting_pole(
    //     int damage, Vector3 knockback_velocity)
    // {
    //     // immune to pole - electricute Ninja
    //     var ninja = GameObject.Find ("Ninja");
    //     // ninja receives damage of his pole
    //     ninja.GetComponent<Ninja> ().receive_hit (damage);
    // } // on_hit_by_jousting_pole

    // public override void on_hit_spit(int damage)
    // {
    //     stunned = true;

    //     base.on_hit_spit(damage);

    // }// on_hit_spit

}
