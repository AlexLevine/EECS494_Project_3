using UnityEngine;
using System;
using InControl;

public class Pause_screen_controller : MonoBehaviour {

    private static Pause_screen_controller instance; 

    public static Pause_screen_controller get()
    {
        return instance;
    }// get


    // public GameObject start_screen; 

    void Awake()
    {
        instance = this; 
    }

    // Use this for initialization
    void Start () 
    {
        gameObject.SetActive(false);
    }


    void Update()
    {
        // if(start_screen != null && start_screen.activeSelf)
        // {
        //     return; 
        // }

        if (Llama.get().input_device.GetControl(InputControlType.Start).WasPressed || 
            Ninja.get().input_device.GetControl(InputControlType.Start).WasPressed)
        {
            // Pause Screen is visible
            if(Pause_screen_controller.get().gameObject.activeSelf)
            {
                Actor.actors_paused = false; 
                // Pause_screen_controller.get().gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            
        }

    
        if (Llama.get().input_device.GetControl(InputControlType.Back).WasPressed || 
            Ninja.get().input_device.GetControl(InputControlType.Back).WasPressed)        {
            if(Pause_screen_controller.get().gameObject.activeSelf)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
