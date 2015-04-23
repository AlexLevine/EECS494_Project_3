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
}
