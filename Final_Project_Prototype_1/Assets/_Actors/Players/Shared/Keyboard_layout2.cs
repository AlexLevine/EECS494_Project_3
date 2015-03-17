using System;
using System.Collections;
using UnityEngine;
using InControl;


namespace Keyboard_controls
{
    // This custom profile is enabled by adding it to the Custom Profiles list
    // on the InControlManager component, or you can attach it yourself like so:
    // InputManager.AttachDevice( new UnityInputDevice( "Keyboard_layout" ) );
    //
    public class Keyboard_layout2 : UnityInputDeviceProfile
    {
        public Keyboard_layout2()
        {
            Name = "Keyboard2";
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
                    Handle = "AltJump",
                    Target = InputControlType.Action1,
                    Source = KeyCodeButton( KeyCode.X )
                },

                new InputControlMapping
                {
                    Handle = "AltProjectile",
                    Target = InputControlType.Action2,
                    Source = KeyCodeButton( KeyCode.Z )
                },

                new InputControlMapping
                {
                    Handle = "Alt Team up",
                    Target = InputControlType.Action3,
                    Source = KeyCodeButton( KeyCode.C )
                },

                new InputControlMapping
                {
                    Handle = "Alt Throw",
                    Target = InputControlType.Action4,
                    Source = KeyCodeButton( KeyCode.V )
                }
            };

            AnalogMappings = new[]
            {
                new InputControlMapping {
                    Handle = "Move X Alternate",
                    Target = InputControlType.LeftStickX,
                    Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
                },
                new InputControlMapping {
                    Handle = "Move Y Alternate",
                    Target = InputControlType.LeftStickY,
                    Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
                }//,
            };
        }
    }
}
