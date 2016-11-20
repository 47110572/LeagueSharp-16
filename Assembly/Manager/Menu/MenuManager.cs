namespace Flowers_XX.Manager.Menu
{
    using LeagueSharp.Common;

    internal class MenuManager : Logic
    {
        internal static void Init()
        {
            Menu = new Menu("Flowers' XX", "Flowers' XX", true);

            var miscMenu = Menu.AddSubMenu(new Menu("Misc", "Misc"));
            {
                var skinMenu = miscMenu.AddSubMenu(new Menu("SkinChange", "SkinChange"));
                {
                    skinMenu.AddItem(new MenuItem("EnableSkin", "Enabled", true).SetValue(false)).ValueChanged += EnbaleSkin;
                    skinMenu.AddItem(
                        new MenuItem("SelectSkin", "Select Skin: ", true).SetValue(
                            new StringList(new[]
                            {
                                "Classic", "Head Hunter", "Night Hunter", "SSW"
                            })));
                }

                var autoWardMenu = miscMenu.AddSubMenu(new Menu("Auto Ward", "Auto Ward"));
                {
                    autoWardMenu.AddItem(new MenuItem("AutoWardEnable", "Enabled", true).SetValue(true));
                    autoWardMenu.AddItem(new MenuItem("OnlyCombo", "Only Combo Mode Active", true).SetValue(true));
                }
            }

            Menu.AddItem(new MenuItem("asd ad asd ", " ", true));
            Menu.AddItem(new MenuItem("Credit", "Credit: NightMoon", true));

            Menu.AddToMainMenu();
        }
    }
}
