using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereDungeon : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjDungeon>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            if (Main.LocalPlayer.ZoneDungeon) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworksRGB;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.Bone, 25)
            .AddIngredient(ItemID.WaterCandle, 5)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public class SphereProjDungeon : SphereProj { }
}