using System;
using System.Linq;
using System.Xml.Linq;
using Hamaze.Arpg.Objects.Items;
using Hamaze.Engine.Data;
using Hamaze.Engine.Systems.Inventory;

ResourceManager.RegisterResourceType(nameof(SwordItem), () => new SwordItem());
ResourceManager.RegisterResourceType(nameof(Item), () => new Item());
using var game = new Hamaze.Arpg.ArpgGame();
game.Run();