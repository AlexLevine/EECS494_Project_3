using UnityEngine;
using System.Collections;

public class Team_up_animation : MonoBehaviour
{
    public enum Team_up_animation_state_e
    {
        NOT_PLAYING,
        JUMPING,
        DIVING
    }

    // public bool is_playing { get { return state != NOT_PLAYING; } }

    private Team_up_animation_state_e state;

    // float time_since_start = 0;
    float duration = 0.1f;

    private float horizontal_speed = 30f;

    private float jump_height;
    private float vertical_velocity = 0;
    private float fake_gravity = -40f;

    public void start_animation()
    {
        state = Team_up_animation_state_e.JUMPING;

        // pause things
        Actor.actors_paused = true;
        // Time.timeScale = 0;
        Input_reader.toggle_player_controls(false);

        var current_pos = transform.position;
        var target_pos = Llama.get().team_up_point.transform.position;

        // duration = Mathf.Max(
        //     1f, Vector3.Distance(current_pos, target_pos) / horizontal_speed);
        // print("duration: " + duration);

        jump_height = Mathf.Max(0, target_pos.y + 2f - current_pos.y);
        // print("jump_height: " + jump_height);

        // var ascending_duration = duration / 2;
        vertical_velocity = (
            jump_height - (
                0.5f * fake_gravity * (duration * duration))
            ) / duration;
        if (vertical_velocity > 18)
        {
            vertical_velocity = 18;
        }
        // print("vertical_velocity: " + vertical_velocity);
    }

    //--------------------------------------------------------------------------

    void Update()
    {
        switch (state)
        {
        case Team_up_animation_state_e.NOT_PLAYING:
            break;

        case Team_up_animation_state_e.JUMPING:
            step_jump();
            break;

        case Team_up_animation_state_e.DIVING:
            step_dive();
            break;
        }
    }

    //--------------------------------------------------------------------------

    void step_jump()
    {
        var y_step = vertical_velocity * Time.deltaTime;

        var new_pos = transform.position;
        new_pos.y += y_step;
        transform.position = new_pos;

        vertical_velocity += fake_gravity * Time.deltaTime;
        if (vertical_velocity < 0)
        {
            state = Team_up_animation_state_e.DIVING;
        }

    }

    //--------------------------------------------------------------------------

    void step_dive()
    {
        var current_pos = transform.position;
        var target_pos = Llama.get().team_up_point.transform.position;

        var new_pos = Vector3.MoveTowards(
            current_pos, target_pos, horizontal_speed * Time.deltaTime);

        transform.position = new_pos;

        var distance_left = Vector3.Distance(
            transform.position, Llama.get().team_up_point.transform.position);
        if (distance_left < 0.1f)
        {
            state = Team_up_animation_state_e.NOT_PLAYING;

            Actor.actors_paused = false;
            // Time.timeScale = 1;
            Input_reader.toggle_player_controls(true);
        }
    }

}
