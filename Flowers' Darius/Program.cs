namespace Flowers_Darius
{
    using System;
    using Manager.Events;
    using Manager.Menu;
    using Manager.Spells;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Program
    {
        private static void Main(string[] Args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs Args)
        {
            if (ObjectManager.Player.ChampionName != "Darius")
            {
                return;
            }

            Game.PrintChat("<font color='#00a8ff'>Flowers' Darius Load! by NightMoon</font>");

            SpellManager.Init();
            MenuManager.Init();
            EventManager.Init();
        }
    }
}
