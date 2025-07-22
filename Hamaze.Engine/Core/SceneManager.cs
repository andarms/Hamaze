using System;
using System.Collections.Generic;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;
namespace Hamaze.Engine.Core;

public static class SceneManager
{
    static Scene? currentScene = null;
    static Scene? previousScene = null;
    static readonly Dictionary<Type, Scene> scenes = [];
    static readonly Stack<Scene> sceneStack = [];

    public static Scene? CurrentScene => currentScene;
    public static bool IsTransitioning { get; private set; } = false;

    public static void Initialize()
    {
        currentScene?.Initialize();
        // PhysicsWorld.Initialize(64);
    }

    public static void Update(float dt)
    {
        currentScene?.Update(dt);
        // PhysicsWorld.Update(dt);
    }

    public static void Draw(Renderer renderer)
    {
        currentScene?.Draw(renderer);
        // PhysicsWorld.Draw(renderer);
    }

    public static void AddScene(Scene scene)
    {
        scenes.Add(scene.GetType(), scene);
    }

    public static void AddScene<T>(T scene) where T : Scene
    {
        scenes[typeof(T)] = scene;
    }

    public static void SwitchTo<T>() where T : Scene, new()
    {
        if (scenes.TryGetValue(typeof(T), out var scene))
        {
            SwitchToScene(scene);
        }
        else
        {
            // Create new scene if not found
            var newScene = new T();
            scenes[typeof(T)] = newScene;
            SwitchToScene(newScene);
        }
    }

    public static void SwitchTo<T>(T scene) where T : Scene
    {
        scenes[typeof(T)] = scene;
        SwitchToScene(scene);
    }

    private static void SwitchToScene(Scene scene)
    {
        IsTransitioning = true;
        previousScene = currentScene;
        previousScene?.OnExit();

        currentScene = scene;
        currentScene.OnEnter();
        currentScene.Initialize();

        IsTransitioning = false;
    }

    public static void PushScene<T>() where T : Scene, new()
    {
        if (currentScene != null)
        {
            sceneStack.Push(currentScene);
            currentScene.OnPause();
        }

        if (scenes.TryGetValue(typeof(T), out var scene))
        {
            currentScene = scene;
        }
        else
        {
            var newScene = new T();
            scenes[typeof(T)] = newScene;
            currentScene = newScene;
        }

        currentScene.OnEnter();
        currentScene.Initialize();
    }

    public static void PopScene()
    {
        if (sceneStack.Count > 0)
        {
            currentScene?.OnExit();
            currentScene = sceneStack.Pop();
            currentScene.OnResume();
        }
    }

    public static T? GetScene<T>() where T : Scene
    {
        return scenes.TryGetValue(typeof(T), out var scene) ? scene as T : null;
    }

    public static bool HasScene<T>() where T : Scene
    {
        return scenes.ContainsKey(typeof(T));
    }

    public static void RemoveScene<T>() where T : Scene
    {
        if (scenes.TryGetValue(typeof(T), out var scene))
        {
            if (currentScene == scene)
            {
                currentScene = null;
            }
            scene.Dispose();
            scenes.Remove(typeof(T));
        }
    }

    public static void ClearAllScenes()
    {
        foreach (var scene in scenes.Values)
        {
            scene.Dispose();
        }
        scenes.Clear();
        sceneStack.Clear();
        currentScene = null;
        previousScene = null;
    }
}