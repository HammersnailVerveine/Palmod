using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereEvil : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjEvil>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Pink;

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddIngredient(ItemID.ShadowScale, 15)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.CrimtaneBar, 10)
            .AddIngredient(ItemID.TissueSample, 15)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

    public class SphereProjEvil : SphereProj { }
}