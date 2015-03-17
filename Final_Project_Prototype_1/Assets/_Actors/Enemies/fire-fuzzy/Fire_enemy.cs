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
//	private int stunned_timer;
//	const int init_stunned_timer = 200;


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
		base.Update ();
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


	}// Update

	// public override void on_hit_sword(int damage) {
	// 	// immune to sword - electricute Ninja
	// 	var ninja = GameObject.Find ("Ninja");
	// 	// ninja receives damage of his sword
	// 	ninja.GetComponent<Ninja> ().receive_hit (damage);
	// }// on_hit_sword

	public override void on_hit_charge(int damage) {
		if (on_fire) {
			// burns llama - and takes no damge if still on fire
			var llama = GameObject.Find ("Llama");
			llama.GetComponent<Llama> ().receive_hit (damage);
		}
	}// on_hit_charge

	public override void on_hit_spit(int damage)
	{
		base.on_hit_spit (damage);

		// put out fire:
		foreach (Transform t in transform)
		{
			if(t.name == "Fire_origin") {
				t.GetComponentInChildren<ParticleSystem>().enableEmission = false;
				on_fire = false;
			}
		}


	}// on_hit_spit



}
