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
                Game.PrintChat("Flowers' Riven Not Load!");
                return;
            }

            SpellManager.Init();
            MenuManager.Init();
            EventManager.Init();
        }
    }
}
