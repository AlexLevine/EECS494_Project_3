using UnityEngine;
using System.Collections;

public class Start_boss_battle_trigger : MonoBehaviour, Checkpoint_load_subscriber
{
    bool fight_started = false;

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

        // lock_player_in_cutscene.GetComponent<Cut_scene>().activate(
        Samurai_Attack.get().notify_players_in_arena();
        print ("battle started");
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }// OnTriggerStay

    //--------------------------------------------------------------------------

    public void notify_checkpoint_load()
    {
        fight_started = false;
    }// notify_checkpoint_load

    //--------------------------------------------------------------------------


}
