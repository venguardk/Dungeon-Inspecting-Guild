using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform cursorTransform; // Reference to the RectTransform of the cursor UI element
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float cursorSpeed = 1000f; // Speed of the cursor movement
    [SerializeField] private float padding = 35f;
    private Mouse virtualMouse, currentMouse;
    private bool previousMouseState, previousBButtonState;
    private Camera mainCamera;
    private string previousControlScheme = "";
    private const string gamepadScheme = "Gamepad", mouseScheme = "Keyboard&Mouse";

    private void OnEnable()
    {
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            // If the virtual mouse was removed, re-add it
            InputSystem.AddDevice("VirtualMouse");
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position); // Set the initial position of the virtual mouse to match the cursor
        }

        InputSystem.onAfterUpdate += UpdateMotion; //Calls the UpdateMotion function after each update
        playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        playerInput.onControlsChanged -= OnControlsChanged;
        InputSystem.onAfterUpdate -= UpdateMotion;

        if (virtualMouse != null && virtualMouse.added)
        {
            playerInput.user.UnpairDevice(virtualMouse);
            InputSystem.RemoveDevice(virtualMouse); // Remove the virtual mouse when the script is disabled
        }

        if (cursorTransform != null)
        {
            cursorTransform.gameObject.SetActive(false);
        }

        if (canvasRectTransform != null)
        {
            canvasRectTransform.anchoredPosition = Vector2.zero; // Reset the cursor position
        }

        cursorTransform = null; // Clear the reference to the cursor transform
        canvasRectTransform = null; // Clear the reference to the canvas rect transform
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null) return;

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding); // Clamp to screen width
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding); // Clamp to screen height

        InputState.Change(virtualMouse.position, newPosition); // Update the virtual mouse position
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.isPressed;
        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed; // Update the previous state
        }

        bool bButtonIsPressed = Gamepad.current.bButton.isPressed;
        if (previousBButtonState != bButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Right, bButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousBButtonState = bButtonIsPressed; // Update the previous state
        }

        AnchorCursor(newPosition); // Ensure the cursor stays within the screen bounds
    }

    private void AnchorCursor(Vector2 position)
    {
        if (cursorTransform == null || canvasRectTransform == null) return;
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition; // Update the cursor's anchored position
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false); // Hide the cursor when switching to mouse
            Cursor.visible = true; // Show the system cursor
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            cursorTransform.gameObject.SetActive(true); // Show the cursor when switching to gamepad
            Cursor.visible = false; // Hide the system cursor
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
        }
    }

}
