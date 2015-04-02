using UnityEngine;
using System.Collections;
using System;

public class Stationary_enemy_script : Enemy
{

    public GameObject projectile_prefab;
    public GameObject projectile_spawn_point;
    private GameObject closest_player;
    public float spawn_distance; // radius an enemy must be in to start attacking

    private static float time_between_shots = 1;
    private float timer;

    public override float attack_power { get { return 0; } }

    public override float max_health { get { return 20; } }

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

        if(timer >= time_between_shots)
        {
            shoot_projectile();
        }

    }// Update

    //--------------------------------------------------------------------------

    // returns true if the hit is fatal
    public override bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker)
    {
        return base.receive_hit(damage, Vector3.zero, attacker);
    }// receive_hit

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

        var direction_to_player =
                closest_player.transform.position - transform.position;
        direction_to_player = direction_to_player.normalized;
        bullet.GetComponent<Rigidbody>().velocity = direction_to_player * 10;
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
