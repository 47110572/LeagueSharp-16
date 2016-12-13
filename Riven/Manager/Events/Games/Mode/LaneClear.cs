namespace Flowers_Riven_Reborn.Manager.Events.Games.Mode
{
    using myCommon;
    using LeagueSharp.Common;

    internal class LaneClear : Logic
    {
        internal static void Init()
        {
            if (!Me.UnderTurret(true) && Menu.GetBool("LaneClearW") && W.IsReady())
            {
                var minions = MinionManager.GetMinions(Me.ServerPosition, W.Range);

                if (minions.Count >= 3)
                {
                    W.Cast(true);
                }
            }
        }
    }
}
