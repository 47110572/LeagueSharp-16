﻿namespace GosuMechanicsYasuo.Manager.Events
{
    using System;
    using Games.Mode;
    using LeagueSharp.Common;
    using Orbwalking = Orbwalking;

    internal class LoopManager : Logic
    {
        internal static void Init(EventArgs Args)
        {
            if (Me.IsDead || Me.IsRecalling())
            {
                return;
            }

            EvadeDetectedSkillshots.RemoveAll(skillshot => !skillshot.IsActive());

            DodgeSpell.Init();
            KillSteal.Init();
            Auto.Init();

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo.Init();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass.Init();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear.Init();
                    JungleClear.Init();
                    break;
                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit.Init();
                    break;
                case Orbwalking.OrbwalkingMode.Flee:
                    Flee.Init();
                    break;
                case Orbwalking.OrbwalkingMode.WallJump:
                    WallJump.Init();
                    break;
                case Orbwalking.OrbwalkingMode.None:
                    if (Menu.Item("EQFlash", true).GetValue<KeyBind>().Active)
                    {
                        EQFlash.Init();
                    }
                    break;
            }
        }
    }
}