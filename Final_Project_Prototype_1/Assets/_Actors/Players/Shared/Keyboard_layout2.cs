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
                    Handle = "Jump",
                    Target = InputControlType.Action1,
                    Source = KeyCodeButton( KeyCode.Alpha7 )
                },

                new InputControlMapping
                {
                    Handle = "Attack",
                    Target = InputControlType.Action3,
                    Source = KeyCodeButton( KeyCode.Alpha8 )
                },

                new InputControlMapping
                {
                    Handle = "Team up or throw",
                    Target = InputControlType.Action2,
                    Source = KeyCodeButton( KeyCode.Alpha9 )
                },

                new InputControlMapping
                {
                    Handle = "Charge",
                    Target = InputControlType.RightBumper,
                    Source = KeyCodeButton( KeyCode.Alpha0 )
                },

                new InputControlMapping
                {
                    Handle = "Lock on",
                    Target = InputControlType.LeftBumper,
                    Source = KeyCodeButton( KeyCode.Space )
                }
            };

            AnalogMappings = new[]
            {
                new InputControlMapping {
                    Handle = "Move X",
                    Target = InputControlType.LeftStickX,
                    Source = KeyCodeAxis( KeyCode.J, KeyCode.L )
                },
                new InputControlMapping {
                    Handle = "Move Y",
                    Target = InputControlType.LeftStickY,
                    Source = KeyCodeAxis( KeyCode.K, KeyCode.I )
                }//,
            };
        }
    }
}
