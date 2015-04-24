using UnityEngine;
using System.Collections;

public class Point_lerp_enemy : Enemy
{
    public int destination_index = 0;

    // Points that the enemy should move between.
    public GameObject[] path_nodes;

    public float speed;

    public override float gravity { get { return 0; } }

    //--------------------------------------------------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        transform.position = path_nodes[destination_index].transform.position;
        ++destination_index;

        kinematic_rigidbody.isKinematic = true;
    }// Start

    // public override Sweep_test_summary move()
    // {
    //     return new Sweep_test_summary();
    // }

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            path_nodes [destination_index].transform.position,
            speed * Time.fixedDeltaTime);

        var distance_to_dest = Vector3.Distance(
            transform.position,
            path_nodes [destination_index].transform.position);
        var reached_destination = distance_to_dest < 0.1f; Mathf.Approximately (distance_to_dest, 0);

        if (!reached_destination)
        {
            return;
        }

        ++destination_index;
        destination_index %= path_nodes.Length;
    }// update_impl

    public override void on_death(GameObject killer)
    {
        print("point lerp on death");
        play_damage_vocals();
        base.on_death(killer);
    }

    public override void play_damage_vocals()
    {
        print("play damage vocals");
        GetComponent<Sound_effect_randomizer>().play();
    }

}
