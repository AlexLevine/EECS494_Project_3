using UnityEngine;
using System.Collections;

public class Enemy_trap_room : MonoBehaviour 
{
    public GameObject event_to_trigger;

    void Update()
    {
        var enemies_left = GetComponentsInChildren<Enemy>();
        if (enemies_left.Length == 0)
        {
            event_to_trigger.GetComponent<Switchee>().activate();
        }
    }
}
