using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereHallow : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjHallow>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneHallow) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworksRGB;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.PixieDust, 10)
            .AddIngredient(ItemID.UnicornHorn, 3)
            .AddIngredient(ItemID.CrystalShard, 20)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public class SphereProjHallow : SphereProj { }
}