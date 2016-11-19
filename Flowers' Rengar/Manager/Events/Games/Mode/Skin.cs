namespace Flowers_Rengar.Manager.Events
{
    using LeagueSharp.Common;
    using LeagueSharp;

    internal class Skin : Logic
    {
        internal static void Init()
        {
            if (Menu.Item("EnableSkin", true).GetValue<bool>())
            {
                ObjectManager.Player.SetSkin(ObjectManager.Player.ChampionName,
                    Menu.Item("SelectSkin", true).GetValue<StringList>().SelectedIndex);
            }
        }
    }
}