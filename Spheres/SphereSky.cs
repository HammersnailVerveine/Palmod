using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereSky : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjSky>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworksRGB;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.Cloud, 50)
            .AddIngredient(ItemID.Feather, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public class SphereProjSky : SphereProj { }
}