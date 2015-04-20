using UnityEngine;
using System.Collections;
//using System;

public class Fire_enemy : Point_lerp_enemy
{
    public override float attack_power
    {
        get
        {
            return (is_puffing ? 2 : 1);
        }
    }

    public override float max_health
    {
        get
        {
            return 10;
        }
    }

    //--------------------------------------------------------------------------

    private bool is_puffing = false;

    //--------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();

        StartCoroutine(puff_coroutine());
    }// Start

    //--------------------------------------------------------------------------

    private IEnumerator puff_coroutine()
    {
        var puff_frequency = Random.Range(2f, 4f);
        var time_until_next_puff = puff_frequency;
        var puff_duration = 0.45f;

        // var hitbox = GetComponent<CapsuleCollider>();
        // var start_radius = hitbox.radius;
        // var end_radius = start_radius * 1.5f;

        var fire_effect = GetComponentInChildren<ParticleSystem>();
        var start_particle_size = fire_effect.startSize;
        var end_particle_size = start_particle_size * 3;

        while (true)
        {
            time_until_next_puff -= Time.deltaTime;
            if (time_until_next_puff > 0)
            {
                yield return null;
                continue;
            }

            is_puffing = true;
            var puff_time_elapsed = 0f;
            var lerp_percent = puff_time_elapsed / puff_duration;
            while (lerp_percent < 1)
            {
                // hitbox.radius = Mathf.Lerp(
                //     start_radius, end_radius, lerp_percent);
                fire_effect.startSize = Mathf.Lerp(
                    start_particle_size, end_particle_size, lerp_percent);

                puff_time_elapsed += Time.deltaTime;
                lerp_percent = puff_time_elapsed / puff_duration;
                yield return null;
            }// while lerp_percent < 1

            is_puffing = false;
            // hitbox.radius = start_radius;
            fire_effect.startSize = start_particle_size;
            time_until_next_puff = puff_frequency;
            yield return null;
        }// while true

    }// puff_coroutine
}
