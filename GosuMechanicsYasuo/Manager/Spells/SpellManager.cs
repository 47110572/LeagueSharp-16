namespace GosuMechanicsYasuo.Manager.Spells
{
    using System;
    using System.Linq;
    using SharpDX;
    using LeagueSharp;
    using LeagueSharp.Common;
    using static Common.Common;

    internal class SpellManager : Logic
    {
        internal static void Init()
        {
            Q = new Spell(SpellSlot.Q, 475f);
            Q3 = new Spell(SpellSlot.Q, 1000f);
            W = new Spell(SpellSlot.W, 400f);
            E = new Spell(SpellSlot.E, 475f);
            R = new Spell(SpellSlot.R, 1200f);

            Q.SetSkillshot(0.4f, 30f, float.MaxValue, false, SkillshotType.SkillshotLine);
            Q3.SetSkillshot(0.35f, 90f, 1200f, false, SkillshotType.SkillshotLine);

            var dot = ObjectManager.Player.GetSpellSlot("summonerdot");
            if (dot != SpellSlot.Unknown)
            {
                Ignite = new Spell(dot, 600, TargetSelector.DamageType.True);
            }

            var flash = ObjectManager.Player.GetSpellSlot("SummonerFlash");
            if (flash != SpellSlot.Unknown)
            {
                Flash = new Spell(dot, 425);
            }
        }

        internal static bool HaveStatik => Me.GetBuffCount("ItemStatikShankCharge") == 100;

        internal static bool HaveQ3 => Me.HasBuff("YasuoQ3W");

        internal static bool CanCastE(Obj_AI_Base target)
        {
            return !target.HasBuff("YasuoDashWrapper");
        }

        internal static void CastQ3()
        {
            //copy from valvesharp
            var targets = HeroManager.Enemies.Where(x => x.DistanceToPlayer() <= Q3.Range);
            var castPos = Vector3.Zero;

            if (!targets.Any())
            {
                return;
            }

            foreach (var pred in
                targets.Select(i => Q3.GetPrediction(i, true))
                    .Where(
                        i => i.Hitchance >= HitChance.VeryHigh ||
                             (i.Hitchance >= HitChance.High && i.AoeTargetsHitCount > 1))
                    .OrderByDescending(i => i.AoeTargetsHitCount))
            {
                castPos = pred.CastPosition;
                break;
            }

            if (castPos.IsValid())
            {
                Q3.Cast(castPos, true);
            }
        }

        internal static bool useENormal(Obj_AI_Base target)
        {
            if (!E.IsReady() || target.Distance(Me) > 470)
            {
                return false;
            }

            var posAfter = V2E(Me.Position, target.Position, 475);

            if (!Menu.Item("ETower", true).GetValue<bool>())
            {
                if (isSafePoint(posAfter).IsSafe)
                {
                    E.Cast(target, true);
                }

                return true;
            }

            var pPos = Me.ServerPosition.To2D();
            var posAfterE = pPos + Vector2.Normalize(target.Position.To2D() - pPos) * E.Range;

            if (!posAfterE.To3D().UnderTurret(true))
            {
                if (isSafePoint(posAfter, true).IsSafe)
                {
                    E.Cast(target, true);
                }

                return true;
            }

            return false;
        }

        internal static double GetQDmg(Obj_AI_Base target)
        {
            var dmgItem = 0d;

            if (Items.HasItem(3057) && (Items.CanUseItem(3057) || Me.HasBuff("Sheen")))
            {
                dmgItem = Me.BaseAttackDamage;
            }

            if (Items.HasItem(3078) && (Items.CanUseItem(3078) || Me.HasBuff("Sheen")))
            {
                dmgItem = Me.BaseAttackDamage * 2;
            }

            var damageModifier = 1d;
            var reduction = 0d;
            var result = dmgItem
                         + Me.TotalAttackDamage * (Me.Crit >= 0.85f ? (Items.HasItem(3031) ? 1.875 : 1.5) : 1);

            if (Items.HasItem(3153))
            {
                var dmgBotrk = Math.Max(0.08 * target.Health, 10);
                result += target is Obj_AI_Minion ? Math.Min(dmgBotrk, 60) : dmgBotrk;
            }

            var targetHero = target as Obj_AI_Hero;

            if (targetHero != null)
            {
                if (Items.HasItem(3047, targetHero))
                {
                    damageModifier *= 0.9d;
                }

                if (targetHero.ChampionName == "Fizz")
                {
                    reduction += 4 + (targetHero.Level - 1 / 3) * 2;
                }

                var mastery = targetHero.Masteries.FirstOrDefault(i => i.Page == MasteryPage.Defense && i.Id == 68);

                if (mastery != null && mastery.Points >= 1)
                {
                    reduction += 1 * mastery.Points;
                }
            }

            return Me.CalcDamage(
                       target,
                       Damage.DamageType.Physical,
                       20*Q.Level + (result - reduction)*damageModifier)
                   + (HaveStatik
                       ? Me.CalcDamage(
                           target,
                           Damage.DamageType.Magical,
                           100*(Me.Crit >= 0.85f ? (Items.HasItem(3031) ? 2.25 : 1.8) : 1))
                       : 0);
        }


        internal static double GetEDmg(Obj_AI_Base target)
        {
            var stacksPassive = Me.Buffs.Find(b => b.DisplayName.Equals("YasuoDashScalar"));
            var Estacks = stacksPassive?.Count ?? 0;
            var damage = (E.Level * 20 + 50) * (1 + 0.25 * Estacks) + Me.FlatMagicDamageMod * 0.6;

            return Me.CalcDamage(target, Damage.DamageType.Magical, damage);
        }
    }
}
