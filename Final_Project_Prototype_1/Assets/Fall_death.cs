using UnityEngine;
using System.Collections;

public class Fall_death : MonoBehaviour, Checkpoint_load_subscriber
{
    void Start()
    {
        Checkpoint.subscribe(this);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<Player_character>() == null)
        {
            return;
        }

        Camera_follow.stop_following_player();
        Checkpoint.load_last_checkpoint();
    }

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }

    public void notify_checkpoint_load()
    {
        Camera_follow.start_following_player();
        // Camera_follow.adjust_main_camera(
        //     new_point_of_interest: Camera_follow.calculate_player_midpoint())
    }// notify_checkpoint_load
}
