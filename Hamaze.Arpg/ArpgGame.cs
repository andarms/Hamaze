using Hamaze.Arpg.Content;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg;

public class ArpgGame : Game
{
    private readonly GraphicsDeviceManager graphics;

    private Renderer renderer;
    private Player player;


    public ArpgGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        renderer = new Renderer(GraphicsDevice);
        AssetsManager.LoadContent(Content);
        player = new Player
        {
            Position = new Vector2(64, 64) // Center the player in the window
        };
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        base.Update(gameTime);
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        player.Update(dt);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        renderer.Begin();
        player.Draw(renderer);
        renderer.End();

        base.Draw(gameTime);
    }
}

