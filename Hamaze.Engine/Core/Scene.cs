using System.Collections.Generic;
using System.Linq;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Hamaze.Engine.Core;

public class Scene
{
    protected readonly List<GameObject> instances = [];
    public bool IsActive { get; protected set; } = true;
    public bool IsPaused { get; protected set; } = false;

    public void AddChild(GameObject child)
    {
        instances.Add(child);
        child.Initialize();
    }

    public void RemoveChild(GameObject child)
    {
        instances.Remove(child);
        child.Dispose();
    }

    public virtual void Initialize()
    {
        instances.ForEach(obj => obj.Initialize());
    }

    public virtual void LoadContent(ContentManager content) { }

    public virtual void OnEnter()
    {
        IsActive = true;
        IsPaused = false;
    }

    public virtual void OnExit()
    {
        IsActive = false;
    }

    public virtual void OnPause()
    {
        IsPaused = true;
    }

    public virtual void OnResume()
    {
        IsPaused = false;
    }

    public virtual void Update(float dt)
    {
        if (!IsActive || IsPaused) return;
        instances.ForEach(obj => obj.Update(dt));
    }

    public virtual void Draw(Renderer renderer)
    {
        if (!IsActive) return;

        // add dynamic and static objects to the scene
        // and sort them by their global position
        // to ensure correct rendering order
        var sortedInstances = instances.OrderBy(i => i.GlobalPosition.Y);
        foreach (var obj in sortedInstances)
        {
            obj.Draw(renderer);
        }
    }

    public virtual void Dispose()
    {
        instances.ForEach(obj => obj.Dispose());
        instances.Clear();
        IsActive = false;
    }
}