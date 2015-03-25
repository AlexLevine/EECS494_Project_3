using UnityEngine;
using System.Collections;
// using Rewired;
using InControl;

public class Input_reader : MonoBehaviour
{
    public int player_id = 0;
    public static GameObject[] players;

    public InputDevice input_device;

    // private Player player; // The Rewired Player
    private Player_character pc;
    private bool controls_enabled = true;

    private static UnityInputDevice[] keyboard_profiles =
        new UnityInputDevice[] {
            new UnityInputDevice(new Keyboard_controls.Keyboard_layout()),
            new UnityInputDevice(new Keyboard_controls.Keyboard_layout2())
        };

    void Awake()
    {
        // player = ReInput.players.GetPlayer(player_id);
    }

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        input_device = (InputManager.Devices.Count > player_id) ?
                       InputManager.Devices[player_id] : null;

        if (input_device == null)
        {
            print("player " + player_id +
                  " doesn't have a controller, assigning keyboard");
            input_device = keyboard_profiles[player_id];
            InputManager.AttachDevice(input_device);
        }

        pc = Player_character.player_characters[
                player_id].GetComponent<Player_character>();
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (controls_enabled && input_device != null)
        {
            process_input();
        }
    }

    //--------------------------------------------------------------------------

    public void toggle_player_controls()
    {
        controls_enabled = !controls_enabled;
    }// toggle_player_controls

    //--------------------------------------------------------------------------

    private void process_input()
    {
        if (input_device.GetControl(InputControlType.Action1).WasPressed)
        {
            pc.jump();
        }

        if (input_device.GetControl(InputControlType.Action2).WasPressed)
        {
            print("team up");
            pc.team_up_engage_or_throw();
        }

        if (input_device.GetControl(InputControlType.Action3).WasPressed)
        {
            pc.attack();
        }

        if(input_device.GetControl(InputControlType.LeftBumper).WasPressed ||
           input_device.GetControl(InputControlType.LeftBumper).WasReleased &&
           pc.is_locked_on)
        {
            pc.toggle_lock_on();
        }

        // if (input_device.GetControl(InputControlType.RightBumper).WasPressed)
        // {
        //     pc.charge();
        // }

        Vector3 tilt = Vector3.zero;
        tilt.x = input_device.GetControl(InputControlType.LeftStickX).Value;
        tilt.z = input_device.GetControl(InputControlType.LeftStickY).Value;
        // tilt = tilt.normalized;

        pc.adjust_jousting_pole(tilt.z, tilt.x);

        // var sprint = player.GetButton("sprint");

        var target_velocity = tilt * pc.run_speed;
        pc.update_movement_velocity(target_velocity);

        // move(delta_position);
    }// process_input
}
