using Multi2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class LocalInputTest : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    private InputUser inputUser1;
    private InputUser inputUser2;

    private PlayerInputEntry playerEntry;

    private LocalMultiplayerInput input1;
    private LocalMultiplayerInput input2;

    private void Start() 
    {
        input1 = new LocalMultiplayerInput();
        
        inputUser1 = InputUser.PerformPairingWithDevice(Keyboard.current, inputUser1);
        inputUser1.AssociateActionsWithUser(input1);

        input1.Enable();
        input1.PlayerControll.Enable();
        input1.PlayerControll.Move.performed += ctx => 
        { 
            player1.position += 5 * Time.deltaTime * (Vector3)ctx.ReadValue<Vector2>();
            Debug.Log($"User with id {inputUser1.id} performed move {ctx.time}, {ctx.ReadValue<Vector2>()}");
        };

        input2 = new LocalMultiplayerInput();
        inputUser2 = InputUser.CreateUserWithoutPairedDevices();
        inputUser2.AssociateActionsWithUser(input2);

        input2.Enable();
        input2.PlayerControll.Enable();
        input2.PlayerControll.Move.performed += ctx => 
        { 
            player2.position += 5 * Time.deltaTime * (Vector3)ctx.ReadValue<Vector2>();
            Debug.Log($"User with id {inputUser2.id} performed move {ctx.time}, {ctx.ReadValue<Vector2>()}");
        };

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added)
        {
            Debug.Log($"Device {device.name} added");
            inputUser2 = InputUser.PerformPairingWithDevice(device, inputUser2, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        }
    }
}