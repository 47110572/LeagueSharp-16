namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using Spells;
    using System.Linq;
    using LeagueSharp.Common;

    internal class KillSteal : Logic
    {
        internal static void Init()
        {
            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(Q3.Range)))
            {
                if (Menu.Item("KillStealQ3", true).GetValue<bool>() && target.IsValidTarget(Q3.Range) && Q3.IsReady() &&
                    SpellManager.HaveQ3 && target.Health <= SpellManager.GetQDmg(target))
                {
                    var q3Pred = Q3.GetPrediction(target, true);

                    if (q3Pred.Hitchance >= HitChance.VeryHigh)
                    {
                        Q3.Cast(q3Pred.CastPosition, true);
                        return;
                    }
                }

                if (Menu.Item("KillStealQ", true).GetValue<bool>() && target.IsValidTarget(Q.Range) && Q.IsReady() &&
                    !SpellManager.HaveQ3 && target.Health <= SpellManager.GetQDmg(target))
                {
                    var qPred = Q.GetPrediction(target, true);

                    if (qPred.Hitchance >= HitChance.VeryHigh)
                    {
                        Q.Cast(qPred.CastPosition, true);
                        return;
                    }
                }

                if (Menu.Item("KillStealE", true).GetValue<bool>() && target.IsValidTarget(E.Range) && E.IsReady() &&
                    SpellManager.CanCastE(target) && target.Health <= SpellManager.GetEDmg(target))
                {
                    E.CastOnUnit(target, true);
                    return;
                }
            }
        }
    }
}
