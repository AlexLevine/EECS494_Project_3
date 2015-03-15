using UnityEngine;
using System.Collections;
using System;

public class Flying_enemy_script : Enemy {

	public GameObject projectile_prefab;
	public float spawn_distance;
	
	public int num_to_spawn;
	public float time_between_spawns;
	
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
			return 3;
		}
	}
	
	// Use this for initialization
	public override void Start()
	{
		base.Start ();

	}// Start
	
	//--------------------------------------------------------------------------
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update ();

		var llama_pos = GameObject.Find("Llama").transform.position;
		var ninja_pos = GameObject.Find("Ninja").transform.position;
		
		var llama_distance = Vector3.Distance(llama_pos, transform.position);
		var ninja_distance = Vector3.Distance(ninja_pos, transform.position);
		
		if (llama_distance <= spawn_distance ||
		    ninja_distance <= spawn_distance)
		{
			if (is_spawning)
			{
				return;
			}
			
			is_spawning = true;
			
			//print("spawn!");
			StartCoroutine(spawn());
		}
	}// Update
	
	//--------------------------------------------------------------------------
	
	private IEnumerator spawn()
	{
		while (num_to_spawn > 0)
		{
			--num_to_spawn;
			Vector3 spawn_point = transform.position;
			spawn_point.y -= 1f;
			Instantiate(projectile_prefab, spawn_point, Quaternion.identity);
			yield return new WaitForSeconds(time_between_spawns);
		}
	}// spawn
}
