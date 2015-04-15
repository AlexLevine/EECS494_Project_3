using UnityEngine;
using System.Collections;

public class Fire_wall : Switchee
{
    public override void activate(Switchee_callback callback=null)
    {
        gameObject.SetActive(false);

        if (callback != null)
        {
            callback();
        }
    }
}
