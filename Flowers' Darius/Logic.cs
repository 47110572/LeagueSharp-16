namespace Flowers_Darius
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = myCommon.Orbwalking;

    internal class Logic
    {
        internal static int lastETime;
        internal static Spell Q, W, E, R;
        internal static SpellSlot Ignite = SpellSlot.Unknown;
        internal static Menu Menu;
        internal static Obj_AI_Hero Me = ObjectManager.Player;
        internal static Orbwalking.Orbwalker Orbwalker;
    }
}
