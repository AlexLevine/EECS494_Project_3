using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Enemy : Actor
{
    public virtual int score_when_killed { get { return 1; } }

    public virtual float attack_power { get { return 1f; } }

    //protected override float invincibility_flash_duration {
    //   get { return 0f; } }

    public override void Start()
    {
        base.Start();
        Model.get().register_enemy(this);
    }


    //--------------------------------------------------------------------------

    // // Use this for initialization
    // public override void Start()
    // {
    //     base.Start();
    // }// Start

    //--------------------------------------------------------------------------

    // // Update is called once per frame
    // public override void Update ()
    // {
    //     base.Update();
    // }// Update

    //--------------------------------------------------------------------------    --------------------------------------------------------------------------

    public virtual void OnTriggerEnter(Collider c)
    {
        if (actors_paused)
        {
            return;
        }

        var player = c.gameObject.GetComponent<Player_character>();
        if (player == null || being_knocked_back ||
            taking_damage_animation_playing)
        {
            return;
        }

        var knockback_direction = (
            c.gameObject.transform.position - transform.position).normalized;
        var knockback_velocity = knockback_direction * 5;

        player.receive_hit(attack_power, knockback_velocity, gameObject);

        // on_player_hit();
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }// OnTriggerStay

    //--------------------------------------------------------------------------

    public override void on_death(GameObject killer)
    {
        if (killer.name == Ninja_sword.global_name)
        {
            Timer.num_enemies_killed_by_ninja += score_when_killed;
        }

        if (killer.name == Llama_spit.global_name)
        {
            Timer.num_enemies_killed_by_llama += score_when_killed;
        }

        GameObject boom = (GameObject)Instantiate(Resources.Load("dead_enemy"));
        boom.transform.position = transform.position;

        drop_item();
        Model.get().remove_enemy(this);

        Destroy(gameObject);
    }// on_death

    //--------------------------------------------------------------------------

    protected virtual void drop_item()
    {
        // drop health object a third of the time
        int rand = UnityEngine.Random.Range(0, 5);
        if (rand < 1) {
            GameObject heart = (GameObject)Instantiate(Resources.Load ("Collectable_Heart"));
            heart.transform.position = transform.position;
        }
    }

    //--------------------------------------------------------------------------

    // void OnDestroy()
    // {
    //     Model.get().remove_enemy(gameObject);
    //     // enemies.Remove(gameObject);
    //     // Player_character.notify_enemy_gone(gameObject);
    // }// OnDestroy
}
