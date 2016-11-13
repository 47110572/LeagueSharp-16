namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using Spells;
    using LeagueSharp.Common;

    internal class Harass : Logic
    {
        internal static void Init()
        {
            var target = TargetSelector.GetTarget(Q3.Range, TargetSelector.DamageType.Physical);

            if (target.IsValidTarget(Q3.Range))
            {
                if (Menu.Item("HarassTower", true).GetValue<bool>() || !Me.UnderTurret(true))
                {
                    if (!IsDashing)
                    {
                        if (Menu.Item("HarassQ", true).GetValue<bool>() && Q.IsReady() && target.IsValidTarget(Q.Range) 
                            && !SpellManager.HaveQ3)
                        {
                            var qPred = Q.GetPrediction(target, true);

                            if (qPred.Hitchance >= HitChance.VeryHigh)
                            {
                                Q.Cast(qPred.CastPosition, true);
                            }
                        }

                        if (Menu.Item("HarassQ3", true).GetValue<bool>() && Q3.IsReady() && target.IsValidTarget(Q3.Range) 
                            && SpellManager.HaveQ3)
                        {
                            var q3Pred = Q3.GetPrediction(target, true);

                            if (q3Pred.Hitchance >= HitChance.VeryHigh)
                            {
                                Q3.Cast(q3Pred.CastPosition, true);
                            }
                        }
                    }
                }
            }
        }
    }
}
