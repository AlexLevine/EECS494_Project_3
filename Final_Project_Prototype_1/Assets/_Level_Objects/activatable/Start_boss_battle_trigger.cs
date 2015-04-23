﻿using UnityEngine;
using System.Collections;

public class Start_boss_battle_trigger : MonoBehaviour, Checkpoint_load_subscriber
{
    public GameObject boss_rear_up_cutscene;

    bool fight_started = false;
    bool player_died = false;

    void Start()
    {
        Checkpoint.subscribe(this);
    }// Start

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag != "Player" || fight_started)
        {
            return;
        }

        fight_started = true;

        if (player_died)
        {
            // fight_started = true;
            Samurai_Attack.get().notify_players_in_arena();
            return;
        }

        boss_rear_up_cutscene.GetComponent<Cut_scene>().activate(
            start_fight_for_first_time);
    }// OnTriggerEnter

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }// OnTriggerStay

    //--------------------------------------------------------------------------

    void start_fight_for_first_time()
    {
        if (!Ninja.get().is_teamed_up)
        {
            Ninja.get().team_up_engage_or_throw();
        }

        Player_character.force_team_up = true;
        fight_started = true;
        // Samurai_Attack.get().notify_players_in_arena();
        Camera.main.GetComponent<Camera_follow>().activate_boss_mode();
        print("battle started");
    }// start_fight_for_first_time

    //--------------------------------------------------------------------------

    public void notify_checkpoint_load()
    {
        player_died = true;
        fight_started = false;
    }// notify_checkpoint_load

}
