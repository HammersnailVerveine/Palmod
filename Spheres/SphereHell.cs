using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereHell : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjHell>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneUnderworldHeight) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Red;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.HellstoneBar, 20)
            .AddIngredient(ItemID.Obsidian, 20)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public class SphereProjHell : SphereProj { }
}