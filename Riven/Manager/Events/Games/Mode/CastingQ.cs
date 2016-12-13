namespace Flowers_Riven_Reborn.Manager.Events.Games.Mode
{
    using myCommon;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class CastingQ : Logic
    {
        internal static void Init()
        {
            if (qTarget != null && canQ)
            {
                switch (Menu.GetList("QMode"))
                {
                    case 0:
                        Q.Cast(qTarget.Position, true);
                        break;
                    case 1:
                        Q.Cast(Game.CursorPos, true);
                        break;
                    case 2:
                        Q.Cast(Me.Position.Extend(qTarget.Position, Q.Range), true);
                        break;
                    default:
                        Q.Cast(Me.Position.Extend(Game.CursorPos, Q.Range), true);
                        break;
                }
            }
        }
    }
}
