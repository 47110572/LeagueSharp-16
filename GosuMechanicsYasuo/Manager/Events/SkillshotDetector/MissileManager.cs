namespace GosuMechanicsYasuo.Manager.Events
{
    using System;
    using Evade;
    using LeagueSharp;
    using LeagueSharp.Common;
    using System.Linq;

    internal class MissileManager : Logic
    {
        internal static void Init(Skillshot skillshot, MissileClient missile)
        {
            if (skillshot.SpellData.SpellName == "VelkozQ")
            {
                var spellData = SpellDatabase.GetByName("VelkozQSplit");
                var direction = skillshot.Direction.Perpendicular();

                if (EvadeDetectedSkillshots.All(s => s.SpellData.SpellName != "VelkozQSplit"))
                {
                    for (var i = -1; i <= 1; i = i + 2)
                    {
                        var skillshotToAdd = new Skillshot(
                            DetectionType.ProcessSpell, spellData, Environment.TickCount, missile.Position.To2D(),
                            missile.Position.To2D() + i*direction*spellData.Range, skillshot.Unit);

                        EvadeDetectedSkillshots.Add(skillshotToAdd);
                    }
                }
            }
        }
    }
}