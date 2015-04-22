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

    public static void load_last_checkpoint(
        Checkpoint_load_callback callback=null)
    {
        // screen fade to black
        // disable actors

        var new_llama_pos = last_checkpoint.transform.position;
        var new_ninja_pos = new_llama_pos;
        new_ninja_pos.x += 2;

        Camera_follow.adjust_main_camera(
            new_point_of_interest: new_llama_pos,
            transition_duration: 0);

        Ninja.get().gameObject.transform.position = new_ninja_pos;
        Llama.get().gameObject.transform.position = new_llama_pos;

        foreach (var subscriber in subscribers)
        {
            subscriber.notify_checkpoint_load();
        }

        // screen fade in
        // re-enable actors
    }

    // delegate void Fade_screen_callback();

    // static IEnumerator fade_screen(
    //     bool fade_to_black, Fade_screen_callback callback=null)
    // {
    //     var start_alpha_val = 1f;
    //     var end_alpha_val = 0f;

    //     var fade_duration = 0f;
    //     var time_elapsed = 1f;

    //     var lerp_percent = Mathf.Clamp01(time_elapsed / fade_duration);
    //     while (lerp_percent <= 1)
    //     {
    //         GUI.color = new Color(0, 0, 0, alphaFadeValue);
    //         GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), blackTexture );

    //     }

    // }

    public static void unsubscribe(GameObject obj)
    {
        subscribers.Remove(obj.GetComponent<Checkpoint_load_subscriber>());
    }

}
