using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Bridge : MonoBehaviour, Switchee
{
    bool activated = false;

    public void activate()
    {
        if (activated)
        {
            return;
        }

        print("activate bridge");
        GetComponent<Animator>().Play("lower_drawbridge");
        activated = true;
    }// activate

}
