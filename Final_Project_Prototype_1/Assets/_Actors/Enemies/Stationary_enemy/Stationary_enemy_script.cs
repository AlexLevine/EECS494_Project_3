using UnityEngine;
using System.Collections;
using System;

public class Stationary_enemy_script : Enemy {

    public GameObject projectile_prefab;
    public GameObject projectile_spawn_point; 
    private GameObject closest_player; 
    public float spawn_distance; // radius an enemy must be in to start attacking

    public float time_between_spawns;
    private float timer;
            
    
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
            return 20;
        }
    }
    
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }// Start
    
    //--------------------------------------------------------------------------
    
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime; 

        get_closest_player();
        look_toward(closest_player);

        if(timer >= time_between_spawns)
        {
            shoot_projectile();
        }

    }// Update

    //--------------------------------------------------------------------------
    
    private void shoot_projectile()
    {

        if(Vector3.Distance(closest_player.transform.position, transform.position) 
            >= spawn_distance)
        {
            return; 
        }


        var bullet = Instantiate(
                projectile_prefab, projectile_spawn_point.transform.position,
                transform.localRotation) as GameObject;
        
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10; 
        timer = 0; 

    }

    //--------------------------------------------------------------------------

    private void get_closest_player()
    {
        // Get closest player
        Vector3 llama_pos = Llama.get().gameObject.transform.position; 
        Vector3 ninja_pos = Ninja.get().gameObject.transform.position;

        if(Vector3.Distance(llama_pos, transform.position) <= 
           Vector3.Distance(ninja_pos, transform.position))
        {
            closest_player = Llama.get().gameObject;
        }
        else
        {
            closest_player = Ninja.get().gameObject;
        }
    }
}
