using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Title_screen_input_reader : MonoBehaviour
{
    public int player_id = 0;
    public static List<Input_reader> input_readers = new List<Input_reader>();

    public InputDevice input_device;

    public GameObject title_controller; 

    private static UnityInputDevice[] keyboard_profiles =
        new UnityInputDevice[] {
            new UnityInputDevice(new Keyboard_controls.Keyboard_layout()),
            new UnityInputDevice(new Keyboard_controls.Keyboard_layout2())
        };


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
    }

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (input_device == null)
        {
            return; 
        }

        if (input_device.GetControl(InputControlType.Action1).WasPressed)
        {
            if(player_id == 0)
            {
                title_controller.GetComponent<Title_screen_controller>().setLlama(); 
            }

            if(player_id == 1)
            {
                title_controller.GetComponent<Title_screen_controller>().setNinja(); 
            }
            // Do a thing
        }
    }
}
