using UnityEngine;
using System.Collections;
using System;

public class Evil_Eyeball : Enemy {

    private float speed = 2f;

    //the 2 players that the sword targets
    public string player_name1 = "Llama";
    public string player_name2 = "Ninja";

    public GameObject start_location;

    private int locate_timer;
    const int full_locate_timer = 300;

    public bool is_moving;
    private bool stunned;
    private int stunned_timer;
    const int init_stunned_timer = 300;

    public Vector3 start;
    private Vector3 dest;



    //--------------------------------------------------------------------------

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

    //--------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();

        start = (start_location == null ?
                 transform.position : start_location.transform.position);
        transform.position = start;

        stunned = false;
        locate_timer = 0;
    }// Start

    //--------------------------------------------------------------------------

    void Update()
    {
        base.Update ();
        if (!stunned) {
            //Basic Movement
            Vector3 pos = this.transform.position;

            Vector3 player1_pos = pos;
            Vector3 player2_pos = pos;

            // if the objects exist, find their postions
            if (GameObject.Find(player_name1))
            {
                player1_pos = GameObject.Find(player_name1).transform.position;
            }
            if (GameObject.Find(player_name2))
            {
                player2_pos = GameObject.Find(player_name2).transform.position;
            }

            if (locate_timer <= 0) {
                locate_timer = full_locate_timer;
                // set destination point
                if (player1_pos == pos && player2_pos != pos) {
                    dest = player2_pos;
                } else if (player2_pos == pos && player1_pos != pos) {
                    dest = player1_pos;
                } else if (player2_pos == pos && player1_pos == pos) {
                    dest = pos;
                }
                // else if (Mathf.Sqrt(Mathf.Pow((player1_pos.y - pos.y), 2) + Mathf.Pow((player1_pos.x - pos.x), 2)) <
                //     Mathf.Sqrt(Mathf.Pow((player2_pos.y - pos.y), 2) + Mathf.Pow((player2_pos.x - pos.x), 2)))
                else if (Vector3.Distance (player1_pos, pos) < Vector3.Distance (player2_pos, pos)) {
                    dest = player1_pos;
                } else {
                    dest = player2_pos;
                }
            } else --locate_timer;

            Vector3 angle = dest - pos;
            angle.Normalize ();

            this.GetComponent<Rigidbody>().velocity = angle * speed;

            if (this.GetComponent<Rigidbody>().velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(this.GetComponent<Rigidbody>().velocity),
                    Time.deltaTime * speed);
            }
        } else {
            if (stunned_timer <= 0) {
                stunned_timer = init_stunned_timer;
                stunned = false;
            } else --stunned_timer;
        }


    }// Update

    // public override void on_hit_sword(int damage) {
    //  // immune to sword - do nothing

    // }// on_hit_sword

    // public override void on_hit_spit(int damage)
    // {
    //     stunned = true;
    //     base.on_hit_spit(damage);

    // }// on_hit_spit

    // public override void on_hit_charge(int damage)
    // {
    //  // immune to charge - possibly do something to Llama here
    // }// on_hit_charge

    // public override void on_hit_by_jousting_pole(int damage) {
    //  // immune to pole
    // } // on_hit_by_jousting_pole
}
