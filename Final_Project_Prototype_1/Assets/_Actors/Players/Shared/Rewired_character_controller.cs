using UnityEngine;
using System.Collections;
using Rewired;

public class Rewired_character_controller : MonoBehaviour
{
    public int player_id = 0;
    // private GameObject player_character;

    private Player player; // The Rewired Player
    private Player_character pc;

    void Awake()
    {
        player = ReInput.players.GetPlayer(player_id);
    }

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        pc = gameObject.GetComponent<Player_character>();
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        process_input();
    }

    //--------------------------------------------------------------------------

    private void process_input()
    {
        if (player.GetButtonDown("jump"))
        {
            pc.jump();
        }

        if (player.GetButtonDown("team_up"))
        {
            pc.team_up_engage_or_throw();
        }

        if (player.GetButtonDown("attack"))
        {
            pc.attack();
        }

        if(player.GetButtonDown("lock_on") ||
           player.GetButtonUp("lock_on"))
        {
            pc.toggle_lock_on();
        }

        Vector3 tilt = Vector3.zero;
        tilt.x = player.GetAxis("move_x"); // left and right
        tilt.z = player.GetAxis("move_z"); // forward and backward

        var sprint = player.GetButton("sprint");

        var target_velocity = tilt * (sprint ? pc.sprint_speed : pc.run_speed);
        // if (!on_ground)
        // {
        //     target_velocity.x =
        // }

        target_velocity.y = pc.velocity.y;
        // target_velocity *= Time.deltaTime;

        pc.velocity = target_velocity;

        // move(delta_position);
    }// process_input
}
