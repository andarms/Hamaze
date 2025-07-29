using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Engine.Input;

/// <summary>
/// Helper class for configuring input maps with fluent API
/// </summary>
public class InputMap
{
    private readonly Dictionary<string, InputAction> actions = new();

    /// <summary>
    /// Create a new input action and add it to the map
    /// </summary>
    public InputMap AddAction(string actionName)
    {
        actions[actionName] = new InputAction(actionName);
        return this;
    }

    /// <summary>
    /// Add a key binding to the last created action
    /// </summary>
    public InputMap WithKey(Keys key)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            lastAction?.AddKey(key);
        }
        return this;
    }

    /// <summary>
    /// Add multiple key bindings to the last created action
    /// </summary>
    public InputMap WithKeys(params Keys[] keys)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            if (lastAction != null)
            {
                foreach (var key in keys)
                {
                    lastAction.AddKey(key);
                }
            }
        }
        return this;
    }

    /// <summary>
    /// Add a gamepad button binding to the last created action
    /// </summary>
    public InputMap WithGamepadButton(Buttons button)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            lastAction?.AddGamepadButton(button);
        }
        return this;
    }

    /// <summary>
    /// Add multiple gamepad button bindings to the last created action
    /// </summary>
    public InputMap WithGamepadButtons(params Buttons[] buttons)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            if (lastAction != null)
            {
                foreach (var button in buttons)
                {
                    lastAction.AddGamepadButton(button);
                }
            }
        }
        return this;
    }

    /// <summary>
    /// Add a mouse button binding to the last created action
    /// </summary>
    public InputMap WithMouseButton(MouseButton button)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            lastAction?.AddMouseButton(button);
        }
        return this;
    }

    /// <summary>
    /// Add multiple mouse button bindings to the last created action
    /// </summary>
    public InputMap WithMouseButtons(params MouseButton[] buttons)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            if (lastAction != null)
            {
                foreach (var button in buttons)
                {
                    lastAction.AddMouseButton(button);
                }
            }
        }
        return this;
    }

    /// <summary>
    /// Set dead zone for the last created action
    /// </summary>
    public InputMap WithDeadZone(float deadZone)
    {
        if (actions.Count > 0)
        {
            var lastAction = GetLastAction();
            lastAction?.SetDeadZone(deadZone);
        }
        return this;
    }

    /// <summary>
    /// Apply this input map to the InputService
    /// </summary>
    public void Apply()
    {
        foreach (var action in actions.Values)
        {
            InputManager.AddAction(action);
        }
    }

    /// <summary>
    /// Create a default input map with common game actions
    /// </summary>
    public static InputMap CreateDefault()
    {
        return new InputMap()
            // Movement actions
            .AddAction("move_up")
                .WithKeys(Keys.W, Keys.Up)
                .WithGamepadButton(Buttons.DPadUp)
            .AddAction("move_down")
                .WithKeys(Keys.S, Keys.Down)
                .WithGamepadButton(Buttons.DPadDown)
            .AddAction("move_left")
                .WithKeys(Keys.A, Keys.Left)
                .WithGamepadButton(Buttons.DPadLeft)
            .AddAction("move_right")
                .WithKeys(Keys.D, Keys.Right)
                .WithGamepadButton(Buttons.DPadRight)

            // Action buttons
            .AddAction("jump")
                .WithKey(Keys.Space)
                .WithGamepadButton(Buttons.A)
            .AddAction("attack")
                .WithKey(Keys.Enter)
                .WithMouseButton(MouseButton.Left)
                .WithGamepadButton(Buttons.X)
            .AddAction("use")
                .WithKey(Keys.E)
                .WithGamepadButton(Buttons.Y)
            .AddAction("run")
                .WithKey(Keys.LeftShift)
                .WithGamepadButton(Buttons.B)

            // Menu actions
            .AddAction("menu")
                .WithKey(Keys.Escape)
                .WithGamepadButton(Buttons.Start)
            .AddAction("pause")
                .WithKey(Keys.P)
                .WithGamepadButton(Buttons.Back)
            .AddAction("confirm")
                .WithKey(Keys.Enter)
                .WithKey(Keys.Space)
                .WithGamepadButton(Buttons.A)
            .AddAction("cancel")
                .WithKey(Keys.Escape)
                .WithGamepadButton(Buttons.B);
    }

    private InputAction? GetLastAction()
    {
        if (actions.Count == 0) return null;

        InputAction? lastAction = null;
        foreach (var action in actions.Values)
        {
            lastAction = action;
        }
        return lastAction;
    }
}
