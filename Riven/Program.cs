namespace Flowers_Riven_Reborn
{
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Manager.Events;
    using Manager.Menu;
    using Manager.Spells;

    internal class Program
    {
        private static void Main(string[] Args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs Args)
        {
            if (ObjectManager.Player.ChampionName != "Riven")
            {
                return;
            }

            Game.PrintChat("<font color='#00a8ff'>Flowers' Riven Reborn Load! by NightMoon</font>");

            SpellManager.Init();
            MenuManager.Init();
            EventManager.Init();
        }
    }
}
