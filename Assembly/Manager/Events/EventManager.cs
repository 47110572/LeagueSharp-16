namespace Flowers_XX.Manager.Events
{
    using Games;
    using Drawings;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class EventManager
    {
        internal static void Init()
        {
            Game.OnUpdate += LoopManager.Init;
            Drawing.OnDraw += DrawManager.Init;
        }
    }
}
