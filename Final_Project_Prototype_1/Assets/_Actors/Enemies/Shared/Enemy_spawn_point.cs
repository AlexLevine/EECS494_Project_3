using UnityEngine;
using System.Collections;
using System;

public class Enemy_spawn_point : MonoBehaviour
{
    public GameObject enemy_prefab;
    public float spawn_distance;

    public int num_to_spawn;
    public float time_between_spawns;

    private bool is_spawning = false;

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        if (enemy_prefab.GetComponent<Enemy>() == null)
        {
            throw new Exception("Only enemies can be attached to span points");
        }
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        var llama_pos = Llama.get().transform.position;
        var ninja_pos = Ninja.get().transform.position;

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

            print("spawn!");
            StartCoroutine(spawn());
        }
    }// Update

    //--------------------------------------------------------------------------

    private IEnumerator spawn()
    {
        while (num_to_spawn > 0)
        {
            --num_to_spawn;
            Instantiate(enemy_prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(time_between_spawns);
        }
    }// spawn
}
