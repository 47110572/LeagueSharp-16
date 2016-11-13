namespace GosuMechanicsYasuo.Manager.Menu
{
    using Evade;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = Orbwalking;

    internal class MenuManager : Logic
    {
        internal static void Init()
        {
            Menu = new Menu("GosuMechanics Yasuo Rework", "GosuMechanics Yasuo Rework", true);

            var orbMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            {
                Orbwalker = new Orbwalking.Orbwalker(orbMenu);
            }

            var comboMenu = Menu.AddSubMenu(new Menu("Combo", "combo"));
            {
                comboMenu.AddItem(new MenuItem("ComboQ", "Use Q", true)).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboW", "Use W", true)).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboE", "Use E")).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboEWall", "Use E to Wall Position", true)).SetValue(true);
                comboMenu.AddItem(
                    new MenuItem("ComboERange", "Use E| Target Distance to Player >= x", true)).SetValue(
                        new Slider(375, 0, 475));
                comboMenu.AddItem(
                    new MenuItem("ComboEGap", "Use E GapCloser| Target Distance to Player >=x", true)).SetValue(
                        new Slider(230, 0, 1300));
                comboMenu.AddItem(
                    new MenuItem("ComboEMode", "Use E Mode: ", true)).SetValue(new StringList(new[] {"Target", "Mouse"}));
                comboMenu.AddItem(new MenuItem("ComboEQ", "Use EQ", true)).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboEQ3", "Use EQ3", true)).SetValue(true);
                comboMenu.AddItem(
                    new MenuItem("ComboR", "Use R", true)).SetValue(new KeyBind('R', KeyBindType.Toggle, true));
                comboMenu.AddItem(
                    new MenuItem("ComboRHp", "Use R|When target HealthPercent <= x%", true)).SetValue(new Slider(50));
                comboMenu.AddItem(
                    new MenuItem("ComboRCount", "Use R|When knockedUp enemy Count >= x", true)).SetValue(
                        new Slider(2, 1, 5));
                comboMenu.AddItem(
                    new MenuItem("ComboRAlly", "Use R| When Have Ally In Range", true)).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboIgnite", "Use Ignite", true)).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboItems", "Use Items", true)).SetValue(true);
            }

            var harassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            {
                harassMenu.AddItem(new MenuItem("HarassQ", "Use Q", true)).SetValue(true);
                harassMenu.AddItem(new MenuItem("HarassQ3", "Use Q3", true)).SetValue(true);
                harassMenu.AddItem(new MenuItem("HarassTower", "Under Tower", true)).SetValue(true);
            }

            var laneClearMenu = Menu.AddSubMenu(new Menu("LaneClear", "LaneClear"));
            {
                laneClearMenu.AddItem(new MenuItem("LaneClearQ", "Use Q", true)).SetValue(true);
                laneClearMenu.AddItem(new MenuItem("LaneClearQ3", "Use Q3", true)).SetValue(true);
                laneClearMenu.AddItem(
                    new MenuItem("LaneClearQ3count", "Use Q3| Hit Minions >= x", true)).SetValue(new Slider(3, 1, 5));
                laneClearMenu.AddItem(new MenuItem("LaneClearE", "Use E", true)).SetValue(true);
                laneClearMenu.AddItem(new MenuItem("LaneClearETurret", "Use E Under Turret", true)).SetValue(false);
                laneClearMenu.AddItem(new MenuItem("LaneClearItems", "Use Items", true)).SetValue(true);
            }

            var jungleClearMenu = Menu.AddSubMenu(new Menu("JungleClear", "JungleClear"));
            {
                jungleClearMenu.AddItem(new MenuItem("JungleClearQ", "Use Q", true)).SetValue(true);
                jungleClearMenu.AddItem(new MenuItem("JungleClearQ3", "Use Q3", true)).SetValue(true);
                jungleClearMenu.AddItem(new MenuItem("JungleClearE", "Use E", true)).SetValue(true);
            }

            var lastHitMenu = Menu.AddSubMenu(new Menu("LastHit", "LastHit"));
            {
                lastHitMenu.AddItem(new MenuItem("LastHitQ", "Use Q", true)).SetValue(true);
                lastHitMenu.AddItem(new MenuItem("LastHitQ3", "Use Q3", true)).SetValue(true);
                lastHitMenu.AddItem(new MenuItem("LastHitE", "Use E", true)).SetValue(true);
            }

            var fleeMenu = Menu.AddSubMenu(new Menu("Flee", "Flee"));
            {
                fleeMenu.AddItem(new MenuItem("FleeQ", "Use Q", true).SetValue(true));
                fleeMenu.AddItem(new MenuItem("FleeE", "Use E", true).SetValue(true));
            }

            var miscMenu = Menu.AddSubMenu(new Menu("Misc", "Misc"));
            {
                var qMenu = miscMenu.AddSubMenu(new Menu("Q Settings", "Q Settings"));
                {
                    qMenu.AddItem(new MenuItem("KillStealQ", "Use Q KillSteal", true).SetValue(true));
                    qMenu.AddItem(new MenuItem("KillStealQ3", "Use Q3 KillSteal", true).SetValue(true));
                    qMenu.AddItem(new MenuItem("Q3Int", "Use Q3 Interrupter", true)).SetValue(true);
                    qMenu.AddItem(new MenuItem("Q3Anti", "Use Q3 AntiGapcloser", true)).SetValue(true);
                    qMenu.AddItem(new MenuItem("AutoQ1", "Use Q Stack while Dashing")).SetValue(true);
                    qMenu.AddItem(
                        new MenuItem("AutoQ", "Auto Q", true)).SetValue(new KeyBind('L', KeyBindType.Toggle, true));
                    qMenu.AddItem(
                        new MenuItem("AutoQ3", "Auto Q3", true)).SetValue(false);
                }

                var wMenu = miscMenu.AddSubMenu(new Menu("W Settings", "W Settings"));
                {
                    var wWhitelistMenu = Menu.AddSubMenu(new Menu("Combo W Target", "Combo W Target"));
                    {
                        foreach (var hero in HeroManager.Enemies)
                        {
                            wWhitelistMenu.AddItem(
                                new MenuItem("ComboW" + hero.ChampionName.ToLower(), hero.ChampionName, true)).SetValue(true);
                        }
                    }
                    wMenu.AddItem(new MenuItem("smartW", "Use W Dodge Spell", true)).SetValue(true);
                    wMenu.AddItem(new MenuItem("smartWDanger", "if Spell DangerLevel >=", true)).SetValue(new Slider(3, 1, 5));
                    wMenu.AddItem(
                        new MenuItem("smartWDelay", "WindWall Humanizer (500 = Lowest Reaction Time)", true)).SetValue(
                            new Slider(3000, 500, 3000));
                    wMenu.AddItem(new MenuItem("wwDanger", "Block only dangerous", true)).SetValue(false);
                }

                var eMenu = miscMenu.AddSubMenu(new Menu("E Settings", "E Settings"));
                {
                    eMenu.AddItem(new MenuItem("KillStealE", "Use E KillSteal", true).SetValue(true));
                    eMenu.AddItem(new MenuItem("ETower", "Dont Jump turrets", true)).SetValue(true);
                    eMenu.AddItem(new MenuItem("smartEDogue", "Use E Dodge Spell", true)).SetValue(true);
                    eMenu.AddItem(
                        new MenuItem("smartEDogueDanger", "if Spell DangerLevel >=", true)).SetValue(new Slider(1, 1, 5));
                }

                var rMenu = miscMenu.AddSubMenu(new Menu("R Settings", "R Settings"));
                {
                    var rWhitelist = rMenu.AddSubMenu(new Menu("R Whitelist", "R Whitelist"));
                    {
                        foreach (var hero in HeroManager.Enemies)
                        {
                            rWhitelist.AddItem(
                                new MenuItem("R" + hero.ChampionName.ToLower(), hero.ChampionName, true)).SetValue(true);
                        }
                    }

                    var autoR = rMenu.AddSubMenu(new Menu("Auto R", "Auto R"));
                    {
                        autoR.AddItem(new MenuItem("AutoR", "Auto R", true)).SetValue(true);
                        autoR.AddItem(
                            new MenuItem("AutoRCount", "Auto R|When knockedUp enemy Count >= x", true)).SetValue(
                                new Slider(3, 1, 5));
                        autoR.AddItem(
                            new MenuItem("AutoRRangeCount", "Auto R|When all Enemy Count >= x", true)).SetValue(
                                new Slider(2, 1, 5));
                        autoR.AddItem(
                            new MenuItem("AutoRMyHp", "Auto R|When Player HealthPercent >= x%", true)).SetValue(
                                new Slider(50));
                    }
                }

                var spellSettings = miscMenu.AddSubMenu(new Menu("Dodge Spells", "Dodge Spells"));
                {
                    foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
                    {
                        if (hero.Team != ObjectManager.Player.Team)
                        {
                            foreach (var spell in SpellDatabase.Spells)
                            {
                                if (spell.ChampionName == hero.ChampionName)
                                {
                                    var subMenu = spellSettings.AddSubMenu(new Menu(spell.MenuItemName, spell.MenuItemName));

                                    subMenu.AddItem(
                                        new MenuItem("DangerLevel" + spell.MenuItemName, "Danger level", true).SetValue(
                                            new Slider(spell.DangerValue, 5, 1)));

                                    subMenu.AddItem(
                                        new MenuItem("IsDangerous" + spell.MenuItemName, "Is Dangerous", true).SetValue(
                                            spell.IsDangerous));

                                    subMenu.AddItem(
                                        new MenuItem("Enabled" + spell.MenuItemName, "Enabled", true).SetValue(true));
                                }
                            }
                        }
                    }
                }

                miscMenu.AddItem(new MenuItem("KS", "KillSteal")).SetValue(true);
            }

            var drawMenu = Menu.AddSubMenu(new Menu("Draw", "Draw"));
            {
                drawMenu.AddItem(new MenuItem("DrawQ", "Draw Q Range", true)).SetValue(true);
                drawMenu.AddItem(new MenuItem("DrawQ3", "Draw Q3 Range", true)).SetValue(true);
                drawMenu.AddItem(new MenuItem("DrawW", "Draw W Range", true)).SetValue(true);
                drawMenu.AddItem(new MenuItem("DrawE", "Draw E Range", true)).SetValue(true);
                drawMenu.AddItem(new MenuItem("DrawR", "Draw R Range", true)).SetValue(true);
                drawMenu.AddItem(new MenuItem("DrawSpots", "Draw WallJump Spots", true)).SetValue(true);
            }

            Menu.AddItem(new MenuItem("Credit", "Credit: tulisan69", true));
            Menu.AddItem(new MenuItem("Rework", "Rework: NightMoon", true));

            Menu.AddToMainMenu();
        }
    }
}