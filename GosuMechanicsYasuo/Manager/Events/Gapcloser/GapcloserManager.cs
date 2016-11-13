namespace GosuMechanicsYasuo.Manager.Events
{
    using Spells;
    using LeagueSharp.Common;

    internal class GapcloserManager : Logic
    {
        internal static void Init(ActiveGapcloser Args)
        {
            if (Menu.Item("Q3Anti", true).GetValue<bool>())
            {
                var target = Args.Sender;

                if (target.IsValidTarget(400) && Q3.IsReady() && SpellManager.HaveQ3)
                {
                    Q3.Cast(target);
                }
            }
        }
    }
}