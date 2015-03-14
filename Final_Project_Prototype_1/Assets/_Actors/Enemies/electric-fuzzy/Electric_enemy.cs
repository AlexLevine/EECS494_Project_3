using UnityEngine;
using System.Collections;
using System;

public class Electric_enemy : Enemy
{
    public int destination_index = 0;

    // Points that the platform should move between.
    public GameObject[] path_nodes;

    public bool is_moving;
	private bool stunned;
	private int stunned_timer;
	const int init_stunned_timer = 200;


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

    public override void Start()
    {
        base.Start();
        transform.position = path_nodes[destination_index].transform.position;
        ++destination_index;
		stunned = false;
    }// Start

    //--------------------------------------------------------------------------

    void FixedUpdate()
    {
        if (!stunned) {
			transform.position = Vector3.MoveTowards (
       	    	transform.position,
       	    	path_nodes [destination_index].transform.position,
       	    	speed * Time.fixedDeltaTime);

			var distance_to_dest = Vector3.Distance (
            	transform.position,
       	   		path_nodes [destination_index].transform.position);
			var reached_destination = Mathf.Approximately (distance_to_dest, 0);

			if (!reached_destination) {
				return;
			}

			++destination_index;
			destination_index %= path_nodes.Length;
		} else {
			if (stunned_timer <= 0) {
				stunned_timer = init_stunned_timer;
				stunned = false;
			} else --stunned_timer;
		}


    }// Update

	public override void on_hit_sword(int damage) {
		// immune to sword - electricute Ninja
		var ninja = GameObject.Find ("Ninja");
		// ninja receives damage of his sword
		ninja.GetComponent<Ninja> ().receive_hit (damage);
	}// on_hit_sword

	public override void on_hit_by_jousting_pole(int damage) {
		// immune to pole - electricute Ninja
		var ninja = GameObject.Find ("Ninja");
		// ninja receives damage of his pole
		ninja.GetComponent<Ninja> ().receive_hit (damage);
	} // on_hit_by_jousting_pole

	public override void on_hit_spit(int damage)
	{
		stunned = true;

        base.on_hit_spit(damage);

	}// on_hit_spit

}
