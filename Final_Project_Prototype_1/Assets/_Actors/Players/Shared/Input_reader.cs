using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Input_reader : MonoBehaviour
{
    public int player_id = 0;
    public static List<Input_reader> input_readers = new List<Input_reader>();

    public InputDevice input_device;

    private Player_character pc;
    private bool controls_enabled = true;

    private static UnityInputDevice[] keyboard_profiles =
        new UnityInputDevice[] {
            new UnityInputDevice(new Keyboard_controls.Keyboard_layout()),
            new UnityInputDevice(new Keyboard_controls.Keyboard_layout2())
        };

    void Awake()
    {
        // if (input_readers.Count > 2)
        // {
        //     return;
        // }

        input_readers.Add(this);

    }

    //--------------------------------------------------------------------------

    // Use this for initialization
    IEnumerator Start()
    {
        Player_character.controls_enabled = false;

        input_device = (InputManager.Devices.Count > player_id) ?
                       InputManager.Devices[player_id] : null;

        if (input_device == null)
        {
            print("player " + player_id +
                  " doesn't have a controller, assigning keyboard");
            input_device = keyboard_profiles[player_id];
            yield return new WaitForSeconds(1f);
            InputManager.AttachDevice(input_device);
        }

        pc = Player_character.player_characters[
                player_id].GetComponent<Player_character>();
        pc.input_device = input_device;

        Player_character.controls_enabled = true;
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // if (controls_enabled && input_device != null)
        // {
        //     process_input();
        // }
    }

    //--------------------------------------------------------------------------

    public static void toggle_player_controls(bool enable)
    {
        foreach (var input_reader in input_readers)
        {
            input_reader.controls_enabled = enable;
        }
    }// toggle_player_controls

    //--------------------------------------------------------------------------

    // private void process_input()
    // {
    //     if (input_device.GetControl(InputControlType.Action1).WasPressed)
    //     {
    //         pc.jump();
    //     }

    //     if (input_device.GetControl(InputControlType.Action2).WasPressed)
    //     {
    //         pc.team_up_engage_or_throw();
    //     }

    //     if (input_device.GetControl(InputControlType.Action3).WasPressed)
    //     {
    //         pc.attack();
    //     }

    //     if (input_device.GetControl(InputControlType.Action3).IsPressed)
    //     {
    //         pc.charge();
    //     }

    //     if (input_device.GetControl(InputControlType.Action3).WasReleased)
    //     {
    //         pc.stop_charge();
    //     }

    //     if(input_device.GetControl(InputControlType.LeftBumper).WasPressed ||
    //        input_device.GetControl(InputControlType.LeftBumper).WasReleased &&
    //        pc.is_locked_on)
    //     {
    //         pc.toggle_lock_on();
    //     }

    //     var tilt = Vector3.zero;
    //     tilt.x = input_device.GetControl(InputControlType.LeftStickX).Value;
    //     tilt.z = input_device.GetControl(InputControlType.LeftStickY).Value;

    //     var cam_right = Camera.main.transform.right;
    //     cam_right.y = 0;
    //     cam_right = cam_right.normalized;
    //     cam_right *= tilt.x;

    //     var cam_forward = Camera.main.transform.forward;
    //     cam_forward.y = 0;
    //     cam_forward = cam_forward.normalized;
    //     cam_forward *= tilt.z;

    //     var relative_move_dir = cam_right + cam_forward;

    //     var target_velocity = relative_move_dir * pc.run_speed;
    //     pc.update_movement_velocity(target_velocity);

    // }// process_input
}
