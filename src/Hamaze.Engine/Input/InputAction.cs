using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Engine.Input;

/// <summary>
/// Represents an input action that can be triggered by various input methods (keys, buttons, etc.)
/// </summary>
public class InputAction
{
    public string Name { get; set; }
    public List<Keys> Keys { get; set; } = new();
    public List<Buttons> GamepadButtons { get; set; } = new();
    public List<MouseButton> MouseButtons { get; set; } = new();
    public float DeadZone { get; set; } = 0.5f;

    public InputAction(string name)
    {
        Name = name;
    }

    public InputAction AddKey(Keys key)
    {
        Keys.Add(key);
        return this;
    }

    public InputAction AddGamepadButton(Buttons button)
    {
        GamepadButtons.Add(button);
        return this;
    }

    public InputAction AddMouseButton(MouseButton button)
    {
        MouseButtons.Add(button);
        return this;
    }

    public InputAction SetDeadZone(float deadZone)
    {
        DeadZone = deadZone;
        return this;
    }
}

/// <summary>
/// Represents mouse buttons for input mapping
/// </summary>
public enum MouseButton
{
    Left,
    Right,
    Middle,
    XButton1,
    XButton2
}
