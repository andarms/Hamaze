# Hamaze Game Engine

A cross-platform 2D game engine built with C# and MonoGame, designed for creating action RPGs and other 2D games.

## 🎮 Features

- **Component-Based Architecture**: Modular game object system with reusable components
- **Cross-Platform Support**: Runs on Windows, macOS, and Linux
- **Physics Integration**: Built-in collision detection and response system
- **Event System**: Signal-based communication between game objects
- **Asset Management**: Streamlined content loading and management
- **Camera System**: Flexible 2D camera with follow and viewport controls

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [MonoGame](https://monogame.net/) (included as NuGet package)
- **Platform-specific requirements:**
  - **Windows**: Visual Studio 2022 or VS Code
  - **macOS**: Visual Studio for Mac or VS Code with C# extension
  - **Linux**: VS Code with C# extension or any .NET-compatible IDE

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/andarms/Hamaze.git
cd Hamaze
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Game

```bash
dotnet run --project Hamaze.Desktop
```

## 🏗️ Project Structure

```
Hamaze/
├── Hamaze.Engine/          # Core game engine library
│   ├── Core/               # Base game object and component system
│   ├── Events/             # Signal and event handling
│   ├── Graphics/           # Rendering and sprite management
│   └── Physics/            # Collision detection and physics
├── Hamaze.Arpg/           # Action RPG game implementation
│   ├── Content/            # Game assets (sprites, sounds, etc.)
│   ├── Objects/            # Game-specific objects (Player, etc.)
│   └── ArpgGame.cs        # Main game class
├── Hamaze.Desktop/        # Desktop platform launcher
│   └── Program.cs         # Entry point
└── Hamaze.sln            # Visual Studio solution file
```

## 🔧 Development Setup

### Using Visual Studio Code

1. Install the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
2. Open the project folder in VS Code
3. Use the built-in tasks for building and running:
   - **Ctrl+Shift+P** → "Tasks: Run Task" → "build"
   - **Ctrl+Shift+P** → "Tasks: Run Task" → "watch" (for development)

### Using Visual Studio

1. Open `Hamaze.sln` in Visual Studio
2. Set `Hamaze.Desktop` as the startup project
3. Press **F5** to build and run

### Available Tasks

- `dotnet build` - Build the entire solution
- `dotnet run --project Hamaze.Desktop` - Run the desktop version
- `dotnet watch run --project Hamaze.Desktop` - Run with hot reload
- `dotnet publish` - Create a release build

## 🎯 Creating Your First Game

1. **Extend the ArpgGame class** or create a new game class inheriting from `Game`
2. **Add your game objects** in the `Hamaze.Arpg/Objects/` directory
3. **Import assets** using the MonoGame Content Pipeline
4. **Implement game logic** using the component system

### Example Game Object

```csharp
public class Enemy : GameObject
{
    private Sprite sprite;
    private PhysicsObject physics;

    public Enemy(Vector2 position)
    {
        sprite = new Sprite("Textures/enemy");
        physics = new PhysicsObject();
        Transform.Position = position;
    }

    public override void Update(GameTime gameTime)
    {
        // Enemy AI logic here
        base.Update(gameTime);
    }
}
```

## 📦 Adding Content

1. Place your assets in `Hamaze.Arpg/Content/`
2. Add them to `Content.mgcb` using the MonoGame Pipeline Tool
3. Load them in your game using the `AssetsManager`

## 🐛 Debugging

- Use Visual Studio's debugger or VS Code's debugging features
- Enable debug symbols in Debug configuration
- Check the console output for runtime information

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is open source. See the LICENSE file for details.

## 🔗 Resources

- [MonoGame Documentation](https://docs.monogame.net/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Game Programming Patterns](https://gameprogrammingpatterns.com/)

## 📞 Support

If you encounter any issues or have questions:

- Open an issue on GitHub
- Check the documentation
- Review existing discussions and issues

---

**Happy Game Development! 🎮**
