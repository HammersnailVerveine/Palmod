using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using VervPalMod.Spheres;

namespace VervPalMod
{
    public abstract class SphereMain : ModItem
    {
        public override void SetDefaults()
        {
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item64;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.rare = ItemRarityID.Green;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.shoot = ModContent.ProjectileType<SphereProj>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            VervPalModPlayer modPlayer = player.GetModPlayer<VervPalModPlayer>();
            if (modPlayer.SphereUI)
            {
                Item.autoReuse = false;
                Item.useAnimation = 10;
                Item.useTime = 10;
                modPlayer.keepSelected = player.selectedItem;
                int wheelValue = PlayerInput.ScrollWheelDeltaForUI;
                if (wheelValue != 0 && modPlayer.TamedIDs.Count > 0)
                {
                    modPlayer.SphereUIIndex += Math.Sign(wheelValue);
                    //SoundEngine.PlaySound(SoundID.MenuTick, player.Center);

                    if (modPlayer.SphereUIIndex >= modPlayer.TamedIDs.Count) modPlayer.SphereUIIndex = 0;
                    if (modPlayer.SphereUIIndex < 0) modPlayer.SphereUIIndex = modPlayer.TamedIDs.Count - 1;
                }
            }
            else
            {
                Item.autoReuse = true;
                Item.useAnimation = 30;
                Item.useTime = 30;
            }
        }

        public override bool CanUseItem(Player player)
        {
            VervPalModPlayer modPlayer = player.GetModPlayer<VervPalModPlayer>();
            if (player.altFunctionUse == 2)
            {
                modPlayer.SphereUI = !modPlayer.SphereUI;
                if (modPlayer.SphereUI) CombatText.NewText(player.Hitbox, Color.YellowGreen, Language.GetTextValue(ModContent.GetInstance<VervPalMod>().GetLocalizationKey("Common.ReleaseMode")));
                else CombatText.NewText(player.Hitbox, Color.OrangeRed, Language.GetTextValue(ModContent.GetInstance<VervPalMod>().GetLocalizationKey("Common.CaptureMode")));
                SoundEngine.PlaySound(SoundID.MenuTick, player.Center);
                return false;
            }
            else if (modPlayer.SphereUI)
            {
                if (modPlayer.CurrentTamedNPCs >= modPlayer.MaxTamedNPCs || modPlayer.TamedIDs.Count == 0)
                {
                    CombatText.NewText(player.Hitbox, Color.LightGray, Language.GetTextValue(ModContent.GetInstance<VervPalMod>().GetLocalizationKey("Common.CannotUse")), false, true);
                    SoundEngine.PlaySound(SoundID.MenuTick, player.Center);
                    return false;
                }
            }

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            VervPalModPlayer modPlayer = player.GetModPlayer<VervPalModPlayer>();
            int ai = 0;
            if (modPlayer.SphereUI)
            {
                if (modPlayer.TamedIDs.Count > 0)
                {
                    Tuple<int, int> tuple = modPlayer.TamedIDs[modPlayer.SphereUIIndex];
                    ai = tuple.Item1;
                    if (tuple.Item2 - 1 > 0) modPlayer.TamedIDs[modPlayer.SphereUIIndex] = new Tuple<int, int>(tuple.Item1, tuple.Item2 - 1);
                    else
                    {
                        modPlayer.TamedIDs.RemoveAt(modPlayer.SphereUIIndex);
                        if (modPlayer.SphereUIIndex >= modPlayer.TamedIDs.Count) modPlayer.SphereUIIndex = 0;
                    }
                }
            }

            int newDamage = (int)(GetCaptureThreshold() * 100);
            if (Main.hardMode) newDamage *= 3;
            Projectile.NewProjectile(source, position, velocity, Item.shoot, newDamage, 1f, player.whoAmI, ai, GetCaptureThreshold(), GetDustID());

            return false;
        }

        public virtual float GetCaptureThreshold()
        {
            return 0.1f;
        }

        public virtual int GetDustID() => DustID.Electric;
    }

    public class Sphere : SphereMain {}

    public class SphereProj : ModProjectile
    {
        private Texture2D TextureMain;
        public List<Vector2> OldPosition;
        public List<float> OldRotation;
        public int TimeSpent;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
            Projectile.scale = 1f;
            Projectile.penetrate = 1; ;
            TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            OldPosition = new List<Vector2>();
            OldRotation = new List<float>();
            TimeSpent = Main.rand.Next(314);
        }

        public override void AI()
        {
            Projectile.friendly = Projectile.ai[0] == 0;

            TimeSpent += (int)Projectile.velocity.X;
            Projectile.velocity.Y += 0.15f;
            Projectile.velocity.X *= 0.99f;

            Projectile.rotation = Projectile.velocity.ToRotation() + TimeSpent * 0.01f;
            OldPosition.Add(Projectile.Center);
            OldRotation.Add(Projectile.rotation);

            if (OldPosition.Count > 10)
            {
                OldPosition.RemoveAt(0);
                OldRotation.RemoveAt(0);
            }

            if (Main.rand.NextBool(10))
            {
                int dustType = (int)Projectile.ai[2];
                if (dustType == -1)
                {
                    switch (Main.rand.Next(6))
                    {
                        case 0:
                            dustType = DustID.Electric;
                            break;
                        case 1:
                            dustType = DustID.FireworkFountain_Green;
                            break;
                        case 2:
                            dustType = DustID.FireworkFountain_Pink;
                            break;
                        case 3:
                            dustType = DustID.FireworkFountain_Red;
                            break;
                        case 4:
                            dustType = DustID.FireworkFountain_Yellow;
                            break;
                        default:
                            dustType = DustID.FireworksRGB;
                            break;
                    }
                }

                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);
                dust.noGravity = true;
                dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.5f;
                if (dustType == DustID.FireworksRGB) dust.color = Color.White;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
            if (VervPalModGlobalNPC.CanBeTamed(target))
            {
                bool validCapture = false;
                if (target.realLife != -1)
                {
                    if (Main.npc[target.realLife].life <= Main.npc[target.realLife].lifeMax * Projectile.ai[1]) validCapture = true;
                }

                if (target.life <= target.lifeMax * Projectile.ai[1] || validCapture || (target.life + target.defense * 0.5) - Projectile.damage <= 0)
                {
                    target.active = false;
                    if (target.realLife != -1) Main.npc[target.realLife].active = false;
                    modifiers.FinalDamage *= 0f;

                    Player player = Main.player[Projectile.owner];

                    int count = 1;
                    if (Projectile.ai[2] != DustID.Electric) count++;
                    if (Projectile.ModProjectile is SphereProjUniversal) count = 3;
                    if (Projectile.ModProjectile is SphereProjLuminite) count = 5;
                    if (Projectile.ModProjectile is SphereProjDebug) count = 10;

                    CombatText.NewText(player.Hitbox, Color.YellowGreen, "+" + count + " " + target.GivenOrTypeName, true);
                    SoundEngine.PlaySound(SoundID.Item158, target.Center);

                    for (int i = 0; i < target.width; i += 2)
                    {
                        int dustType = (int)Projectile.ai[2];
                        if (dustType == -1)
                        {
                            switch (Main.rand.Next(6))
                            {
                                case 0:
                                    dustType = DustID.Electric;
                                    break;
                                case 1:
                                    dustType = DustID.FireworkFountain_Green;
                                    break;
                                case 2:
                                    dustType = DustID.FireworkFountain_Pink;
                                    break;
                                case 3:
                                    dustType = DustID.FireworkFountain_Red;
                                    break;
                                case 4:
                                    dustType = DustID.FireworkFountain_Yellow;
                                    break;
                                default:
                                    dustType = DustID.FireworksRGB;
                                    break;
                            }
                        }

                        Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, dustType);
                        dust.noGravity = true;
                        if (dustType == DustID.FireworksRGB) dust.color = Color.White;
                    }

                    VervPalModPlayer modPlayer = player.GetModPlayer<VervPalModPlayer>();

                    int targetType = target.netID;
                    if (target.realLife != -1) targetType = Main.npc[target.realLife].netID;

                    if (modPlayer.TamedIDs.Count > 0)
                    {
                        for (int i = modPlayer.TamedIDs.Count - 1; i >= 0; i--)
                        {
                            Tuple<int, int> tuple = modPlayer.TamedIDs[i];
                            if (tuple.Item1 == targetType)
                            {
                                modPlayer.TamedIDs[i] = new Tuple<int, int>(tuple.Item1, tuple.Item2 + count);
                                return;
                            }
                        }
                    }

                    modPlayer.TamedIDs.Add(new Tuple<int, int>(targetType, count));
                }
                else
                { // Sphere can't kill the target
                    if ((target.realLife == -1 || target.realLife == target.whoAmI))
                    {
                        if (target.life - modifiers.GetDamage(Projectile.damage, false) <= target.lifeMax * Projectile.ai[1])
                        {
                            modifiers.SourceDamage *= 0f;
                            modifiers.FinalDamage.Flat = target.life - (int)(target.lifeMax * Projectile.ai[1]);
                        }
                    }
                    else
                    {
                        NPC newtarget = Main.npc[target.realLife];
                        if (newtarget.life - modifiers.GetDamage(Projectile.damage, false) <= newtarget.lifeMax * Projectile.ai[1])
                        {
                            modifiers.SourceDamage *= 0f;
                            modifiers.FinalDamage.Flat = newtarget.life - (int)(newtarget.lifeMax * Projectile.ai[1]);
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustType = (int)Projectile.ai[2];
                if (dustType == -1)
                {
                    switch (Main.rand.Next(6))
                    {
                        case 0:
                            dustType = DustID.Electric;
                            break;
                        case 1:
                            dustType = DustID.FireworkFountain_Green;
                            break;
                        case 2:
                            dustType = DustID.FireworkFountain_Pink;
                            break;
                        case 3:
                            dustType = DustID.FireworkFountain_Red;
                            break;
                        case 4:
                            dustType = DustID.FireworkFountain_Yellow;
                            break;
                        default:
                            dustType = DustID.FireworksRGB;
                            break;
                    }
                }

                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);
                dust.noGravity = true;
                if (dustType == DustID.FireworksRGB) dust.color = Color.White;
            }

            if (Projectile.ai[0] != 0)
            {
                NPC npc = NPC.NewNPCDirect(Projectile.GetSource_FromAI(), (int)Projectile.Center.X, (int)Projectile.Center.Y, (int)Projectile.ai[0]);
                VervPalModGlobalNPC globalNPC = npc.GetGlobalNPC<VervPalModGlobalNPC>();
                if (Projectile.ai[0] < 0) npc.netID = (int)Projectile.ai[0];
                npc.friendly = true;
                npc.lifeMax *= 2;
                npc.life = npc.lifeMax;
                globalNPC.Tamed = true;
                npc.netUpdate = true;

                foreach (NPC npc2 in Main.npc)
                {
                    if (npc2.realLife == npc.whoAmI && !npc2.friendly)
                    {
                        globalNPC = npc2.GetGlobalNPC<VervPalModGlobalNPC>();
                        npc2.friendly = true;
                        globalNPC.Tamed = true;
                        npc2.netUpdate = true;
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            // Draw code here

            for (int i = 0; i < OldPosition.Count; i++)
            {
                Vector2 drawPosition2 = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
                spriteBatch.Draw(TextureMain, drawPosition2, null, Color.White * 0.06f * (i + 1), OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.075f, SpriteEffects.None, 0f);
            }

            // Draw code ends here

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
            spriteBatch.Draw(TextureMain, drawPosition, null, Color.White, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}