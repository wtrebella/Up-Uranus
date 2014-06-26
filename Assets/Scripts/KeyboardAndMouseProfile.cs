using System;
using System.Collections;
using UnityEngine;
using InControl;

public class KeyboardAndMouseProfile : UnityInputDeviceProfile
{
	public KeyboardAndMouseProfile()
	{
		Name = "Keyboard/Mouse";
		Meta = "A keyboard and mouse combination profile appropriate for Up Uranus.";
		
		// This profile only works on desktops.
		SupportedPlatforms = new[]
		{
			"Windows",
			"Mac",
			"Linux"
		};
		
		Sensitivity = 1.0f;
		LowerDeadZone = 0.9f;
		UpperDeadZone = 1.0f;
		
		ButtonMappings = new[]
		{
			new InputControlMapping
			{
				Handle = "Jump - Mouse",
				Target = InputControlType.Action1,
				Source = MouseButton0
			},
			new InputControlMapping
			{
				Handle = "Jump - Keyboard",
				Target = InputControlType.Action1,
				Source = KeyCodeButton( KeyCode.Space )
			},
		};
		
		AnalogMappings = new[]
		{
			new InputControlMapping
			{
				Handle = "Move X",
				Target = InputControlType.LeftStickX,
				Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
			},
			new InputControlMapping
			{
				Handle = "Move Y",
				Target = InputControlType.LeftStickY,
				Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
			},
			new InputControlMapping {
				Handle = "Move X Alternate",
				Target = InputControlType.LeftStickX,
				Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
			},
			new InputControlMapping {
				Handle = "Move Y Alternate",
				Target = InputControlType.LeftStickY,
				Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
			}
		};
	}
}

