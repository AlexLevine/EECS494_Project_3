using UnityEngine;
using System.Collections;
// using System;

public class Stationary_enemy_script : Enemy
{

    public GameObject projectile_prefab;
    public GameObject projectile_spawn_point;
    private GameObject closest_player;
    public float attack_radius = 30; // radius an enemy must be in to start attacking

    private static float time_between_shots = 2;
    private float timer;

    public override float attack_power { get { return 0; } }

    public override float max_health { get { return 15; } }

    public override float gravity { get { return 0; } }

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        time_between_shots += Random.Range(0f, 1f);
        kinematic_rigidbody.isKinematic = true;
    }// Start

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        base.update_impl();
        timer += Time.deltaTime;

        get_closest_player();
        look_toward(closest_player, 2f);

        if(timer >= time_between_shots)
        {
            shoot_projectile();
        }

    }// update_impl

    //--------------------------------------------------------------------------

    // returns true if the hit is fatal
    public override bool receive_hit(
        float damage, Vector3 knockback_velocity, GameObject attacker,
        float knockback_duration)
    {
        // This enemy should not be knocked back.
        return base.receive_hit(damage, Vector3.zero, attacker, 0);
    }// receive_hit

    //--------------------------------------------------------------------------

    private void shoot_projectile()
    {
        var in_range = Vector3.Distance(
            closest_player.transform.position,
            transform.position) <= attack_radius;
        if(!in_range)
        {
            return;
        }

        var bullet = Instantiate(
                projectile_prefab, projectile_spawn_point.transform.position,
                body.transform.localRotation) as GameObject;

        // var direction_to_player =
        //         closest_player.transform.position - transform.position;
        // direction_to_player = direction_to_player.normalized;
        bullet.GetComponent<Rigidbody>().velocity = body.transform.forward * 14;
        timer = 0;
    }// shoot_projectile

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
    }// get_closest_player
}
