namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using Common;
    using System.Linq;
    using Spells;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Flee : Logic
    {
        internal static void Init()
        {
            if (Menu.Item("FleeE", true).GetValue<bool>() && E.IsReady())
            {
                var bestMinion =
                    MinionManager.GetMinions(Me.Position, E.Range, MinionTypes.All, MinionTeam.NotAlly)
                        .Where(x => x.Distance(Game.CursorPos) < x.DistanceToPlayer())
                        .OrderByDescending(x => x.DistanceToPlayer())
                        .FirstOrDefault();

                if (bestMinion != null && Me.IsFacing(bestMinion) && SpellManager.CanCastE(bestMinion) && E.IsReady())
                {
                    E.CastOnUnit(bestMinion, true);
                }
            }

            if (Menu.Item("FleeQ", true).GetValue<bool>() && Q.IsReady())
            {
                var qMinion =
                    MinionManager.GetMinions(Me.Position, Q.Range, MinionTypes.All, MinionTeam.NotAlly).FirstOrDefault();

                if (qMinion.IsValidTarget(Q.Range) && !SpellManager.HaveQ3 && IsDashing)
                {
                    var qPred = Q.GetPrediction(qMinion, true);

                    if (qPred.Hitchance >= HitChance.VeryHigh)
                    {
                        Q.Cast(qPred.CastPosition, true);
                    }
                }
            }
        }
    }
}
