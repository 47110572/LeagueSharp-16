namespace Flowers_Darius.Manager.Events.Games.Mode
{
    using Spells;
    using myCommon;
    using LeagueSharp.Common;
    using Orbwalking = myCommon.Orbwalking;

    internal class Harass : Logic
    {
        internal static void Init()
        {
            if (ManaManager.HasEnoughMana(Menu.GetSlider("HarassMana")))
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

                if (target.Check(E.Range))
                {
                    if (Menu.GetBool("HarassE") && E.IsReady() && !Orbwalking.InAutoAttackRange(target) &&
                        !target.HaveShiled() && target.DistanceToPlayer() <= E.Range - 30)
                    {
                        var pred = E.GetPrediction(target);
                        E.Cast(pred.UnitPosition, true);
                    }

                    if (Menu.GetBool("HarassQ") && Q.IsReady() && !Orbwalking.InAutoAttackRange(target) &&
                        target.DistanceToPlayer() <= Q.Range && SpellManager.CanQHit(target))
                    {
                        Q.Cast(true);
                    }
                }
            }
        }
    }
}
