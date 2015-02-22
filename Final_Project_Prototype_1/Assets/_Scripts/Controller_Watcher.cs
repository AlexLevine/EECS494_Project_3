using UnityEngine;
using System.Collections;
using InControl;

public class Controller_Watcher : MonoBehaviour {
    public InputDevice device{get; set;}

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update () 
    {
        if(device == null)
        {
            return; 
        }


        if (device.GetControl( InputControlType.Action1 ))
        {
            rigidbody.AddForce(Vector3.up * 10);
        }
        var horiz_movement = device.GetControl( InputControlType.LeftStickX);
        if (horiz_movement < 0)
        {
            rigidbody.AddForce(Vector3.left * 10);
        }
        if (horiz_movement > 0)
        {
            rigidbody.AddForce(Vector3.right * 10);
        }
    }
}
