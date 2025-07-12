using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereUniversal : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjUniversal>();
            Item.shootSpeed = 15f;
        }

        public override float GetCaptureThreshold()
        {
            return 0.35f;
        }

        public override int GetDustID() => -1;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<SphereJungle>()
            .AddIngredient<SphereUnderground>()
            .AddIngredient<SphereDesert>()
            .AddIngredient<SphereHell>()
            .AddIngredient<SphereEvil>()
            .AddIngredient<SphereHallow>()
            .AddIngredient<SphereSky>()
            .AddIngredient<SphereDungeon>()
            .AddIngredient(ItemID.HallowedBar, 20)
            .AddIngredient(ItemID.SoulofMight, 5)
            .AddIngredient(ItemID.SoulofSight, 5)
            .AddIngredient(ItemID.SoulofFright, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public class SphereProjUniversal : SphereProj { }
}