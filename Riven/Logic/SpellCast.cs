namespace Flowers_Riven
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class SpellCast
    {
        public static void Init()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnPlayAnimation += OnPlayAnimation;
            AntiGapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += OnInterruptableTarget;
        }

        private static void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (Program.Menu.Item("AntiGapCloserW", true).GetValue<bool>() && Program.W.IsReady())
            {
                if (gapcloser.Sender.IsValidTarget(Program.W.Range) && Program.Me.CountEnemiesInRange(1500) < 3)
                {
                    Program.W.Cast();
                }
            }
        }

        private static void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs Args)
        {
            if (Program.Menu.Item("InterruptTargetW", true).GetValue<bool>() && Program.W.IsReady())
            {
                if (sender.IsValidTarget(Program.W.Range) && !sender.ServerPosition.UnderTurret(true))
                {
                    Program.W.Cast();
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs Args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (Args.SData.Name)
            {
                case "ItemTiamatCleave":
                    switch (Program.Orbwalker.ActiveMode)
                    {
                        case Orbwalking.OrbwalkingMode.Combo:
                            if (Program.Menu.Item("ComboW", true).GetValue<bool>() && Program.W.IsReady() &&
                                Program.Me.CountEnemiesInRange(Program.W.Range - 50) > 0)
                            {
                                Program.W.Cast();
                            }
                            break;
                        case Orbwalking.OrbwalkingMode.Burst:
                            if (Program.Me.CountEnemiesInRange(Program.W.Range - 50) > 0)
                            {
                                Program.W.Cast();
                            }
                            break;
                        case Orbwalking.OrbwalkingMode.Mixed:
                            if (Program.Menu.Item("HarassW", true).GetValue<bool>() && Program.W.IsReady() &&
                                Program.Me.CountEnemiesInRange(Program.W.Range - 50) > 0)
                            {
                                Program.W.Cast();
                            }
                            break;
                        case Orbwalking.OrbwalkingMode.QuickHarass:
                            if (Program.Me.CountEnemiesInRange(Program.W.Range - 50) > 0)
                            {
                                Program.W.Cast();
                            }
                            break;
                    }
                    break;
                case "RivenTriCleave":
                    Program.CanQ = false;

                    if (Program.Me.CountEnemiesInRange(400) == 0)
                    {
                        return;
                    }

                    switch (Program.Orbwalker.ActiveMode)
                    {
                        case Orbwalking.OrbwalkingMode.Combo:
                            Program.CastItem(true);

                            if (Program.Menu.Item("ComboR", true).GetValue<bool>() && Program.R.IsReady())
                            {
                                var target = TargetSelector.GetSelectedTarget() ??
                                             TargetSelector.GetTarget(Program.R.Range, TargetSelector.DamageType.Physical);

                                if (target.IsValidTarget(Program.R.Range))
                                {
                                    LoopEvent.R2Logic(target);
                                }
                            }
                            break;
                        case Orbwalking.OrbwalkingMode.Burst:
                            Program.CastItem(true);

                            if (Program.R.IsReady())
                            {
                                if (Program.BurstTarget != null && Program.BurstTarget.IsValidTarget(Program.R.Range))
                                {
                                    var rPred = Program.R.GetPrediction(Program.BurstTarget);

                                    if (rPred.Hitchance >= HitChance.High)
                                    {
                                        Program.R.Cast(rPred.CastPosition);
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case "RivenMartyr":
                    if ((Program.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                        Program.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst) &&
                        Program.Me.CountEnemiesInRange(400) > 0)
                    {
                        Program.CastItem(true);
                    }
                    break;
                case "RivenFeint":
                    if (Program.R.Instance.Name != "RivenIzunaBlade")
                    {
                        return;
                    }

                    if (!Program.R.IsReady())
                    {
                        return;
                    }

                    switch (Program.Orbwalker.ActiveMode)
                    {
                        case Orbwalking.OrbwalkingMode.Combo:
                            var target = TargetSelector.GetSelectedTarget() ??
                                         TargetSelector.GetTarget(Program.R.Range, TargetSelector.DamageType.Physical);

                            if (target.IsValidTarget(Program.R.Range))
                            {
                                LoopEvent.R2Logic(target);
                            }
                            break;
                        case Orbwalking.OrbwalkingMode.Burst:
                            if (Program.BurstTarget != null && Program.BurstTarget.IsValidTarget(Program.R.Range))
                            {
                                var rPred = Program.R.GetPrediction(Program.BurstTarget);

                                if (rPred.Hitchance >= HitChance.High)
                                {
                                    Program.R.Cast(rPred.CastPosition);
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        private static void OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs Args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (Program.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None ||
                Program.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.WallJump ||
                Program.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee)
            {
                return;
            }

            switch (Args.Animation)
            {
                case "Spell1a":
                    Program.QStack = 1;
                    if (Program.Menu.Item("Dance", true).GetValue<bool>())
                    {
                        Game.SendEmote(Emote.Dance);
                    }
                    Utility.DelayAction.Add(281, () =>
                    {
                        Game.SendEmote(Emote.Dance);
                        Program.Me.IssueOrder(GameObjectOrder.MoveTo, Program.Me.Position.Extend(Game.CursorPos, -10));
                        Orbwalking.ResetAutoAttackTimer();
                    });
                    break;
                case "Spell1b":
                    if (Program.Menu.Item("Dance", true).GetValue<bool>())
                    {
                        Game.SendEmote(Emote.Dance);
                    }
                    Utility.DelayAction.Add(281, () =>
                    {
                        Game.SendEmote(Emote.Dance);
                        Program.Me.IssueOrder(GameObjectOrder.MoveTo, Program.Me.Position.Extend(Game.CursorPos, -10));
                        Orbwalking.ResetAutoAttackTimer();
                    });
                    break;
                case "Spell1c":
                    if (Program.Menu.Item("Dance", true).GetValue<bool>())
                    {
                        Game.SendEmote(Emote.Dance);
                    }
                    Utility.DelayAction.Add(381, () =>
                    {
                        Game.SendEmote(Emote.Dance);
                        Program.Me.IssueOrder(GameObjectOrder.MoveTo, Program.Me.Position.Extend(Game.CursorPos, -10));
                        Orbwalking.ResetAutoAttackTimer();
                    });
                    break;
            }
        }
    }
}