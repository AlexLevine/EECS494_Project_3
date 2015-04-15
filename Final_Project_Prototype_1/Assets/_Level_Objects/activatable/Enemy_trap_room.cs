using UnityEngine;
using System.Collections;

public class Enemy_trap_room : MonoBehaviour
{
    public GameObject event_or_cutscene_to_trigger;

    Switchee switchee;
    Cut_scene cut_scene;

    void Start()
    {
        switchee = event_or_cutscene_to_trigger.GetComponent<Switchee>();
        cut_scene = event_or_cutscene_to_trigger.GetComponent<Cut_scene>();
    }// Start

    //--------------------------------------------------------------------------

    void Update()
    {
        var enemies_left = GetComponentsInChildren<Enemy>();
        if (enemies_left.Length != 0)
        {
            return;
        }

        if (cut_scene != null)
        {
            cut_scene.activate();
            return;
        }

        if (switchee != null)
        {
            switchee.activate(null, null);
        }
    }// Update
}
