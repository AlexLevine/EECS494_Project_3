using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    static public GameObject last_checkpoint = null;

    private bool ninja_passed_checkpoint = false;
    private bool llama_passed_checkpoint = false;

    private static List<Checkpoint_load_subscriber> subscribers =
            new List<Checkpoint_load_subscriber>();

    //--------------------------------------------------------------------------

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }// Start

    //--------------------------------------------------------------------------

    public static void subscribe(Checkpoint_load_subscriber subscriber)
    {
        subscribers.Add(subscriber);
    }// subscribe

    //--------------------------------------------------------------------------

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Llama.get().gameObject)
        {
            if(Llama.get().is_teamed_up)
            {
                ninja_passed_checkpoint = true;
            }

            llama_passed_checkpoint = true;
        }
        if(other.gameObject == Ninja.get().gameObject)
        {
            ninja_passed_checkpoint = true;
        }

        if(!llama_passed_checkpoint || !ninja_passed_checkpoint)
        {
            return;
        }

        // if(last_checkpoint != null)
        // {
        //     print("last_checkpoint:" + last_checkpoint);
        //     last_checkpoint.GetComponent<Checkpoint>().ninja_passed_checkpoint = false;
        //     last_checkpoint.GetComponent<Checkpoint>().llama_passed_checkpoint = false;
        // }

        last_checkpoint = gameObject;
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    public static void load_last_checkpoint()
    {
        // display prompt
        // do whatever

        var new_llama_pos = last_checkpoint.transform.position;
        var new_ninja_pos = new_llama_pos;

        new_ninja_pos.x += 2;

        Ninja.get().gameObject.transform.position = new_ninja_pos;
        Llama.get().gameObject.transform.position = new_llama_pos;

        foreach (var subscriber in subscribers)
        {
            subscriber.notify_checkpoint_load();
        }
    }

}
