using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Title_screen_input_reader : MonoBehaviour
{
    // public int player_id = 0;

    // public InputDevice input_device = null;

    // public GameObject title_controller;

    // // Use this for initialization
    // void Start()
    // {
    //     get_input_device();

    // }

    // //--------------------------------------------------------------------------

    // // Update is called once per frame
    // void Update()
    // {
    //     if (input_device == null)
    //     {
    //         get_input_device();
    //     }

    //     if (input_device.GetControl(InputControlType.Action1).WasPressed)
    //     {
    //         if(player_id == 0)
    //         {
    //             print("0");
    //             title_controller.GetComponent<Title_screen_controller>().setLlama();
    //         }

    //         if(player_id == 1)
    //         {
    //             print("1");
    //             title_controller.GetComponent<Title_screen_controller>().setNinja();
    //         }

    //         Destroy(gameObject);
    //     }
    // }

    // // void get_input_device()
    // // {
    // //     if(player_id == 0)
    // //     {
    // //         input_device = Llama.get().GetComponent<Input_reader>().input_device;
    // //     }
    // //     else
    // //     {
    // //         input_device = Ninja.get().GetComponent<Input_reader>().input_device;
    // //     }
    // // }
}
