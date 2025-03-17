using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Multi2D
{
    //[DefaultExecutionOrder(-5000)]
    public class DeviceListener : MonoBehaviour
    {
        private List<LocalMultiplayerInput> inputs = new List<LocalMultiplayerInput>();

        private InputUser inputUser1;
        private InputUser inputUser2;

        private void Start()
        {
            InputSystem.onDeviceChange += OnDeviceChanged;

            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                InputDevice device = InputSystem.devices[i];

                Debug.Log($"Registerd device - {device.name}");
            }

            var allUsers = InputUser.all; //after start - this is empty

            for (int i = 0; i < allUsers.Count; i++) 
            {
                var user = allUsers[i];
                Debug.Log($"User registered {user.id}");
                foreach (var inputDevice in user.pairedDevices)
                {
                    Debug.Log($"{user.id} has paired device - {inputDevice.name}");
                }
            }
        }

        private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            //Debug.Log($"Device {device.name} changed {change}");

            //if(change == InputDeviceChange.Added)
            //{
            //    LocalMultiplayerInput newInput = new LocalMultiplayerInput();
            //    inputUser2 = InputUser.PerformPairingWithDevice(device);
            //    inputUser2.AssociateActionsWithUser(newInput);

            //    foreach (var inputDevice in inputUser2.pairedDevices)
            //    {
            //        Debug.Log($"{inputUser2.id} has paired device - {inputDevice.name}");
            //    }

            //    inputs.Add(newInput);
            //    newInput.Enable();
            //    newInput.Player1.Enable();
            //    newInput.Player1.Interact.Enable();
            //    newInput.Player1.Interact.performed += ctx => Debug.Log($"User with id {inputUser2.id} performed interation {ctx.time}");
            //}

            //if(change == InputDeviceChange.Enabled && device.name == "Keyboard")
            //{
            //    inputUser1 = InputUser.PerformPairingWithDevice(device);
            //    LocalMultiplayerInput newInput = new LocalMultiplayerInput();
            //    inputUser1 = InputUser.PerformPairingWithDevice(device);
            //    inputUser1.AssociateActionsWithUser(newInput);

            //    foreach (var inputDevice in inputUser1.pairedDevices)
            //    {
            //        Debug.Log($"{inputUser2.id} has paired device - {inputDevice.name}");
            //    }

            //    inputs.Add(newInput);
            //    newInput.Enable();
            //    newInput.Player0.Enable();
            //    newInput.Player0.Interact.Enable();
            //    newInput.Player0.Interact.performed += ctx => Debug.Log($"User with id {inputUser1.id} performed interation {ctx.time}");
            //}
        }

        private void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChanged;
        }
    }
}
