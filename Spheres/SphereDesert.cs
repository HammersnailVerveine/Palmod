using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereDesert : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjDesert>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneDesert) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Yellow;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.AntlionMandible, 5)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddIngredient(ItemID.Amber, 8)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public class SphereProjDesert : SphereProj { }
}