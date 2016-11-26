namespace Flowers_ADC_Series
{
    using System;
    using LeagueSharp.Common;

    internal static class Program
    {
        private static void Main(string[] Args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs Args)
        {
            Logic.InitAIO();
        }
    }
}
