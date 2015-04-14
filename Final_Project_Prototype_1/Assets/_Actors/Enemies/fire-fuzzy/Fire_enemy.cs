using UnityEngine;
using System.Collections;
//using System;

public class Fire_enemy : Point_lerp_enemy
{
    private int puff_timer;
    const int base_puff_timer = 100;
    public bool puffing = false;

    //--------------------------------------------------------------------------

    public override float attack_power
    {
        get
        {
            return 1;
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

    public override void Start()
    {
        base.Start();

        int rand = Random.Range (0, 10);
        puff_timer = base_puff_timer * rand;
    }// Start

    //--------------------------------------------------------------------------

    protected override void update_impl()
    {
        base.update_impl();

        if (!puffing)
        {
            if (puff_timer <= 0)
            {
                int rand = Random.Range (0, 10);
                puff_timer = base_puff_timer * rand;
                puffing = true;
                StartCoroutine( puff());
            } else -- puff_timer;
        }

        // base.Update ();
        // if (being_knocked_back)
        // {
        //     return;
        // }

    }// Update

    //--------------------------------------------------------------------------

    // public override bool receive_hit(
    //     float damage, Vector3 knockback_velocity, GameObject attacker)
    // {
    //     return base.receive_hit(damage, Vector3.zero, attacker);
    // }// receive_hit

    //--------------------------------------------------------------------------

    private IEnumerator puff ()
    {
        CapsuleCollider collider =  this.GetComponent<CapsuleCollider>();
        ParticleSystem particles = this.GetComponentInChildren<ParticleSystem>();
        float radius = collider.radius;
        float particleSize = particles.startSize;
        for (int i = 0; i < 25; ++i)
        {
            collider.radius *= 1.05f;
            particles.startSize *= 1.05f;
            yield return new WaitForSeconds(.1f);
        }
        collider.radius = radius;
        particles.startSize = particleSize;
        puffing = false;
    }
}
