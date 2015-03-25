using UnityEngine;
using System.Collections;

public class Fall_death : MonoBehaviour
{
    public GameObject respawn_point;

    // // Use this for initialization
    // void Start ()
    // {

    // }

    // // Update is called once per frame
    // void Update ()
    // {

    // }

    void OnTriggerEnter(Collider c)
    {
        if (!c.gameObject.tag.Contains("Player"))
        {
            return;
        }

        var llama_pos = respawn_point.transform.position;
        var ninja_pos = llama_pos;

        ninja_pos.x += 2;

        Ninja.get().gameObject.transform.position = ninja_pos;
        Llama.get().gameObject.transform.position = llama_pos;
    }
}
