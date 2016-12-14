namespace Flowers_Darius.Manager.Events.Games.Mode
{
    using myCommon;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = myCommon.Orbwalking;

    internal class QMoveMent : Logic
    {
        internal static void Init()
        {
            if (Me.HasBuff("dariusqcast"))
            {
                Orbwalker.SetAttack(false);

                if (Me.CountEnemiesInRange(800) >= 3)
                {
                    return;
                }

                if (Orbwalking.isCombo)
                {
                    if (Menu.GetBool("ComboQLock"))
                    {
                        Orbwalker.SetMovement(false);

                        var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

                        if (target.DistanceToPlayer() <= 250)
                        {
                            Me.IssueOrder(GameObjectOrder.MoveTo ,Me.Position.Extend(target.Position, -300));
                            return;
                        }

                        if (target.DistanceToPlayer() <= Q.Range + 50)
                        {
                            Me.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                            return;
                        }
                    }
                    else
                    {
                        Orbwalker.SetMovement(true);
                    }
                }

                if (Orbwalking.isHarass)
                {
                    if (Menu.GetBool("HarassQLock"))
                    {
                        Orbwalker.SetMovement(true);

                        var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

                        if (target.DistanceToPlayer() <= 250)
                        {
                            Me.IssueOrder(GameObjectOrder.MoveTo, Me.Position.Extend(target.Position, -Q.Range));
                            return;
                        }

                        if (target.DistanceToPlayer() <= Q.Range + 50)
                        {
                            Me.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                        }
                    }
                    else
                    {
                        Orbwalker.SetMovement(true);
                    }
                }
            }
            else
            {
                Orbwalker.SetMovement(true);
                Orbwalker.SetAttack(true);
            }
        }
    }
}
