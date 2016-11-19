namespace Flowers_Rengar.Manager.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class MenuManager : Logic
    {
        internal static void Init()
        {
            Menu = new Menu("Flowers' Rengar", "Flowers' Rengar", true);

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
            }

            Menu.AddItem(new MenuItem("asd ad asd ", " ", true));
            Menu.AddItem(new MenuItem("Credit", "Credit: NightMoon", true));

            Menu.AddToMainMenu();
        }
    }
}
