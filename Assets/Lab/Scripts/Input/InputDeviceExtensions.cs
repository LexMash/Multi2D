using UnityEngine.InputSystem;

namespace Multi2D
{
    public static class InputDeviceExtensions
    {
        public static InputDeviceType GetInputType(this InputDevice device)
        {
            return device switch
            {
                Keyboard => InputDeviceType.Keyboard,
                Gamepad => InputDeviceType.Gamepad,
                Joystick => InputDeviceType.Joystick,
                _ => InputDeviceType.Unknown,
            };
        }
    }
}
