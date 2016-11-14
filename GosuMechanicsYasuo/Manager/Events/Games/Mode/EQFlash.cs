namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SharpDX;
    using Spells;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = Orbwalking;
    using static Common.Common;

    internal class EQFlash : Logic
    {
        public static void Init()
        {
            Orbwalking.MoveTo(Game.CursorPos);

            if (!SpellManager.HaveQ3)
            {
                if (Q.IsReady())
                {
                    var minion =
                        MinionManager.GetMinions(Me.Position, Q.Range, MinionTypes.All, MinionTeam.NotAlly)
                            .FirstOrDefault();

                    if (minion != null && minion.DistanceToPlayer() <= Q.Range)
                    {
                        Q.Cast(minion);
                    }
                }
            }
            else
            {
                if (Me.IsDashing())
                {
                    var bestPos =
                        FlashPoints().Where(x => HeroManager.Enemies.Count(a => a.IsValidTarget(600f)) > 0)
                            .OrderByDescending(x => HeroManager.Enemies.Count(i => i.Distance(x, true) <= 220*220))
                            .FirstOrDefault();

                    if (bestPos.IsValid() && Q3.Cast(bestPos, true))
                    {
                        Game.PrintChat("Qtarget!");
                        Utility.DelayAction.Add(10+(Game.Ping/2-5),
                                           () => Me.Spellbook.CastSpell(Me.GetSpellSlot("SummonerFlash"), bestPos));
                    }
                }

                if (E.IsReady() && Q3.IsReady())
                {
                    var allTargets =
                        ObjectManager.Get<Obj_AI_Base>()
                            .Where(x => !x.IsAlly && !x.IsMe && x.DistanceToPlayer() <= E.Range).ToList();

                    if (allTargets.Any())
                    {
                        var eTarget =
                            allTargets.Where(x => x.IsValidTarget(E.Range) && SpellManager.CanCastE(x))
                                .OrderByDescending(
                                    x =>
                                        HeroManager.Enemies.Count(
                                            t => t.IsValidTarget(595f, true, PosAfterE(x).To3D())))
                                .FirstOrDefault();

                        if (eTarget != null)
                        {
                        
                            E.CastOnUnit(eTarget, true);
                        }
                    }
                }
            }
        }


        private static List<Vector3> FlashPoints()
        {
            var points = new List<Vector3>();

            for (var i = 1; i <= 360; i++)
            {
                var angle = i * 2 * Math.PI / 360;
                var point = new Vector2(Me.Position.X + 425f*(float) Math.Cos(angle),
                    Me.Position.Y + 425f*(float) Math.Sin(angle)).To3D();

                points.Add(point);
            }

            return points;
        }
    }
}
