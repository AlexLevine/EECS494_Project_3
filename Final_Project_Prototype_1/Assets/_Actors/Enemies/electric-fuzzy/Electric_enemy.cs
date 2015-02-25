using UnityEngine;
using System.Collections;
using System;

public class Electric_enemy : Enemy
{
    public int destination_index = 0;

    // Points that the platform should move between.
    public GameObject[] path_nodes;

    public bool is_moving; 

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
            return 3;
        }
    }

    //--------------------------------------------------------------------------

    void Start()
    {
        transform.position = path_nodes[destination_index].transform.position;
        ++destination_index;
    }// Start

    //--------------------------------------------------------------------------

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            path_nodes[destination_index].transform.position,
            speed * Time.fixedDeltaTime);

        var distance_to_dest = Vector3.Distance(
            transform.position,
            path_nodes[destination_index].transform.position);

        var reached_destination = Mathf.Approximately(distance_to_dest, 0);
        if (!reached_destination)
        {
            return;
        }
        ++destination_index;
        destination_index %= path_nodes.Length;
    }// Update
}
