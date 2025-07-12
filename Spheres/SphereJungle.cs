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
    public class SphereJungle : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjJungle>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneJungle || Main.LocalPlayer.ZoneGlowshroom) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Green;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.JungleSpores, 15)
            .AddIngredient(ItemID.Stinger, 8)
            .AddIngredient(ItemID.Vine, 3)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public class SphereProjJungle : SphereProj { }
}