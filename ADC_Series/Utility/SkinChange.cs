namespace Flowers_ADC_Series.Utility
{
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Script.Serialization;

    internal static class SkinChange
    {
        private static int SkinID;

        private static readonly Menu Menu = Program.Utilitymenu;
        private static readonly Obj_AI_Hero Me = Program.Me;

        private static readonly WebClient Webclient = new WebClient();
        private static readonly JavaScriptSerializer JSerializer = new JavaScriptSerializer();
        private static readonly List<SkinInfo> SkinList = new List<SkinInfo>();

        private class SkinInfo
        {
            internal string Name;
        }

        static SkinChange()
        {
            var versionJson = Webclient.DownloadString("http://ddragon.leagueoflegends.com/realms/na.json");
            var gameVersion =
                (string)
                ((Dictionary<string, object>)JSerializer.Deserialize<Dictionary<string, object>>(versionJson)["n"])
                    ["champion"];
            var champJson =
                Webclient.DownloadString(
                    $"http://ddragon.leagueoflegends.com/cdn/{gameVersion}/data/en_US/champion/{ObjectManager.Player.ChampionName}.json");
            var skins =
                (ArrayList)
                ((Dictionary<string, object>)
                ((Dictionary<string, object>)
                    JSerializer.Deserialize<Dictionary<string, object>>(champJson)["data"])[
                    ObjectManager.Player.ChampionName])["skins"];

            foreach (Dictionary<string, object> skin in skins)
            {
                SkinList.Add(new SkinInfo
                {
                    Name = skin["name"].ToString().Contains("default")
                        ? ObjectManager.Player.ChampionName
                        : skin["name"].ToString()
                });
            }
        }

        public static void Init()
        {
            SkinID = Me.BaseSkinId;

            var SkinMenu = Menu.AddSubMenu(new Menu("Skin Change", "Skin Change"));
            {
                SkinMenu.AddItem(new MenuItem("EnableSkin", "Enabled", true).SetValue(false))
                    .DontSave().ValueChanged += EnbaleSkin;
                SkinMenu.AddItem(
                    new MenuItem("SelectSkin", "Select Skin: ", true).SetValue(
                        new StringList(SkinList.Select(x => x.Name).ToArray())));
            }

            Game.OnUpdate += OnUpdate;
        }

        private static void EnbaleSkin(object obj, OnValueChangeEventArgs Args)
        {
            if (!Args.GetNewValue<bool>())
            {
                Me.SetSkin(Me.ChampionName, SkinID);
            }
        }

        private static void OnUpdate(EventArgs Args)
        {
            if (Me.IsDead)
            {
                return;
            }

            if (Menu.Item("EnableSkin", true).GetValue<bool>())
            {
                Me.SetSkin(Me.ChampionName,
                    Menu.Item("SelectSkin", true).GetValue<StringList>().SelectedIndex);
            }
        }
    }

    //internal class SkinChange
    //{
    //    private static int SkinID;

    //    private static readonly Menu Menu = Program.Menu;
    //    private static readonly Obj_AI_Hero Me = Program.Me;

    //    private static string[] SkinName
    //    {
    //        get
    //        {
    //            switch (ObjectManager.Player.ChampionName)
    //            {
    //                case "Ashe":
    //                    return new[]
    //                    {
    //                        "Classic", "Freljord Ashe", "Sherwood Forest Ashe",
    //                        "Woad Ashe", "Queen Ashe", "Amethyst Ashe", "Heartseeker Ashe",
    //                        "Marauder Ashe", "Project Ashe"
    //                    };
    //                case "Caitlyn":
    //                    return new[]
    //                    {
    //                        "Classic", "Resistance Caitlyn", "Sheriff Caitlyn",
    //                        "Safari Caitlyn", "Arctic Warfare Caitlyn", "Officer Caitlyn",
    //                        "Headhunter Caitlyn", "Lunar Wraith Caitlyn"
    //                    };
    //                case "Corki":
    //                    return new[]
    //                    {
    //                        "Classic", "UFO Corki", "Ice Toboggan Corki", "Red Baron Corki",
    //                        "Hot Rod Corki", "Urfrider Corki", "Dragonwing Corki", "Fnatic Corki"
    //                    };
    //                case "Draven":
    //                    return new[]
    //                    {
    //                        "Classic", "Soul Reaver Draven", "Gladiator Draven", "Primetime Draven",
    //                        "Pool Party Draven", "Beast Hunter Draven", "Draven Draven"
    //                    };
    //                case "Ezreal":
    //                    return new[]
    //                    {
    //                        "Classic", "Nottingham Ezreal", "Striker Ezreal", "Frosted Ezreal",
    //                        "Explorer Ezreal", "Pulsefire Ezreal", "TPA Ezreal", "Debonair Ezreal",
    //                        "Ace of Spades Ezreal"
    //                    };
    //                case "Graves":
    //                    return new[]
    //                    {
    //                        "Classic", "Hired Gun Graves", "Jailbreak Graves", "Mafia Graves", "Riot Graves",
    //                        "Pool Party Graves", "Cutthroat Graves"
    //                    };
    //                case "Jhin":
    //                    return new[]
    //                    {
    //                        "Classic", "High Noon Jhin"
    //                    };
    //                case "Jinx":
    //                    return new[]
    //                    {
    //                        "Classic", "Mafia Jinx", "Firecracker Jinx", "Slayer Jinx"
    //                    };
    //                case "Kalista":
    //                    return new[]
    //                    {
    //                        "Classic", "Blood Moon Kalista", "Championship Kalista"
    //                    };
    //                case "KogMaw":
    //                    return new[]
    //                    {
    //                        "Classic", "Caterpillar Kog'Maw", "Sonoran Kog'Maw", "Monarch Kog'Maw",
    //                        "Reindeer Kog'Maw", "Lion Dance Kog'Maw", "Deep Sea Kog'Maw",
    //                        "Jurassic Kog'Maw", "Battlecast Kog'Maw"
    //                    };
    //                case "Lucian":
    //                    return new[]
    //                    {
    //                        "Classic", "Hired Gun Lucian", "Striker Lucian", "PROJECT: Lucian"
    //                    };
    //                case "MissFortune":
    //                    return new[]
    //                    {
    //                        "Classic", "Cowgirl Miss Fortune", "Waterloo Miss Fortune",
    //                        "Secret Agent Miss Fortune", "Candy Cane Miss Fortune", "Road Warrior Miss Fortune",
    //                        "Mafia Miss Fortune", "Arcade Miss Fortune", "Captain Fortune"
    //                    };
    //                case "Quinn":
    //                    return new[]
    //                    {
    //                        "Classic", "Phoenix Quinn", "Woad Scout Quinn", "Corsair Quinn"
    //                    };
    //                case "Sivir":
    //                    return new[]
    //                    {
    //                        "Classic", "Warrior Princess Sivir", "Spectacular Sivir",
    //                        "Huntress Sivir", "Bandit Sivir", "PAX Sivir", "Snowstorm Sivir",
    //                        "Warden Sivir", "Victorious Sivir"
    //                    };
    //                case "Tristana":
    //                    return new[]
    //                    {
    //                        "Classic", "Riot Girl Tristana", "Earnest Elf Tristana",
    //                        "Firefighter Tristana", "Guerilla Tristana", "Buccaneer Tristana",
    //                        "Rocket Girl Tristana", "Dragon Trainer Tristana"
    //                    };
    //                case "Twitch":
    //                    return new[]
    //                    {
    //                        "Classic", "Kingpin Twitch", "Whistler Village Twitch", "Medieval Twitch",
    //                        "Gangster Twitch", "Vandal Twitch", "Pickpocket Twitch", "SSW Twitch"
    //                    };
    //                case "Urgot":
    //                    return new[]
    //                    {
    //                        "Classic", "Giant Enemy Crabgot", "Butcher Urgot", "Battlecast Urgot"
    //                    };
    //                case "Varus":
    //                    return new[]
    //                    {
    //                        "Classic", "Blight Crystal Varus", "Arclight Varus", "Arctic Ops Varus",
    //                        "Heartseeker Varus", "Varus Swiftbolt", "Dark Star Varus"
    //                    };
    //                case "Vayne":
    //                    return new[]
    //                    {
    //                        "Classic", "Vindicator Vayne", "Aristocrat Vayne", "Dragonslayer Vayne",
    //                        "Heartseeker Vayne", "SKT T1 Vayne", "Arclight Vayne"
    //                    };
    //                default:
    //                    return new[] { "Classic", "1", "2", "3", "4", "5", "6", "7" };
    //            }
    //        }
    //    }

    //    public static void Init()
    //    {
    //        SkinID = Me.BaseSkinId;

    //        var SkinMenu = Menu.AddSubMenu(new Menu("Skin Change", "Skin Change"));
    //        {
    //            SkinMenu.AddItem(new MenuItem("EnableSkin", "Enabled", true).SetValue(false))
    //                .DontSave().ValueChanged += EnbaleSkin;
    //            SkinMenu.AddItem(
    //                new MenuItem("SelectSkin", "Select Skin: ", true).SetValue(
    //                    new StringList(SkinName)));
    //        }

    //        Game.OnUpdate += OnUpdate;
    //    }

    //    private static void EnbaleSkin(object obj, OnValueChangeEventArgs Args)
    //    {
    //        if (!Args.GetNewValue<bool>())
    //        {
    //            Me.SetSkin(Me.ChampionName, SkinID);
    //        }
    //    }

    //    private static void OnUpdate(EventArgs Args)
    //    {
    //        if (Me.IsDead)
    //        {
    //            return;
    //        }

    //        if (Menu.Item("EnableSkin", true).GetValue<bool>())
    //        {
    //            Me.SetSkin(Me.ChampionName,
    //                Menu.Item("SelectSkin", true).GetValue<StringList>().SelectedIndex);
    //        }
    //    }
    //}
}
