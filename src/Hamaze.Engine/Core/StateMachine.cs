using System;
using System.Collections.Generic;
using System.Linq;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Core;

public class StateMachine : GameObject
{
    private Type initialStateType = null!;
    private readonly Dictionary<Type, State> states = [];
    private State currentState = null!;

    public override void Initialize()
    {
        foreach (var state in Children.OfType<State>())
        {
            states[state.GetType()] = state;
        }

        if (initialStateType == null || !states.TryGetValue(initialStateType, out var initialState))
        {
            throw new InvalidOperationException($"Initial state '{initialStateType?.Name ?? "null"}' is not set or does not exist in the state machine.");
        }

        currentState = initialState;
        currentState.Initialize();
    }

    public void ChangeState<T>() where T : State
    {
        if (!states.TryGetValue(typeof(T), out var newState))
        {
            throw new InvalidOperationException($"State of type '{typeof(T).Name}' was not found in the state machine.");
        }

        currentState?.Dispose();
        currentState = newState;
        currentState.Initialize();
    }

    public void SetInitialState<T>() where T : State
    {
        initialStateType = typeof(T);
    }

    public override void Update(float dt)
    {
        currentState?.Update(dt);
    }

    public override void Draw(Renderer renderer)
    {
        currentState?.Draw(renderer);
    }
}
