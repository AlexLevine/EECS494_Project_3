using UnityEngine;
using System.Collections;
using System;

public class Fire_enemy : Enemy {

    public int destination_index = 0;

    // Points that the enemy should move between.
    public GameObject[] path_nodes;

    public bool is_moving;
    public bool on_fire;
    // private bool stunned;
//  private int stunned_timer;
//  const int init_stunned_timer = 200;


    public float speed;

    //--------------------------------------------------------------------------

    public override int attack_power
    {
        get
        {
            return 1;
        }
    }

    public override int max_health
    {
        get
        {
            return 5;
        }
    }

    //--------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        transform.position = path_nodes[destination_index].transform.position;
        ++destination_index;
        on_fire = true;
    }// Start

    //--------------------------------------------------------------------------

    void FixedUpdate()
    {
        // base.Update ();
        if (being_knocked_back)
        {
            return;
        }

        transform.position = Vector3.MoveTowards (
            transform.position,
            path_nodes [destination_index].transform.position,
            speed * Time.fixedDeltaTime);

        var distance_to_dest = Vector3.Distance (
            transform.position,
            path_nodes [destination_index].transform.position);
        var reached_destination = distance_to_dest < 0.1f; // Mathf.Approximately (distance_to_dest, 0);

        if (!reached_destination) {
                return;
        }

        ++destination_index;
        destination_index %= path_nodes.Length;


    }// Update
}
