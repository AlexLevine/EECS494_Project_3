using UnityEngine;
using System.Collections;

public class Fall_death : MonoBehaviour, Checkpoint_load_subscriber
{
    public bool is_dying = false; 

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
        if(is_dying)
        {
            return; 
        }

        is_dying = true; 
        Camera_follow.stop_following_player();
        Checkpoint.load_last_checkpoint(notify_checkpoint_load);
    }

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }

    public void notify_checkpoint_load()
    {
        is_dying = false; 
        Camera_follow.start_following_player();
    }// notify_checkpoint_load
}
