namespace Flowers_Fiora
{
    using System;
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
            if (ObjectManager.Player.ChampionName != "Fiora")
            {
                return;
            }

            Logic.Load();
        }
    }
}
