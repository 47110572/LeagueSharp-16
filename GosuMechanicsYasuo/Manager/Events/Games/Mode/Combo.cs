namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using System.Collections.Generic;
    using System.Linq;
    using Spells;
    using SharpDX;
    using LeagueSharp;
    using LeagueSharp.Common;
    using static Common.Common;

    internal class Combo : Logic
    {
        internal static void Init()
        {
            var target = TargetSelector.GetTarget(Q3.Range, TargetSelector.DamageType.Physical);

            if (target.IsValidTarget(Q3.Range))
            {
                Orbwalker.ForceTarget(target);

                if (Menu.Item("ComboIgnite", true).GetValue<bool>() && Ignite.IsReady()
                    && target.IsValidTarget(Ignite.Range)
                    && (target.Health <= Me.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite)
                    || target.HealthPercent <= 25))
                {
                    Ignite.Cast(target);
                }

                if (Menu.Item("ComboItems", true).GetValue<bool>())
                {
                    UseItems(target, true);
                }

                if (Menu.Item("ComboQ", true).GetValue<bool>() && !IsDashing)
                {
                    if (SpellManager.HaveQ3)
                    {
                        if (Q3.IsReady() && target.IsValidTarget(Q3.Range))
                        {
                            SpellManager.CastQ3();
                        }
                    }
                    else
                    {
                        if (Q.IsReady() && target.IsValidTarget(Q.Range))
                        {
                            var qPred = Q.GetPrediction(target, true);

                            if (qPred.Hitchance >= HitChance.VeryHigh)
                            {
                                Q.Cast(qPred.CastPosition, true);
                            }
                        }
                    }
                }
            }

            if (Menu.Item("ComboW", true).GetValue<bool>())
            {
                if (W.IsReady())
                {
                    ComboW(target);
                }
                else
                {
                    if (Menu.Item("ComboEWall", true).GetValue<bool>() && wallCasted && target.DistanceToPlayer() < 300)
                    {
                        ComboEWall(target);
                    }   
                }
            }

            if (Menu.Item("ComboE", true).GetValue<bool>())
            {
                var dmg = (float) (SpellManager.GetQDmg(target) + SpellManager.GetEDmg(target)) + R.GetDamage(target);

                if (E.IsReady() && target.DistanceToPlayer() >= Menu.Item("ComboERange", true).GetValue<Slider>().Value &&
                    dmg >= target.Health && UnderTower(PosAfterE(target))
                    && SpellManager.CanCastE(target) && Me.IsFacing(target))
                {
                    E.CastOnUnit(target, true);
                }
                else if (target.DistanceToPlayer() >= Menu.Item("ComboERange", true).GetValue<Slider>().Value &&
                         dmg <= target.Health && SpellManager.CanCastE(target) && Me.IsFacing(target))
                {
                    SpellManager.useENormal(target);
                }

                if (Menu.Item("ComboEQ", true).GetValue<bool>() && Q.IsReady() && !SpellManager.HaveQ3 && IsDashing &&
                    target.DistanceToPlayer() <= 275)
                {
                    Utility.DelayAction.Add(200, () => { Q.Cast(target.Position, true); });
                }

                if (Menu.Item("ComboEQ3", true).GetValue<bool>() && Q3.IsReady() && SpellManager.HaveQ3 && IsDashing &&
                    target.DistanceToPlayer() <= 275)
                {
                    Utility.DelayAction.Add(200, () => { Q3.Cast(target.Position, true);});
                }

                switch (Menu.Item("ComboEMode", true).GetValue<StringList>().SelectedIndex)
                {
                    case 0:
                        {
                            if (E.IsReady())
                            {
                                var bestMinion =
                                    ObjectManager.Get<Obj_AI_Base>()
                                        .Where(x => x.DistanceToPlayer() <= E.Range)
                                        .Where(x => x.Distance(target) < target.DistanceToPlayer())
                                        .OrderByDescending(x => x.DistanceToPlayer())
                                        .FirstOrDefault();

                                var dmg2 =
                                    (float)
                                    (SpellManager.GetQDmg(target) + SpellManager.GetEDmg(target)) + R.GetDamage(target);

                                if (bestMinion != null && dmg2 >= target.Health && UnderTower(PosAfterE(bestMinion)) &&
                                    Me.IsFacing(bestMinion) &&
                                    target.Distance(Me) >= Menu.Item("ComboEGap", true).GetValue<Slider>().Value &&
                                    SpellManager.CanCastE(bestMinion) && Me.IsFacing(bestMinion))
                                {
                                    E.CastOnUnit(bestMinion, true);
                                }
                                else if (bestMinion != null && dmg2 <= target.Health && Me.IsFacing(bestMinion) &&
                                         target.Distance(Me) >= Menu.Item("ComboEGap", true).GetValue<Slider>().Value &&
                                         SpellManager.CanCastE(bestMinion) && Me.IsFacing(bestMinion))
                                {
                                    SpellManager.useENormal(bestMinion);
                                }
                            }
                        }
                        break;
                    case 1:
                        {
                            if (E.IsReady())
                            {
                                var bestMinion =
                                    ObjectManager.Get<Obj_AI_Base>()
                                        .Where(x => x.IsValidTarget(E.Range))
                                        .Where(
                                            x =>
                                                x.Distance(Game.CursorPos) <
                                                ObjectManager.Player.Distance(Game.CursorPos))
                                        .OrderByDescending(x => x.Distance(Me))
                                        .FirstOrDefault();

                                var dmg3 =
                                    (float)
                                    (SpellManager.GetQDmg(target) + SpellManager.GetEDmg(target)) + R.GetDamage(target);

                                if (bestMinion != null && dmg3 >= target.Health && UnderTower(PosAfterE(bestMinion)) &&
                                    Me.IsFacing(bestMinion) &&
                                    target.Distance(Me) >= Menu.Item("ComboEGap", true).GetValue<Slider>().Value &&
                                    SpellManager.CanCastE(bestMinion) && Me.IsFacing(bestMinion))
                                {
                                    E.CastOnUnit(bestMinion, true);
                                }
                                else if (bestMinion != null && dmg3 <= target.Health && Me.IsFacing(bestMinion) &&
                                         target.Distance(Me) >= Menu.Item("ComboEGap", true).GetValue<Slider>().Value &&
                                         SpellManager.CanCastE(bestMinion) && Me.IsFacing(bestMinion))
                                {
                                    SpellManager.useENormal(bestMinion);
                                }
                            }
                        }
                        break;
                }
            }

            if (Menu.Item("ComboR", true).GetValue<KeyBind>().Active && R.IsReady())
            {
                var enemies = HeroManager.Enemies.Where(x => x.IsValidTarget(R.Range));

                foreach (var rTarget in enemies)
                {
                    if (rTarget.DistanceToPlayer() <= 1200)
                    {
                        var enemiesKnockedUp =
                            ObjectManager.Get<Obj_AI_Hero>()
                                .Where(x => x.IsValidTarget(R.Range))
                                .Where(x => x.HasBuffOfType(BuffType.Knockup));

                        var enemiesKnocked = enemiesKnockedUp as IList<Obj_AI_Hero> ?? enemiesKnockedUp.ToList();

                        if (rTarget.IsValidTarget(R.Range) && CanCastDelayR(rTarget) &&
                            enemiesKnocked.Count >= Menu.Item("ComboRCount", true).GetValue<Slider>().Value)
                        {
                            R.Cast();
                        }
                    }

                    if (rTarget.IsValidTarget(R.Range))
                    {
                        if (IsKnockedUp(rTarget) && CanCastDelayR(rTarget) &&
                            rTarget.HealthPercent <= Menu.Item("ComboRHp", true).GetValue<Slider>().Value &&
                            Menu.Item("R" + rTarget.ChampionName, true).GetValue<bool>())
                        {
                            R.Cast();
                        }
                        else if (IsKnockedUp(rTarget) && CanCastDelayR(rTarget) &&
                            rTarget.Health >= Menu.Item("ComboRHp", true).GetValue<Slider>().Value &&
                            Menu.Item("ComboRAlly", true).GetValue<bool>())
                        {
                            if (AlliesNearTarget(rTarget, 600))
                            {
                                R.Cast();
                            }
                        }
                    }
                }
            }
        }

        private static void ComboW(Obj_AI_Hero target)
        {
            if (!W.IsReady() || !E.IsReady() || target.IsMelee() ||
                !Menu.Item("ComboW" + target.ChampionName.ToLower(), true).GetValue<bool>())
            {
                return;
            }

            var dashPos = GetNextPos(target);
            var po = Prediction.GetPrediction(target, 0.5f);
            var dist = Me.Distance(po.UnitPosition);

            if (!target.IsMoving || Me.Distance(dashPos) <= dist + 40)
            {
                if (dist < 330 && dist > 100 && W.IsReady())
                {
                    W.Cast(po.UnitPosition, true);
                }
            }
        }

        private static void ComboEWall(Obj_AI_Hero target)
        {
            if (!E.IsReady() || !TargetIsJump(target) || target.IsMelee())
            {
                return;
            }

            var dist = Me.Distance(target);
            var pPos = Me.Position.To2D();
            var dashPos = target.Position.To2D();

            if (!target.IsMoving || Me.Distance(dashPos) <= dist)
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Base>().Where(enemy => TargetIsJump(enemy)))
                {
                    var posAfterE = pPos + Vector2.Normalize(enemy.Position.To2D() - pPos) * E.Range;

                    if ((target.Distance(posAfterE) < dist
                        || target.Distance(posAfterE) < Orbwalking.GetRealAutoAttackRange(target) + 100)
                        && goesThroughWall(target.Position, posAfterE.To3D()))
                    {
                        if (SpellManager.useENormal(target))
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
