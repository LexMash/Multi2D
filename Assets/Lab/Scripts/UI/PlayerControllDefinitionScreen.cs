using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Multi2D
{
    public class PlayerControllDefinitionScreen : IDisposable
    {
        private const int IndefinitePlayerNumber = 0;
        private readonly Dictionary<InputDevice, IndefiniteInputWrapper> indefiniteMap;
        private readonly Dictionary<IndefiniteInputWrapper, PlayerInputEntryDefinition> definitionMap;
        private readonly PlayerControllDefinitionScreenView view;
        private readonly int minInputs;
        private readonly int maxInputs;
        private int definedInputs;

        public event Action OnAllPlayersDefined;

        public PlayerControllDefinitionScreen(PlayerControllDefinitionScreenView view, int minInputs, int maxInputs)
        {
            this.view = view;
            this.minInputs = minInputs;
            this.maxInputs = maxInputs;
            indefiniteMap = new Dictionary<InputDevice, IndefiniteInputWrapper>(minInputs);
            definitionMap = new Dictionary<IndefiniteInputWrapper, PlayerInputEntryDefinition>(minInputs);
        }

        public void Init()
        {
            view.BuildView(minInputs, maxInputs);

            ReadOnlyArray<InputDevice> devices = InputSystem.devices;

            for (int i = IndefinitePlayerNumber; i < devices.Count; i++)
            {
                InputDevice device = InputSystem.devices[i];
                AddDevice(device);
            }

            InputSystem.onDeviceChange += OnDeviceChanged;
        }

        public Dictionary<PlayerInputEntry, PlayerInputEntryDefinition> GetDefinedInputEntries()
        {
            return definitionMap.Where(kvp => kvp.Value.IsDefined).ToDictionary(kvp => kvp.Key.Entry, kvp => kvp.Value);
        }

        public void Dispose()
        {
            InputSystem.onDeviceChange -= OnDeviceChanged;

            foreach (var entry in indefiniteMap.Values)
                entry.Dispose();

            indefiniteMap.Clear();
            definitionMap.Clear();
        }

        private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added && indefiniteMap.Count < maxInputs)
            {
                AddDevice(device);
            }

            if (change == InputDeviceChange.Removed)
            {
                IndefiniteInputWrapper entry = indefiniteMap[device];
                entry.ChoiceDisable();
                indefiniteMap.Remove(device);
                definitionMap.Remove(entry);
                view.RemoveInputDevice(device.name);
            }
        }

        private void AddDevice(InputDevice device)
        {
            IndefiniteInputWrapper indefinite = new(new PlayerInputEntry(device), OnIndexChange, OnDone);
            indefiniteMap.Add(device, indefinite);
            PlayerInputEntryDefinition definition = new() { DeviceName = device.name, PlayerNumber = IndefinitePlayerNumber, IsDefined = false };
            view.AddInputDeviceDefinition(definition, device.GetInputType());
            indefinite.ChoiceEnable();
        }

        private void OnIndexChange(IndefiniteInputWrapper indefinite, int sign)
        {
            PlayerInputEntryDefinition definition = definitionMap[indefinite];

            if (definition.IsDefined)
            {
                Debug.Log($"Entry {indefinite.Entry.DeviceName} already defined. Player index {definition.PlayerNumber}");
                return;
            }

            definition.PlayerNumber = Mathf.Clamp(definition.PlayerNumber + sign, IndefinitePlayerNumber, minInputs);
            definitionMap[indefinite] = definition;
            view.UpdateDeviceDefinition(definition);
        }

        private void OnDone(IndefiniteInputWrapper indefinite)
        {
            PlayerInputEntryDefinition definition = definitionMap[indefinite];

            if (definition.PlayerNumber == IndefinitePlayerNumber)
                return;

            bool isDefined = definition.IsDefined;
            if (!isDefined)
            {
                foreach (var pcd in definitionMap.Values)
                {
                    if (!pcd.IsDefined)
                        continue;

                    if (pcd.Equals(definition))
                    {
                        //TODO some notification
                        Debug.Log($"Entry {indefinite.Entry.DeviceName} already defined. Player index {pcd.PlayerNumber}");
                        return;
                    }                 
                }

                definedInputs++;
            }
            else
            {
                definedInputs--;
            }

            definition.IsDefined = !isDefined;
            definitionMap[indefinite] = definition;
            view.UpdateDeviceDefinition(definition);

            if (definedInputs == minInputs)
            {
                foreach (var entry in indefiniteMap.Values)
                    entry.ChoiceDisable();

                OnAllPlayersDefined?.Invoke();
                Debug.Log("All player slots are defined.");
            }
        }

        private class IndefiniteInputWrapper : IDisposable
        {
            private readonly LocalMultiplayerInput.PlayerControllActions controll;
            private readonly Action<IndefiniteInputWrapper, int> onIndexChangeRequest;
            private readonly Action<IndefiniteInputWrapper> onDefine;
            public readonly PlayerInputEntry Entry;

            public IndefiniteInputWrapper(PlayerInputEntry entry, Action<IndefiniteInputWrapper, int> onIndexChangeRequest, Action<IndefiniteInputWrapper> onDefine)
            {
                Entry = entry;
                controll = entry.InputActions.PlayerControll;
                controll.Move.performed += ChoicePerformed;
                controll.Attack.performed += DonePerformed;
            }

            public void ChoiceEnable()
            {
                controll.Move.Enable();
                controll.Attack.Enable();
            }

            public void ChoiceDisable()
            {
                controll.Move.Disable();
                controll.Attack.Disable();
            } 

            public void Dispose()
            {
                controll.Move.performed -= ChoicePerformed;
                controll.Attack.performed -= DonePerformed;
            }

            private void ChoicePerformed(InputAction.CallbackContext context) => onIndexChangeRequest(this, (int)Mathf.Sign(context.ReadValue<Vector2>().x));
            private void DonePerformed(InputAction.CallbackContext context) => onDefine(this);
        }
    }
}
