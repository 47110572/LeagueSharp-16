namespace Flowers_Yasuo.Manager.Events
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;

    internal class DashReset : Logic
    {
        internal static void Init()
        {
            if (Me.IsDead)
            {
                isDashing = false;
                lastEPos = Vector3.Zero;
            }
            else
            {
                if (isDashing && !Me.IsDashing())
                {
                    isDashing = false;
                }

                if (Utils.TickCount - lastECast - Game.Ping > 500)
                {
                    isDashing = false;
                    lastEPos = Vector3.Zero;
                }
            }
        }
    }
}