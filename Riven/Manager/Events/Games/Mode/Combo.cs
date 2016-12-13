namespace Flowers_Riven_Reborn.Manager.Events.Games.Mode
{
    using Spells;
    using myCommon;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = myCommon.Orbwalking;

    internal class Combo : Logic
    {
        internal static void Init()
        {
            var target = TargetSelector.GetSelectedTarget() ??
                TargetSelector.GetTarget(900, TargetSelector.DamageType.Physical);

            if (target.Check(900f))
            {
                if (Menu.GetBool("ComboIgnite") && Ignite != SpellSlot.Unknown && Ignite.IsReady() &&
                    SpellManager.GetComboDamage(target) > target.Health)
                {
                    Me.Spellbook.CastSpell(Ignite, target);
                }

                if (Menu.GetBool("ComboW") && W.IsReady() &&
                    target.IsValidTarget(W.Range) && !target.HasBuffOfType(BuffType.SpellShield) &&
                    (!Q.IsReady() || qStack != 0))
                {
                    W.Cast();
                }

                if (Menu.GetBool("ComboE") && E.IsReady() && Me.CanMoveMent() &&
                    target.DistanceToPlayer() <= W.Range + E.Range &&
                    target.DistanceToPlayer() > Orbwalking.GetRealAutoAttackRange(Me) + 100)
                {
                    E.Cast(target.IsMelee ? Game.CursorPos : target.ServerPosition);
                }

                if (Menu.GetBool("ComboQ") && Q.IsReady() &&
                    (!Me.IsDashing() || (Utils.TickCount - lastETime > 300 && !E.IsReady())) &&
                    target.DistanceToPlayer() <= Q.Range + Orbwalking.GetRealAutoAttackRange(Me) &&
                    !Orbwalking.InAutoAttackRange(target) && Utils.TickCount - lastQTime > 500 && Me.CanMoveMent())
                {
                    SpellManager.CastQ(target);
                }

                if (Menu.GetBool("ComboR") && R.IsReady())
                {
                    if (Menu.GetKey("R1Combo") && R.Instance.Name == "RivenFengShuiEngine" && !E.IsReady())
                    {
                        if (target.DistanceToPlayer() < 500 && Me.CountEnemiesInRange(500) >= 1)
                        {
                            R.Cast();
                        }
                    }
                    else if (R.Instance.Name == "RivenIzunaBlade")
                    {
                        SpellManager.R2Logic(target);
                    }
                }
            }
        }
    }
}
