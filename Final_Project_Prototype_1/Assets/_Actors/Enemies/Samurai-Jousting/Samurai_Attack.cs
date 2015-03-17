﻿using UnityEngine;
using System.Collections;

public class Samurai_Attack : Enemy {
    public enum samurai_states_e {
        WAITING,
        LOOKING,
        ATTACKING,
        PAUSING
    }

    public GameObject llama, ninja;
    public samurai_states_e cur_state;
    public float look_thresh;
    public float attack_thresh;

    public float max_pause_time;
    private float cur_pause_time;

    public float speed;

    // Use this for initialization
    public override void Start () {
        base.Start();
        cur_state = samurai_states_e.WAITING;
        cur_pause_time = 0f;

        llama = Llama.get().gameObject;
        ninja = Ninja.get().gameObject;

        if(llama == null || ninja == null)
        {
            print("Could not find llama or ninja!!!!!");
            print("OH NOOOOOOOOOOO");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();

        Vector3 closest_player = look_for_player();
        float dist_to_closest_player = Vector3.Distance(closest_player,
                                                        transform.position);

        switch(cur_state){
            case samurai_states_e.WAITING:
                if(dist_to_closest_player <= look_thresh)
                {
                    cur_state = samurai_states_e.LOOKING;
                    return;
                }
                break;

            case samurai_states_e.LOOKING:
                if(dist_to_closest_player > look_thresh)
                {
                    print(dist_to_closest_player);
                    cur_state = samurai_states_e.WAITING;
                }
                else
                {
                    if(closest_player == llama.transform.position)
                    {
                        look_toward(llama);
                    }
                    else
                    {
                        look_toward(ninja);
                    }
                }

                // ------------------------------
                if(dist_to_closest_player < attack_thresh)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit))
                    {
                        if(hit.collider.gameObject.name == "Llama" ||
                           hit.collider.gameObject.name == "Ninja" )
                        {
                            cur_state = samurai_states_e.ATTACKING;
                        }
                    }
                }
                break;

            case samurai_states_e.ATTACKING:
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
                break;

            case samurai_states_e.PAUSING:
                cur_pause_time += Time.deltaTime;
                if(cur_pause_time >= max_pause_time)
                {
                    cur_pause_time = 0;
                    cur_state = samurai_states_e.WAITING;
                }

                break;
            default:
                print("Oh no!");
                break;
        }
    }


    // void OnCollisionEnter(Collision collision){
    //     cur_state = samurai_states_e.PAUSING;
    // }
    // void OnCollisionStay(Collision collision){
    //     cur_state = samurai_states_e.PAUSING;
    // }


    // returns the position of the nearest player to the enemy
    private Vector3 look_for_player()
    {
        Vector3 llama_pos = llama.transform.position;
        Vector3 ninja_pos = ninja.transform.position;

        float llama_dist = Vector3.Distance(llama_pos, transform.position);
        float ninja_dist = Vector3.Distance(ninja_pos, transform.position);

        if(llama_dist < ninja_dist)
        {
            return llama_pos;
        }
        else
        {
            return ninja_pos;
        }
    }


    public override int attack_power
    {
        get
        {
            return 1;
        }
    }

    public override int max_health
    {
        get
        {
            return 3;
        }
    }

}