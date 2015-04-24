using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public delegate void Checkpoint_load_callback();

    static public GameObject last_checkpoint = null;

    private bool ninja_passed_checkpoint = false;
    private bool llama_passed_checkpoint = false;

    //--------------------------------------------------------------------------

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }// Start

    //--------------------------------------------------------------------------

    public static void subscribe(Checkpoint_load_subscriber subscriber)
    {
        Model.get().subscribe_to_checkpoint_load(subscriber);
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

    public static void load_last_checkpoint(
        Checkpoint_load_callback callback=null)
    {
        // screen fade to black
        // disable actors

        last_checkpoint.GetComponent<Checkpoint>().load(callback);


        // screen fade in
        // re-enable actors
    }

    void load(Checkpoint_load_callback callback)
    {
        Fade_screen.get().fade_to_black(() => StartCoroutine(snap_camera(callback)));
        // StartCoroutine(snap_camera(callback));
    }

    IEnumerator snap_camera(Checkpoint_load_callback callback)
    {
        // Actor.actors_paused = true;

        var new_llama_pos = transform.position;
        var new_ninja_pos = new_llama_pos;
        new_ninja_pos.x += 2;

        Ninja.get().gameObject.transform.position = new_ninja_pos;
        Llama.get().gameObject.transform.position = new_llama_pos;

        Camera_follow.adjust_main_camera(
            new_point_of_interest: new_llama_pos,
            transition_duration: 0);

        yield return new WaitForSeconds(0.5f);

        foreach (var subscriber in Model.get().get_checkpoint_subscribers())
        {
            subscriber.notify_checkpoint_load();
        }

        // Fade_screen.get().fade_from_black(() => Actor.actors_paused = false);
        Fade_screen.get().fade_from_black(() => callback());
    }

    //--------------------------------------------------------------------------

    public static void unsubscribe(GameObject obj)
    {
        Model.get().unsubscribe_from_checkpoint_load(
            obj.GetComponent<Checkpoint_load_subscriber>());
    }

}
