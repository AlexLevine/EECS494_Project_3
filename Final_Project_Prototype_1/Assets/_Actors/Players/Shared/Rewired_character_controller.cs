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
            print("team up");
            pc.team_up_engage_or_throw();
        }

        if (player.GetButtonDown("attack"))
        {
            pc.attack();
        }

        if(player.GetButtonDown("lock_on") ||
           player.GetButtonUp("lock_on") && pc.is_locked_on)
        {
            pc.toggle_lock_on();
        }

        Vector3 tilt = Vector3.zero;
        tilt.x = player.GetAxis("move_x"); // left and right
        tilt.z = player.GetAxis("move_z"); // forward and backward
        tilt = tilt.normalized;

        pc.adjust_jousting_pole(tilt.z, tilt.x);

        var sprint = player.GetButton("sprint");

        var target_velocity = tilt * (sprint ? pc.sprint_speed : pc.run_speed);
        target_velocity.y = pc.velocity.y;
        calculate_new_pc_velocity(target_velocity);

        // move(delta_position);
    }// process_input

    //--------------------------------------------------------------------------

    private void calculate_new_pc_velocity(Vector3 target_velocity)
    {
        // Determine whether we need to keep the character's momentum or
        // accelerate in the opposite direction.
        // var velocity_step = pc.acceleration * Time.deltaTime;

        if (pc.is_grounded)
        {
            pc.velocity = target_velocity;
            // if (target_velocity.x == 0)
            // {
            //     pc.velocity.x = 0;
            // }

            // if (target_velocity.z == 0)
            // {
            //     pc.velocity.z = 0;
            // }
            return;
        }




        // target_velocity *= Time.deltaTime;
        if (opposite_sign(target_velocity.x, pc.velocity.x) ||
            Mathf.Abs(target_velocity.x) > Mathf.Abs(pc.velocity.x))
        {
            var velocity_step = pc.acceleration * Time.deltaTime;
            if (target_velocity.x < pc.velocity.x)
            {
                velocity_step *= -1;
            }

            pc.velocity.x += velocity_step;
        }

        if (opposite_sign(target_velocity.z, pc.velocity.z) ||
            Mathf.Abs(target_velocity.z) > Mathf.Abs(pc.velocity.z))
        {
            var velocity_step = pc.acceleration * Time.deltaTime;
            if (target_velocity.z < pc.velocity.z)
            {
                velocity_step *= -1;
            }

            pc.velocity.z += velocity_step;
        }
    }// calculate_new_pc_velocity

    //--------------------------------------------------------------------------

    private bool opposite_sign(float first, float second)
    {
        return first < 0 && second > 0 || first > 0 && second < 0;
    }// opposite_sign
}
