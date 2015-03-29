using UnityEngine;
using System.Collections;
using System;

public class Stationary_enemy_script : Enemy {

	public GameObject projectile_prefab;
	public float spawn_distance;
	
	public int num_to_spawn;
	public int time_between_spawns;
	private int timer;
	
	public bool _______________________________________;
	
	public bool is_spawning = false;
	
	//--------------------------------------------------------------------------
	
	
	public override int attack_power
	{
		get
		{
			return 0;
		}
	}
	
	public override int max_health
	{
		get
		{
			return 5;
		}
	}
	
	// Use this for initialization
	public override void Start()
	{
		base.Start ();
		timer = time_between_spawns;
	}// Start
	
	//--------------------------------------------------------------------------
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();
		
		var llama_pos = Llama.get().transform.position;
		var ninja_pos = Ninja.get().transform.position;
		
		var llama_distance = Vector3.Distance(llama_pos, transform.position);
		var ninja_distance = Vector3.Distance(ninja_pos, transform.position);
		
		
		if (llama_distance <= spawn_distance ||
		    ninja_distance <= spawn_distance)
		{
			if (!is_spawning)
			{
				Vector3 spawn_point = transform.position;
				spawn_point.y -= 1f;
				Instantiate(projectile_prefab, spawn_point, Quaternion.identity);
				is_spawning = true;
				timer = time_between_spawns;
			} else {
				--timer;
				if (timer <= 0) is_spawning = false;
			}
			
		}
	}// Update
	
}
