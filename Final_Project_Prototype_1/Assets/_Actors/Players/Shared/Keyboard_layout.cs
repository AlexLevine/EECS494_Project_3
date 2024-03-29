﻿using System;
using System.Collections;
using UnityEngine;
using InControl;


namespace Keyboard_controls
{
    // This custom profile is enabled by adding it to the Custom Profiles list
    // on the InControlManager component, or you can attach it yourself like so:
    // InputManager.AttachDevice( new UnityInputDevice( "Keyboard_layout" ) );
    //
    public class Keyboard_layout : UnityInputDeviceProfile
    {
        public Keyboard_layout()
        {
            Name = "Keyboard";
            Meta = "A keyboard profile.";

            // This profile only works on desktops.
            SupportedPlatforms = new[]
            {
                "Windows",
                "Mac",
                "Linux"
            };

            Sensitivity = 1.0f;
            LowerDeadZone = 0.0f;
            UpperDeadZone = 1.0f;

            ButtonMappings = new[]
            {
                new InputControlMapping
                {
                    Handle = "Jump",
                    Target = InputControlType.Action1,
                    Source = KeyCodeButton( KeyCode.Alpha1 )
                },

                new InputControlMapping
                {
                    Handle = "Attack",
                    Target = InputControlType.Action3,
                    Source = KeyCodeButton( KeyCode.Alpha2 )
                },

                new InputControlMapping
                {
                    Handle = "Team up or throw",
                    Target = InputControlType.Action2,
                    Source = KeyCodeButton( KeyCode.Alpha3 )
                },

                new InputControlMapping
                {
                    Handle = "Charge",
                    Target = InputControlType.RightBumper,
                    Source = KeyCodeButton( KeyCode.Alpha4 )
                },

                new InputControlMapping
                {
                    Handle = "Lock on",
                    Target = InputControlType.LeftBumper,
                    Source = KeyCodeButton( KeyCode.LeftShift )
                }
            };

            AnalogMappings = new[]
            {
                new InputControlMapping
                {
                    Handle = "Move X",
                    Target = InputControlType.LeftStickX,
                    // KeyCodeAxis splits the two KeyCodes over an axis. The first is negative, the second positive.
                    Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
                },
                new InputControlMapping
                {
                    Handle = "Move Y",
                    Target = InputControlType.LeftStickY,
                    // Notes that up is positive in Unity, therefore the order of KeyCodes is down, up.
                    Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
                }
            };
        }
    }
}
