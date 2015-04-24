using UnityEngine;
using System.Collections;
using System;

public class Flying_enemy_script : Enemy {

    // public GameObject projectile_prefab;
    // public float spawn_distance;

    // public int num_to_spawn;
    // public float time_between_spawns;

    // public bool _______________________________________;

    // public bool is_spawning = false;

    // private float speed = 5f;

    // //--------------------------------------------------------------------------


    // public override float attack_power
    // {
    //     get
    //     {
    //         return 0;
    //     }
    // }

    // public override float max_health
    // {
    //     get
    //     {
    //         return 3;
    //     }
    // }

    // // Use this for initialization
    // public override void Start()
    // {
    //     base.Start ();
    // }// Start

    // //--------------------------------------------------------------------------

    // // Update is called once per frame
    // public override void Update()
    // {
    //     base.Update();

    //     var llama_pos = Llama.get().transform.position;
    //     var ninja_pos = Ninja.get().transform.position;

    //     var llama_distance = Vector3.Distance(llama_pos, transform.position);
    //     var ninja_distance = Vector3.Distance(ninja_pos, transform.position);


    //     if (llama_distance <= spawn_distance ||
    //         ninja_distance <= spawn_distance)
    //     {
    //         if (is_spawning)
    //         {
    //             return;
    //         }

    //         is_spawning = true;

    //         //print("spawn!");
    //         StartCoroutine(spawn());
    //     } else if (llama_pos.z > this.transform.position.z &&
    //                 ninja_pos.z > this.transform.position.z) {
    //             // move towards the players if they have passes the flying enemy
    //         Vector3 dest = new Vector3(0, 0, 0);
    //         if (llama_distance < ninja_distance) { // move towards llama
    //             dest = llama_pos;
    //         } else { // move towards ninja
    //             dest = ninja_pos;
    //         }
    //         dest.x = this.transform.position.x;
    //         dest.y = this.transform.position.y;
    //         this.transform.position = Vector3.MoveTowards (
    //             this.transform.position,
    //             dest,
    //             speed * Time.fixedDeltaTime);

    //     }
    // }// Update

    // //--------------------------------------------------------------------------

    // private IEnumerator spawn()
    // {
    //     while (num_to_spawn > 0)
    //     {
    //         --num_to_spawn;
    //         Vector3 spawn_point = transform.position;
    //         spawn_point.y -= 1f;
    //         Instantiate(projectile_prefab, spawn_point, Quaternion.identity);
    //         yield return new WaitForSeconds(time_between_spawns);
    //     }
    // }// spawn
}
