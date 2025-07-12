using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod.Spheres
{
    public class SphereDebug : SphereMain
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Quest;
            Item.value = Item.sellPrice(0, 0, 0, 1);
            Item.shoot = ModContent.ProjectileType<SphereProjDebug>();
            Item.shootSpeed = 15f;
        }

        public override float GetCaptureThreshold()
        {
            return 1f;
        }

        public override int GetDustID() => DustID.FireworkFountain_Yellow;
    }

    public class SphereProjDebug : SphereProj { }
}