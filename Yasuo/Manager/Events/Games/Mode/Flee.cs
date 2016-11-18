namespace Flowers_Yasuo.Manager.Events.Games.Mode
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
            if (IsDashing)
            {
                if (Menu.Item("FleeQ", true).GetValue<bool>() && Q.IsReady() && !SpellManager.HaveQ3)
                {
                    var qMinion = MinionManager.GetMinions(lastEPos, 220, MinionTypes.All, MinionTeam.NotAlly).FirstOrDefault();

                    if (qMinion.IsValidTarget(220))
                    {
                        Utility.DelayAction.Add(50 + Game.Ping / 2, () => Q.Cast(Me.Position, true));
                    }
                }
            }
            else
            {
                if (Menu.Item("FleeE", true).GetValue<bool>() && E.IsReady())
                {
                    var eMinion =
                        MinionManager.GetMinions(Me.Position, E.Range, MinionTypes.All, MinionTeam.NotAlly)
                            .Where(x => x.Distance(Game.CursorPos) < x.DistanceToPlayer())
                            .OrderByDescending(x => x.DistanceToPlayer())
                            .FirstOrDefault();

                    if (eMinion != null && SpellManager.CanCastE(eMinion) && E.IsReady())
                    {
                        E.CastOnUnit(eMinion, true);
                    }
                }
            }
        }
    }
}
