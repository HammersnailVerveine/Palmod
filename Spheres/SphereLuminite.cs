using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereLuminite : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.shoot = ModContent.ProjectileType<SphereProjLuminite>();
            Item.shootSpeed = 15f;
        }

        public override float GetCaptureThreshold()
        {
            return 0.5f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Green;

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient<SphereUniversal>()
            .AddIngredient(ItemID.LunarBar, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }

    public class SphereProjLuminite : SphereProj { }
}