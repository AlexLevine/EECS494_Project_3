using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Throw_animation_state_e
{
    NOT_PLAYING,
    REARING_UP,
    // THROW,
    PAUSE,
    COOLING_DOWN
}

public class Throw_animation : MonoBehaviour
{
    public GameObject rotation_axis_pos;
    public bool is_playing {
        get { return state != Throw_animation_state_e.NOT_PLAYING; } }

    private Throw_animation_state_e state;
    private float amount_rotated = 0;

    private static float rotation_speed = 180f;
    // private static float starting_rotation_speed = 10;
    private static float max_rotation = 30f;
    private float throw_speed = 15f;

    private static float pause_duration = 0.2f;
    private float pause_time_elapsed = 0f;

    private Vector3 original_rotation;

    // Use this for initialization
    public void start_animation()
    {
        if (is_playing)
        {
            return;
        }

        // disable controls

        original_rotation = transform.rotation.eulerAngles;
        state = Throw_animation_state_e.REARING_UP;

        if (Llama.get().is_teamed_up)
        {
            // print("teamed up: " + transform.up);
            Llama.get().team_up_disengage();
            // print(transform.up + transform.forward);
            // Ninja.get().on_thrown();
            Ninja.get().apply_momentum(
                (transform.up * 1f + transform.forward) * throw_speed);

            // re-enable ninja controls
        }
    }// start_animation

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
        case Throw_animation_state_e.NOT_PLAYING:
            break;

        case Throw_animation_state_e.REARING_UP:

            var rotation_step = rotation_speed * Time.deltaTime;
            transform.RotateAround(
                rotation_axis_pos.transform.position,
                transform.right, rotation_step);

            amount_rotated += rotation_step;
            if (amount_rotated >= max_rotation)
            {
                state = Throw_animation_state_e.PAUSE;
            }
            break;

        // case Throw_animation_state_e.THROW:
        //     if (Llama.get().is_teamed_up)
        //     {
        //         print("teamed up: " + transform.up);
        //         Ninja.get().velocity = Vector3.up * throw_speed;
        //         Llama.get().team_up_disengage();
        //         // re-enable ninja controls
        //     }

        //     state = Throw_animation_state_e.PAUSE;
        //     break;

        case Throw_animation_state_e.PAUSE:
            if (pause_time_elapsed >= pause_duration)
            {
                state = Throw_animation_state_e.COOLING_DOWN;
                pause_time_elapsed = 0;
            }

            pause_time_elapsed += Time.deltaTime;
            break;

        case Throw_animation_state_e.COOLING_DOWN:

            var step = rotation_speed * Time.deltaTime;
            if (amount_rotated - step < 0)
            {
                step = amount_rotated;
            }

            amount_rotated -= step;
            if (amount_rotated <= .001f)
            {
                transform.rotation = Quaternion.Euler(original_rotation);
                state = Throw_animation_state_e.NOT_PLAYING;
                amount_rotated = 0;
                break;
                // re-enable llama controls
            }

            transform.RotateAround(
                rotation_axis_pos.transform.position, transform.right, -step);

            break;
        }
    }// Update
}
