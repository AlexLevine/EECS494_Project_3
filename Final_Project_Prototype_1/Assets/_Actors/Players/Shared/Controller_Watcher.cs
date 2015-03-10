using UnityEngine;
using System.Collections;
using InControl;

public class Controller_Watcher : MonoBehaviour {
    public InputDevice device{get; set;}

    Player_character this_actor;

    void Start()
    {
        this_actor = GetComponent<Player_character>();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if(device == null)
        {
            return;
        }

        if (device.GetControl(InputControlType.Action1).WasPressed)
        {
            this_actor.jump();
        }

        if (device.GetControl(InputControlType.Action2).WasPressed)
        {
            this_actor.projectile_attack();
        }

        if (device.GetControl(InputControlType.Action3).WasPressed)
        {
            this_actor.team_up_engage_or_throw();
        }

        if (device.GetControl(InputControlType.Action4).WasPressed)
        {
            this_actor.physical_attack();
        }

        if(device.GetControl(InputControlType.LeftBumper).IsPressed)
        {
            float r_vert = device.GetControl(InputControlType.RightStickY);
            float r_horz = device.GetControl(InputControlType.RightStickX);

            this_actor.team_up_attack(r_vert, r_horz);
        }


        bool sprint = false;
        if(device.GetControl(InputControlType.RightBumper).IsPressed)
        {
            sprint = true;
        }

        var vertical_tilt = device.GetControl( InputControlType.LeftStickY);
        var horiz_tilt = device.GetControl( InputControlType.LeftStickX);
        var tilt = new Vector3(horiz_tilt, 0, vertical_tilt);
        this_actor.run(tilt, sprint);

    }
}
