using UnityEngine;
using System.Collections;

public class Change_to_arena_cam : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        Camera_follow.in_boss_arena = true;
        Samurai_Attack.get().notify_players_in_arena();
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
}
