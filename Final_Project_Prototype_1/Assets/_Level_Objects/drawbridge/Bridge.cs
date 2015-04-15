using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Bridge : Switchee
{
    bool activated = false;
    string state_to_play = "lower_drawbridge";

    public override void activate(Switchee_callback callback=null)
    {
        if (activated)
        {
            return;
        }

        // print("activate bridge");
        GetComponent<Animator>().Play(state_to_play);
        StartCoroutine(animation_poll(callback));
        activated = true;
    }// activate

    //--------------------------------------------------------------------------

    private IEnumerator animation_poll(Switchee_callback callback)
    {
        var animator = GetComponent<Animator>();

        while (true)
        {
            var still_playing = animator.GetCurrentAnimatorStateInfo(0).IsName(
                state_to_play);
            if (still_playing)
            {
                yield return null;
                continue;
            }

            if (callback != null)
            {
                callback();
                return false;
            }
        }
    }// animation_poll

}
