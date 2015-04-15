using UnityEngine;
using System.Collections;
using System;

// source: http://answers.unity3d.com/questions/837664/where-does-onlevelwasloaded-fit-in-this-flowchart.html
public class Level_load_event_handler : MonoBehaviour
{
    public static event Action after_start;

    // public GameObject ninja_prefab;
    // public GameObject llama_prefab;

    IEnumerator Start()
    {
        Level_load_event_handler.after_start += place_players;
        yield return null;

        if (after_start != null)
        {
            after_start();
        }
    }

    void place_players()
    {
        Ninja.get().transform.position = GameObject.Find(
            "ninja_start").transform.position;
        Llama.get().transform.position = GameObject.Find(
            "llama_start").transform.position;
    }
}
