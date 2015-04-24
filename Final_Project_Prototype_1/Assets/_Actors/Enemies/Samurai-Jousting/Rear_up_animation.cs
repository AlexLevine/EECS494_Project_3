using UnityEngine;
using System.Collections;

public class Rear_up_animation : Switchee
{
    public GameObject roar_sound_player;

    public bool is_rearing_up
    {
        get
        {
            var cur_state_hash =
                animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            return cur_state_hash != idle_state_hash;
        }
    }

    Animator animator;
    int rear_up_trigger_id;
    int idle_state_hash;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rear_up_trigger_id = Animator.StringToHash("rear-up");
        idle_state_hash = Animator.StringToHash("Idle");
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void activate(
        Switchee_callback callback, float? callback_delay)
    {
        if (is_rearing_up)
        {
            return;
        }

        base.activate(callback, callback_delay);

        StartCoroutine(do_rear_up());
    }

    //--------------------------------------------------------------------------

    IEnumerator do_rear_up()
    {
        animator.SetTrigger(rear_up_trigger_id);

        while (is_rearing_up)
        {
            yield return null;
        }

        do_callback();
    }

    //--------------------------------------------------------------------------

    public void play_roar()
    {
        print("roar!");
        roar_sound_player.GetComponent<Sound_effect_randomizer>().play();
    }
}
