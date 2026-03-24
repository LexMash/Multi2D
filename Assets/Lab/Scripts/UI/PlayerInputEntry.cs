using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Multi2D
{
    public class PlayerInputEntry
    {
        private InputDevice inputDevice; 
        private InputUser inputUser;
        public readonly LocalMultiplayerInput InputActions = new();
        public string DeviceName => inputDevice.name;

        public PlayerInputEntry(InputDevice device)
        {
            inputDevice = device;
            inputUser = InputUser.PerformPairingWithDevice(device);
            inputUser.AssociateActionsWithUser(InputActions);
        }

        public void UpdateDevice(InputDevice device)
        {
            inputDevice = device;
            inputUser = InputUser.PerformPairingWithDevice(device);       
        }
    }
}
