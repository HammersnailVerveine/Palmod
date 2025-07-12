using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace VervPalMod
{
    public class VervPalModGlobalProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.hostile && source is EntitySource_Parent parent && parent.Entity is NPC npc)
            {
                VervPalModGlobalNPC globalNPC = npc.GetGlobalNPC<VervPalModGlobalNPC>();
                if (npc.friendly && npc.active && globalNPC.Tamed)
                {
                    globalNPC.Ai[0] = 210;
                    projectile.friendly = true;
                    projectile.hostile = false;
                    projectile.damage *= 3;
                    projectile.DamageType = DamageClass.Summon;
                    if (Main.LocalPlayer.numMinions > 0) projectile.damage = (int)(projectile.damage * 0.01f);

                    projectile.owner = Main.myPlayer;
                    NPC target = Main.LocalPlayer.GetModPlayer<VervPalModPlayer>().Target;
                    if (target != null)
                    {
                        Vector2 position = target.Center;
                        if (npc.TypeName.Contains("Archer")) position.Y -= (position - projectile.Center).Length() * 0.12f;
                        projectile.velocity = Vector2.Normalize(position - projectile.Center).RotatedByRandom(MathHelper.ToRadians(10f)) * projectile.velocity.Length();
                        projectile.position = npc.Center + Vector2.Normalize(projectile.velocity) * projectile.width;
                    }
                    else projectile.active = false;

                    projectile.netUpdate = true;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (projectile.type)
            {
                case ProjectileID.FlamingArrow:
                    if (Main.rand.NextBool(3)) target.AddBuff(BuffID.OnFire, 60 * 7);
                    break;
                case ProjectileID.GoldenShowerHostile:
                    target.AddBuff(BuffID.Ichor, 60 * 15);
                    break;
                default:
                    break;
            }
        }
    }
}