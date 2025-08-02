using Hamaze.Arpg.Objects;
using Hamaze.Arpg.Objects.Ghost;
using Hamaze.Arpg.Objects.Items;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Scenes;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Inventory;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Scenes;

public class GameplayScene : Scene
{
    readonly Color backgroundColor = Color.CornflowerBlue;

    Player player;
    Ghost ghost;

    public override void Initialize()
    {

        CollisionsManager.Initialize();
        AddLayer("Background", LayerPriority.Background);

        player = new Player
        {
            Position = new Vector2(64, 64)
        };
        AddChild(player);

        ghost = new Ghost
        {
            Position = new Vector2(128, 64)
        };
        AddChild(ghost);


        Box box = new()
        {
            Position = new Vector2(600, 64)
        };
        AddChild(box);


        Chest chest = new()
        {
            Position = new Vector2(400, 200)
        };
        AddChild(chest);

        Spikes spikes = new()
        {
            Position = new Vector2(300, 150)
        };
        AddChild(spikes, "Background");

        CollectableItem sword = new(new SwordItem())
        {
            Position = new Vector2(200, 200),
            Collider = new Collider(
                offset: new Vector2(0, 0),
                size: new Vector2(64)
            )
        };
        AddChild(sword, "Background");

        Sing sing = new("Hello, world!")
        {
            Position = new Vector2(400, 400)
        };
        AddChild(sing);

        HealingZone healingZone = new() { Position = new Vector2(500, 100), };
        AddChild(healingZone);

        NinePatchSprite ninePatchSprite = new(AssetsManager.Textures["UI/Inventory/Window"], 7)
        {
            Position = new Vector2(0, 600),
            Size = new Vector2(320, 30)
        };
        AddChild(ninePatchSprite, UIOverlayLayerName);
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        CollisionsManager.Update(dt);
    }

    public override void Draw(Renderer renderer)
    {
        renderer.ClearBackground(backgroundColor);
        renderer.Begin();
        base.Draw(renderer);
        renderer.End();
    }

}
