using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Engine.Input;

/// <summary>
/// A Godot-like input service that provides action-based input handling
/// </summary>
public static class InputService
{
    private static readonly Dictionary<string, InputAction> inputMap = new();
    private static KeyboardState currentKeyboardState;
    private static KeyboardState previousKeyboardState;
    private static MouseState currentMouseState;
    private static MouseState previousMouseState;
    private static GamePadState currentGamePadState;
    private static GamePadState previousGamePadState;
    private static PlayerIndex activeGamePadPlayer = PlayerIndex.One;

    /// <summary>
    /// Initialize the input service - should be called once during game initialization
    /// </summary>
    public static void Initialize()
    {
        // Initialize states
        currentKeyboardState = Keyboard.GetState();
        previousKeyboardState = currentKeyboardState;
        currentMouseState = Mouse.GetState();
        previousMouseState = currentMouseState;
        currentGamePadState = GamePad.GetState(activeGamePadPlayer);
        previousGamePadState = currentGamePadState;
    }

    /// <summary>
    /// Update input states - should be called every frame before checking input
    /// </summary>
    public static void Update()
    {
        // Store previous states
        previousKeyboardState = currentKeyboardState;
        previousMouseState = currentMouseState;
        previousGamePadState = currentGamePadState;

        // Get current states
        currentKeyboardState = Keyboard.GetState();
        currentMouseState = Mouse.GetState();
        currentGamePadState = GamePad.GetState(activeGamePadPlayer);
    }

    /// <summary>
    /// Add an input action to the input map
    /// </summary>
    public static void AddAction(InputAction action)
    {
        inputMap[action.Name] = action;
    }

    /// <summary>
    /// Add an input action with the specified name
    /// </summary>
    public static InputAction AddAction(string actionName)
    {
        var action = new InputAction(actionName);
        inputMap[actionName] = action;
        return action;
    }

    /// <summary>
    /// Remove an action from the input map
    /// </summary>
    public static void RemoveAction(string actionName)
    {
        inputMap.Remove(actionName);
    }

    /// <summary>
    /// Check if an action is currently pressed (held down)
    /// </summary>
    public static bool IsActionPressed(string actionName)
    {
        if (!inputMap.TryGetValue(actionName, out var action))
            return false;

        // Check keyboard keys
        if (action.Keys.Any(key => currentKeyboardState.IsKeyDown(key)))
            return true;

        // Check gamepad buttons
        if (action.GamepadButtons.Any(button => currentGamePadState.IsButtonDown(button)))
            return true;

        // Check mouse buttons
        if (action.MouseButtons.Any(button => IsMouseButtonDown(button, currentMouseState)))
            return true;

        return false;
    }

    /// <summary>
    /// Check if an action was just pressed this frame
    /// </summary>
    public static bool IsActionJustPressed(string actionName)
    {
        if (!inputMap.TryGetValue(actionName, out var action))
            return false;

        // Check keyboard keys
        foreach (var key in action.Keys)
        {
            if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                return true;
        }

        // Check gamepad buttons
        foreach (var button in action.GamepadButtons)
        {
            if (currentGamePadState.IsButtonDown(button) && !previousGamePadState.IsButtonDown(button))
                return true;
        }

        // Check mouse buttons
        foreach (var button in action.MouseButtons)
        {
            if (IsMouseButtonDown(button, currentMouseState) && !IsMouseButtonDown(button, previousMouseState))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Check if an action was just released this frame
    /// </summary>
    public static bool IsActionJustReleased(string actionName)
    {
        if (!inputMap.TryGetValue(actionName, out var action))
            return false;

        // Check keyboard keys
        foreach (var key in action.Keys)
        {
            if (!currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key))
                return true;
        }

        // Check gamepad buttons
        foreach (var button in action.GamepadButtons)
        {
            if (!currentGamePadState.IsButtonDown(button) && previousGamePadState.IsButtonDown(button))
                return true;
        }

        // Check mouse buttons
        foreach (var button in action.MouseButtons)
        {
            if (!IsMouseButtonDown(button, currentMouseState) && IsMouseButtonDown(button, previousMouseState))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Get the strength of an action (0.0 to 1.0). Useful for analog inputs like gamepad triggers.
    /// </summary>
    public static float GetActionStrength(string actionName)
    {
        if (!inputMap.TryGetValue(actionName, out var action))
            return 0f;

        float maxStrength = 0f;

        // Keyboard and mouse buttons are digital (0 or 1)
        if (action.Keys.Any(key => currentKeyboardState.IsKeyDown(key)) ||
            action.MouseButtons.Any(button => IsMouseButtonDown(button, currentMouseState)))
        {
            maxStrength = 1f;
        }

        // Check gamepad buttons and triggers for analog values
        foreach (var button in action.GamepadButtons)
        {
            float strength = GetGamepadButtonStrength(button);
            if (strength > maxStrength)
                maxStrength = strength;
        }

        return maxStrength;
    }

    /// <summary>
    /// Get a Vector2 representing input for movement (e.g., WASD or gamepad stick)
    /// </summary>
    public static Vector2 GetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadZone = 0.5f)
    {
        var vector = new Vector2(
            (IsActionPressed(positiveX) ? 1f : 0f) - (IsActionPressed(negativeX) ? 1f : 0f),
            (IsActionPressed(positiveY) ? 1f : 0f) - (IsActionPressed(negativeY) ? 1f : 0f)
        );

        // Apply deadzone
        if (vector.Length() < deadZone)
            return Vector2.Zero;

        return vector.Length() > 1f ? Vector2.Normalize(vector) : vector;
    }

    /// <summary>
    /// Get gamepad left stick as Vector2
    /// </summary>
    public static Vector2 GetGamepadLeftStick(float deadZone = 0.1f)
    {
        var stick = currentGamePadState.ThumbSticks.Left;
        stick.Y = -stick.Y; // Invert Y to match screen coordinates

        if (stick.Length() < deadZone)
            return Vector2.Zero;

        return stick;
    }

    /// <summary>
    /// Get gamepad right stick as Vector2
    /// </summary>
    public static Vector2 GetGamepadRightStick(float deadZone = 0.1f)
    {
        var stick = currentGamePadState.ThumbSticks.Right;
        stick.Y = -stick.Y; // Invert Y to match screen coordinates

        if (stick.Length() < deadZone)
            return Vector2.Zero;

        return stick;
    }

    /// <summary>
    /// Get mouse position as Vector2
    /// </summary>
    public static Vector2 GetMousePosition()
    {
        return new Vector2(currentMouseState.X, currentMouseState.Y);
    }

    /// <summary>
    /// Get mouse position delta since last frame
    /// </summary>
    public static Vector2 GetMouseDelta()
    {
        return new Vector2(
            currentMouseState.X - previousMouseState.X,
            currentMouseState.Y - previousMouseState.Y
        );
    }

    /// <summary>
    /// Get mouse scroll wheel delta
    /// </summary>
    public static int GetMouseScrollDelta()
    {
        return currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
    }

    /// <summary>
    /// Set which gamepad player index to use
    /// </summary>
    public static void SetActiveGamePadPlayer(PlayerIndex playerIndex)
    {
        activeGamePadPlayer = playerIndex;
    }

    /// <summary>
    /// Clear all input actions
    /// </summary>
    public static void ClearActions()
    {
        inputMap.Clear();
    }

    /// <summary>
    /// Get all registered action names
    /// </summary>
    public static IEnumerable<string> GetActionNames()
    {
        return inputMap.Keys;
    }

    /// <summary>
    /// Check if an action exists in the input map
    /// </summary>
    public static bool HasAction(string actionName)
    {
        return inputMap.ContainsKey(actionName);
    }

    // Helper methods
    private static bool IsMouseButtonDown(MouseButton button, MouseState mouseState)
    {
        return button switch
        {
            MouseButton.Left => mouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Right => mouseState.RightButton == ButtonState.Pressed,
            MouseButton.Middle => mouseState.MiddleButton == ButtonState.Pressed,
            MouseButton.XButton1 => mouseState.XButton1 == ButtonState.Pressed,
            MouseButton.XButton2 => mouseState.XButton2 == ButtonState.Pressed,
            _ => false
        };
    }

    private static float GetGamepadButtonStrength(Buttons button)
    {
        return button switch
        {
            Buttons.LeftTrigger => currentGamePadState.Triggers.Left,
            Buttons.RightTrigger => currentGamePadState.Triggers.Right,
            _ => currentGamePadState.IsButtonDown(button) ? 1f : 0f
        };
    }
}
