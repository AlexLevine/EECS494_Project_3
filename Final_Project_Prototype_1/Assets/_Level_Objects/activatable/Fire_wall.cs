using UnityEngine;
using System.Collections;

public class Fire_wall : Switchee
{
    public override void activate(
        Switchee_callback callback, float? callback_delay)
    {
        base.activate(callback, callback_delay);
        gameObject.SetActive(false);
        do_callback();
    }
}
