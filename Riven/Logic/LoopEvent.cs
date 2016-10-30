namespace Flowers_Riven
{
    using Common;
    using LeagueSharp;
    using LeagueSharp.Common;
    using System;
    using System.Linq;

    internal class LoopEvent
    {
        public static void Init()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Program.Me.IsDead)
                return;

            Autobool();
            KeelQLogic();
            KillStealLogic();

            if (Program.Menu.Item("EnableSkin", true).GetValue<bool>())
            {
                ObjectManager.Player.SetSkin(ObjectManager.Player.ChampionName,
                    Program.Menu.Item("SelectSkin", true).GetValue<StringList>().SelectedIndex);
            }

            switch (Program.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Burst:
                    Brust();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.QuickHarass:
                    QuickHarass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    break;
                case Orbwalking.OrbwalkingMode.Flee:
                    FleeLogic();
                    break;
                case Orbwalking.OrbwalkingMode.WallJump:
                    WallJump();
                    break;
            }
        }

        private static void Autobool()
        {
            if (Program.QTarget != null)
            {
                if (Program.CanQ)
                {
                    Program.Q.Cast(((Obj_AI_Base)Program.QTarget).Position);
                }
            }
        }

        private static void KeelQLogic()
        {
            if (Program.Menu.Item("KeepQALive", true).GetValue<bool>() && !Program.Me.UnderTurret(true) &&
                !Program.Me.IsRecalling() && Program.Me.HasBuff("RivenTriCleave"))
            {
                if (Program.Me.GetBuff("RivenTriCleave").EndTime - Game.Time < 0.3)
                {
                    Program.Q.Cast(Game.CursorPos);
                }
            }
        }

        private static void KillStealLogic()
        {
            foreach (var e in HeroManager.Enemies.Where(e => !e.IsZombie && !e.HasBuff("KindredrNoDeathBuff") &&
                                                             !e.HasBuff("Undying Rage") &&
                                                             !e.HasBuff("JudicatorIntervention") && e.IsValidTarget()))
            {
                if (Program.W.IsReady() && Program.Menu.Item("KillStealW", true).GetValue<bool>())
                {
                    if (e.IsValidTarget(Program.W.Range) &&
                        Program.Me.GetSpellDamage(e, SpellSlot.W) > e.Health + e.HPRegenRate)
                    {
                        Program.W.Cast();
                    }
                }

                if (Program.R.IsReady() && Program.Menu.Item("KillStealR", true).GetValue<bool>())
                {
                    if (Program.Me.HasBuff("RivenWindScarReady"))
                    {
                        if (Program.E.IsReady() && Program.Menu.Item("KillStealE", true).GetValue<bool>())
                        {
                            if (Program.Me.ServerPosition.CountEnemiesInRange(Program.R.Range + Program.E.Range) < 3 &&
                                Program.Me.HealthPercent > 50)
                            {
                                if (Program.Me.GetSpellDamage(e, SpellSlot.R) > e.Health + e.HPRegenRate &&
                                    e.IsValidTarget(Program.R.Range + Program.E.Range - 100))
                                {
                                    if (Program.E.IsReady())
                                    {
                                        Program.E.Cast(e.Position);
                                    }
                                    else if (!Program.E.IsReady())
                                    {
                                        Program.R.CastIfHitchanceEquals(e, HitChance.High, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Program.Me.GetSpellDamage(e, SpellSlot.R) > e.Health + e.HPRegenRate &&
                                e.IsValidTarget(Program.R.Range - 50))
                            {
                                Program.R.CastIfHitchanceEquals(e, HitChance.High, true);
                            }
                        }
                    }
                }
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(900, TargetSelector.DamageType.Physical);

            if (target.IsValidTarget())
            {
                if (Program.Menu.Item("ComboW", true).GetValue<bool>() && Program.W.IsReady() &&
                    target.IsValidTarget(Program.W.Range))
                {
                    Program.W.Cast();
                }

                if (Program.Menu.Item("ComboE", true).GetValue<bool>() && Program.E.IsReady())
                {
                    if (target.DistanceToPlayer() <= Program.W.Range + Program.E.Range &&
                        target.DistanceToPlayer() > Orbwalking.GetRealAutoAttackRange(Program.Me) + 100)
                    {
                        Program.E.Cast(target.IsMelee ? Game.CursorPos : target.ServerPosition);
                    }
                }

                if (Program.Menu.Item("ComboR", true).GetValue<bool>())
                {
                    if (Program.R.IsReady())
                    {
                        switch (Program.R.Instance.Name)
                        {
                            case "RivenFengShuiEngine":
                                if (Program.Menu.Item("R1Combo", true).GetValue<KeyBind>().Active)
                                {
                                    if (target.Distance(Program.Me.ServerPosition) <
                                        Program.E.Range + Program.Me.AttackRange &&
                                        Program.Me.CountEnemiesInRange(500) >= 1 &&
                                        !target.IsDead)
                                    {
                                        Program.R.Cast();
                                    }
                                }
                                break;
                            case "RivenIzunaBlade":
                                R2Logic(target);
                                break;
                        }
                    }
                }
            }
        }

        public static void R2Logic(Obj_AI_Hero target)
        {
            if (Program.R.Instance.Name != "RivenIzunaBlade")
            {
                return;
            }

            if (target.IsValidTarget(850) && !target.IsDead)
            {
                switch (Program.Menu.Item("R2Mode", true).GetValue<StringList>().SelectedIndex)
                {
                    case 0:
                        if (Program.R.GetDamage(target) > target.Health && target.IsValidTarget(Program.R.Range) &&
                            target.Distance(Program.Me.ServerPosition) < 600)
                        {
                            Program.R.Cast(target);
                        }
                        break;
                    case 1:
                        if (target.HealthPercent < 25 &&
                            target.Health > Program.R.GetDamage(target) + Program.Me.GetAutoAttackDamage(target) * 2)
                        {
                            Program.R.Cast(target);
                        }
                        break;
                    case 2:
                        if (target.IsValidTarget(Program.R.Range) &&
                            target.Distance(Program.Me.ServerPosition) < 600)
                        {
                            Program.R.Cast(target);
                        }
                        break;
                }
            }
        }

        private static void Brust()
        {
            var target = TargetSelector.GetSelectedTarget();

            if (target != null && !target.IsDead && target.IsValidTarget() && !target.IsZombie)
            {
                Program.BurstTarget = target;

                if (Program.R.IsReady())
                {
                    if (Program.Me.HasBuff("RivenFengShuiEngine") & Program.Q.IsReady() && Program.E.IsReady() &&
                        Program.W.IsReady() &&
                        target.Distance(Program.Me.ServerPosition) < Program.E.Range + Program.Me.AttackRange + 100)
                    {
                        Program.E.Cast(target.Position);
                    }

                    if (Program.E.IsReady() &&
                        target.Distance(Program.Me.ServerPosition) < Program.Me.AttackRange + Program.E.Range + 100)
                    {
                        Program.R.Cast();
                        Program.E.Cast(target.Position);
                    }
                }

                if (Program.W.IsReady() && HeroManager.Enemies.Any(x => x.IsValidTarget(Program.W.Range)))
                {
                    Program.W.Cast();
                }

                if (Program.QStack == 1 || Program.QStack == 2 || target.HealthPercent < 50)
                {
                    if (Program.Me.HasBuff("RivenWindScarReady"))
                    {
                        Program.R.Cast(target);
                    }
                }

                if (Program.Menu.Item("BurstIgnite", true).GetValue<bool>() && Program.Ignite != SpellSlot.Unknown)
                {
                    if (target.HealthPercent < 50)
                    {
                        if (Program.Ignite.IsReady())
                        {
                            Program.Me.Spellbook.CastSpell(Program.Ignite, target);
                        }
                    }
                }

                if (Program.Menu.Item("BurstFlash", true).GetValue<bool>() && Program.Flash != SpellSlot.Unknown)
                {
                    if (Program.Flash.IsReady() && Program.R.IsReady() &&
                        Program.R.Instance.Name == "RivenFengShuiEngine" && Program.E.IsReady() &&
                        Program.W.IsReady() && target.Distance(Program.Me.ServerPosition) <= 780 &&
                        target.Distance(Program.Me.ServerPosition) >= Program.E.Range + Program.Me.AttackRange + 85)
                    {
                        Program.R.Cast();
                        Program.E.Cast(target.Position);
                        Utility.DelayAction.Add(150,
                            () => { Program.Me.Spellbook.CastSpell(Program.Flash, target.Position); });
                    }
                }
            }
            else
            {
                Program.BurstTarget = null;
            }
        }

        private static void Harass()
        {
            if (Program.Menu.Item("HarassW", true).GetValue<bool>() && Program.W.IsReady())
            {
                if (HeroManager.Enemies.Find(
                        x => x.IsValidTarget(Program.W.Range) && !x.HasBuffOfType(BuffType.SpellShield)) != null)
                {
                    Program.W.Cast();
                }
            }
        }

        private static void QuickHarass()
        {
            var t = TargetSelector.GetSelectedTarget();

            if (t != null && t.IsValidTarget())
            {
                if (Program.QStack == 2)
                {
                    if (Program.E.IsReady())
                    {
                        Program.E.Cast(Program.Me.ServerPosition +
                                       (Program.Me.ServerPosition - t.ServerPosition).Normalized()*Program.E.Range);
                    }

                    if (!Program.E.IsReady())
                    {
                        Program.Q.Cast(Program.Me.ServerPosition +
                                       (Program.Me.ServerPosition - t.ServerPosition).Normalized()*Program.E.Range);
                    }
                }

                if (Program.W.IsReady())
                {
                    if (t.IsValidTarget(Program.W.Range) && Program.QStack == 1)
                    {
                        Program.W.Cast();
                    }
                }

                if (Program.Q.IsReady())
                {
                    if (Program.QStack == 0)
                    {
                        if (t.IsValidTarget(Program.Me.AttackRange + Program.Me.BoundingRadius + 150))
                        {
                            Program.CastQ(t);
                        }
                    }
                }
            }
        }

        private static void LaneClear()
        {
            if (Program.Menu.Item("LaneClearW", true).GetValue<bool>())
            {
                var minions = MinionManager.GetMinions(Program.Me.ServerPosition, Program.W.Range);

                if (Program.W.IsReady() && minions.Count >= 3)
                {
                    Program.W.Cast();
                }
            }
        }

        private static void FleeLogic()
        {
            var target =
                HeroManager.Enemies.FirstOrDefault(
                    enemy => enemy.IsValidTarget(Program.W.Range) && !enemy.HasBuffOfType(BuffType.SpellShield));

            if (Program.W.IsReady() && target!= null && target.IsValidTarget(Program.W.Range))
            {
                Program.W.Cast();
            }

            if (Program.E.IsReady() && !Program.Me.IsDashing())
            {
                Program.E.Cast(Program.Me.Position.Extend(Game.CursorPos, 300));
            }
            else if (Program.Q.IsReady() && !Program.Me.IsDashing())
            {
                Program.Q.Cast(Game.CursorPos);
            }
        }

        private static void WallJump()
        {
            var DashPos = Program.Me.ServerPosition.Extend(Game.CursorPos, Program.Q.Range);
            var wallPoint = VectorHelper.GetFirstWallPoint(DashPos);

            if (Program.Q.IsReady() && Program.QStack != 2)
            {
                Program.Q.Cast(Game.CursorPos);
            }

            if (!VectorHelper.IsWallDash(DashPos))
            {
                return;
            }

            if (Program.QStack == 2)
            {
                if (wallPoint.DistanceToPlayer() <= 70)
                {
                    Program.Q.Cast(wallPoint);
                }
                else if (Program.E.IsReady() && wallPoint.DistanceToPlayer() <= Program.E.Range)
                {
                    if (Program.E.Cast(wallPoint))
                    {
                        Utility.DelayAction.Add(150 + Game.Ping, () => Program.Q.Cast(wallPoint));
                    }
                }
            }
        }
    }
}