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
            this_actor.elemental_attack();
        }

        if (device.GetControl(InputControlType.Action3).WasPressed)
        {
            this_actor.team_up_engage();
        }

        if (device.GetControl(InputControlType.Action4).WasPressed)
        {
            this_actor.throw_ninja();
        }

        var vertical_tilt = device.GetControl( InputControlType.LeftStickY);
        var horiz_tilt = device.GetControl( InputControlType.LeftStickX);
        var tilt = new Vector3(horiz_tilt, 0, vertical_tilt);
        this_actor.run(tilt);

    }
}
