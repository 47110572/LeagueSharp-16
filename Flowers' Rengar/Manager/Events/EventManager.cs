using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flowers_Rengar.Manager.Events.Games;
using LeagueSharp.Common;
using LeagueSharp;

namespace Flowers_Rengar.Manager.Events
{
    internal class EventManager
    {
        internal static void Init()
        {
            Game.OnUpdate += LoopManager.Init;
        }
    }
}
