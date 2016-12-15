namespace Flowers_Riven_Reborn.Manager.Spells
{
    using myCommon;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class SpellManager : Logic
    {
        internal static void Init()
        {
            Q = new Spell(SpellSlot.Q, 325f);
            W = new Spell(SpellSlot.W, 260f);
            E = new Spell(SpellSlot.E, 312f);
            R = new Spell(SpellSlot.R, 900f);

            Q.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.25f, 45f, 1600f, false, SkillshotType.SkillshotCone);

            Ignite = Me.GetSpellSlot("SummonerDot");
            Flash = Me.GetSpellSlot("SummonerFlash");
        }

        internal static float GetComboDamage(Obj_AI_Hero target)
        {
            if (target == null)
            {
                return 0;
            }

            var damage = 0f;

            if (Q.IsReady())
            {
                damage += GetQDamage(target);
            }

            if (W.IsReady())
            {
                damage += GetWDamage(target);
            }

            if (R.IsReady())
            {
                damage += GetRDamage(target);
            }

            return damage;
        }

        internal static double GetPassive
        {
            get
            {
                if (Me.Level == 18)
                {
                    return 0.5;
                }

                if (Me.Level >= 15)
                {
                    return 0.45;
                }

                if (Me.Level >= 12)
                {
                    return 0.4;
                }

                if (Me.Level >= 9)
                {
                    return 0.35;
                }

                if (Me.Level >= 6)
                {
                    return 0.3;
                }

                if (Me.Level >= 3)
                {
                    return 0.25;
                }

                return 0.2;
            }
        }

        internal static float GetQDamage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            var qhan = 3 - qStack;

            return (float)(Q.GetDamage(target) * qhan + Me.GetAutoAttackDamage(target) * qhan * (1 + GetPassive));
        }

        internal static float GetWDamage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            return W.GetDamage(target);
        }

        internal static float GetRDamage(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }

            return (float)Me.CalcDamage(target, Damage.DamageType.Physical,
                (new double[] { 80, 120, 160 }[R.Level - 1] +
                 0.6 * Me.FlatPhysicalDamageMod) *
                (1 + (target.MaxHealth - target.Health) /
                 target.MaxHealth > 0.75
                    ? 0.75
                    : (target.MaxHealth - target.Health) / target.MaxHealth) * 8 / 3);
        }

        internal static void CastItem(bool tiamat = false, bool youmuu = false)
        {
            if (tiamat)
            {
                if (Items.HasItem(3077) && Items.CanUseItem(3077))
                {
                    Items.UseItem(3077);
                }

                if (Items.HasItem(3074) && Items.CanUseItem(3074))
                {
                    Items.UseItem(3074);
                }

                if (Items.HasItem(3053) && Items.CanUseItem(3053))
                {
                    Items.UseItem(3053);
                }
            }

            if (youmuu)
            {
                if (Items.HasItem(3142) && Items.CanUseItem(3142))
                {
                    Items.UseItem(3142);
                }
            }
        }

        internal static void CastQ(Obj_AI_Base target)
        {
            if (target != null && !target.IsDead)
            {
                switch (Menu.GetList("QMode"))
                {
                    case 0:
                        Q.Cast(target.Position, true);
                        break;
                    case 1:
                        Q.Cast(Game.CursorPos, true);
                        break;
                    case 2:
                        Q.Cast(Me.Position.Extend(target.Position, Q.Range), true);
                        break;
                    default:
                        Q.Cast(Me.Position.Extend(Game.CursorPos, Q.Range), true);
                        break;
                }
            }
        }

        internal static void ResetQA(int time)
        {
            if (Menu.GetBool("Dance"))
            {
                Game.SendEmote(Emote.Dance);
            }
            Utility.DelayAction.Add(time, () =>
            {
                Game.SendEmote(Emote.Dance);
                myCommon.Orbwalking.ResetAutoAttackTimer();
                Me.IssueOrder(GameObjectOrder.MoveTo, Me.Position.Extend(Game.CursorPos, +10));
            });
        }

        internal static void R2Logic(Obj_AI_Base target)
        {
            if (target == null || R.Instance.Name == "RivenFengShuiEngine")
            {
                return;
            }

            if (target.Check(850))
            {
                switch (Menu.GetList("R2Mode"))
                {
                    case 0:
                        if (GetRDamage(target) > target.Health && target.DistanceToPlayer() < 600)
                        {
                            var pred = R.GetPrediction(target, true);

                            if (pred.Hitchance >= HitChance.VeryHigh)
                            {
                                R.Cast(pred.CastPosition, true);
                            }
                        }
                        break;
                    case 1:
                        if (target.HealthPercent < 20 ||
                            (target.Health > GetRDamage(target) + Me.GetAutoAttackDamage(target) * 2 &&
                             target.HealthPercent < 40) ||
                            (target.Health <= GetRDamage(target)))
                        {
                            var pred = R.GetPrediction(target, true);

                            if (pred.Hitchance >= HitChance.VeryHigh)
                            {
                                R.Cast(pred.CastPosition, true);
                            }
                        }
                        break;
                    case 2:
                        if (target.DistanceToPlayer() < 600)
                        {
                            var pred = R.GetPrediction(target, true);

                            if (pred.Hitchance >= HitChance.VeryHigh)
                            {
                                R.Cast(pred.CastPosition, true);
                            }
                        }
                        break;
                }
            }
        }
    }
}
