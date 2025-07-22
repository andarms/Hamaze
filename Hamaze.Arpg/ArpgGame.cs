using Hamaze.Arpg.Content;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Arpg.Scenes;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg;

public class ArpgGame : Game
{
    private readonly GraphicsDeviceManager graphics;
    private Renderer renderer;


    public ArpgGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.SynchronizeWithVerticalRetrace = false;

        SceneManager.AddScene(new GameplayScene());
    }

    protected override void LoadContent()
    {
        renderer = new Renderer(GraphicsDevice);
        AssetsManager.LoadContent(Content);
        SceneManager.Initialize();
        SceneManager.SwitchTo<GameplayScene>();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        base.Update(gameTime);
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        SceneManager.Update(dt);
    }

    protected override void Draw(GameTime gameTime)
    {
        renderer.ClearBackground(Color.Black);
        SceneManager.Draw(renderer);
        base.Draw(gameTime);
    }
}

