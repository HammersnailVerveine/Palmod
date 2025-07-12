using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereUnderground : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjUnderground>();
            Item.shootSpeed = 12.5f;
        }

        public override float GetCaptureThreshold()
        {
            Player player = Main.LocalPlayer;
            if (player.ZoneNormalUnderground || player.ZoneNormalCaverns || (player.ZoneSnow && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight))) return 0.3f;
            return 0.1f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Yellow;

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.GoldBar, 20)
            .AddIngredient(ItemID.Diamond, 10)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient<Sphere>()
            .AddIngredient(ItemID.PlatinumBar, 20)
            .AddIngredient(ItemID.Diamond, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

    public class SphereProjUnderground : SphereProj { }
}