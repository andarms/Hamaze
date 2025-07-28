# 🧩 Game Objects

Hamaze **Game Objects** are the core building blocks of the game world. They support composition through traits and child components, enabling highly modular and reusable entities.

## 🧬 Traits – Data-Only Properties

**Traits** are lightweight data containers that define the state or identity of a game object. They do **not contain logic**, they're purely declarative and can be added or removed dynamically at runtime.

Traits enable flexible tagging and configuration of behaviors such as:

- `Solid`
- `Movable`
- `Interactable`
- `Exhausted`, `Frozen`, etc.

```csharp
// Example of a custom trait definition
public record Exhausted : Trait("exhausted") { }
```

> [!NOTE]  
> You can check for traits using queries like `GameObject.Traits.Has<T>()` or `GameObject.Traits.Has("exhausted")`.

## 🧱 Components – Composable Game Object Children

Game objects can contain **child game objects**. These act as components that define visual, physical, or logical parts of a more complex object.

This enables a **composition over inheritance** design, similar to node-based scene trees like in [Godot](https://docs.godotengine.org/en/4.4/getting_started/introduction/godot_design_philosophy.html#object-oriented-design-and-composition).

### ✅ Example

```csharp
public class Example : GameObject
{
  public Example()
  {
    Name = "Example";

    Sprite sprite = new Sprite(AssetsManager.ExampleSprite)
    {
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(0, 0, 16, 16)
    };

    AddChild(sprite); // Add sprite as a child component
  }
}
```
