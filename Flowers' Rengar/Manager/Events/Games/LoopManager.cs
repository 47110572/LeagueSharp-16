using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowers_Rengar.Manager.Events.Games
{
    internal class LoopManager : Logic
    {
        internal static void Init(EventArgs args)
        {
            Skin.Init();

            if (Me.IsDead || Me.IsRecalling())
            {
                return;
            }
        }
    }
}
