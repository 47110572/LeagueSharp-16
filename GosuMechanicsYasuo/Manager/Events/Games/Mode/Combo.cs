namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using System.Collections.Generic;
    using System.Linq;
    using Spells;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = Orbwalking;
    using static Common.Common;

    internal class Combo : Logic
    {
        internal static void Init()
        {
            var target = TargetSelector.GetTarget(1200f, TargetSelector.DamageType.Physical);

            if (target == null)
            {
                return;
            }

            if (target.DistanceToPlayer() > R.Range)
            {
                return;
            }

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

            if (Menu.Item("ComboQ", true).GetValue<bool>())
            {
                if (!IsDashing)
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
                            Q.Cast(target, true);
                        }
                    }
                }
                else
                {
                    if (Menu.Item("ComboEQ", true).GetValue<bool>() && Q.IsReady() && !SpellManager.HaveQ3 &&
                        target.Distance(lastEPos) <= 220)
                    {
                        Utility.DelayAction.Add(150 + Game.Ping, () => { Q.Cast(true); });
                    }

                    if (Menu.Item("ComboEQ3", true).GetValue<bool>() && Q3.IsReady() && SpellManager.HaveQ3 &&
                        target.Distance(lastEPos) <= 220)
                    {
                        Utility.DelayAction.Add(150 + Game.Ping, () => { Q3.Cast(true); });
                    }
                }
            }

            //if (Menu.Item("ComboW", true).GetValue<bool>())
            //{
            //    if (W.IsReady())
            //    {
            //        ComboW(target);
            //    }
            //    else
            //    {
            //        if (Menu.Item("ComboEWall", true).GetValue<bool>() && wallCasted && target.DistanceToPlayer() < 300)
            //        {
            //            ComboEWall(target);
            //        }   
            //    }
            //}

            if (Menu.Item("ComboE", true).GetValue<bool>() && E.IsReady())
            {
                var dmg = (float) (SpellManager.GetQDmg(target)*2 + SpellManager.GetEDmg(target)) +
                          Me.GetAutoAttackDamage(target)*2 +
                          (R.IsReady() ? R.GetDamage(target) : (float) SpellManager.GetQDmg(target));

                if (target.DistanceToPlayer() >= Orbwalking.GetRealAutoAttackRange(Me) + 65 &&
                    dmg >= target.Health && SpellManager.CanCastE(target) &&
                    (Menu.Item("ComboETurret", true).GetValue<bool>() || !UnderTower(PosAfterE(target))))
                {
                    E.CastOnUnit(target, true);
                }
            }

            if (Menu.Item("ComboEGapcloser", true).GetValue<bool>() && E.IsReady() &&
                target.DistanceToPlayer() >= Menu.Item("ComboEGap", true).GetValue<Slider>().Value)
            {
                if (Menu.Item("ComboEMode", true).GetValue<StringList>().SelectedIndex == 0)
                {
                    SpellManager.EGapTarget(target, Menu.Item("ComboETurret", true).GetValue<bool>(),
                        Menu.Item("ComboEGap", true).GetValue<Slider>().Value);
                }
                else
                {
                    SpellManager.EGapMouse(target, Menu.Item("ComboETurret", true).GetValue<bool>(),
                        Menu.Item("ComboEGap", true).GetValue<Slider>().Value);
                }
            }

            if (Menu.Item("ComboR", true).GetValue<KeyBind>().Active && R.IsReady())
            {
                var enemies = HeroManager.Enemies.Where(x => x.IsValidTarget(R.Range));

                foreach (var rTarget in enemies)
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

                    if (IsKnockedUp(rTarget) && CanCastDelayR(rTarget) &&
                        rTarget.HealthPercent <= Menu.Item("ComboRHp", true).GetValue<Slider>().Value &&
                        Menu.Item("R" + rTarget.ChampionName.ToLower(), true).GetValue<bool>())
                    {
                        R.Cast();
                    }

                    if (IsKnockedUp(rTarget) && CanCastDelayR(rTarget) &&
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
}
