namespace GosuMechanicsYasuo.Manager.Events.Games.Mode
{
    using System.Linq;
    using Evade;
    using Spells;
    using SharpDX;
    using LeagueSharp;
    using LeagueSharp.Common;
    using static Common.Common;

    internal class DodgeSpell : Logic
    {
        private static readonly Menu SpellMenu = Menu.SubMenu("Misc").SubMenu("Dodge Spells");

        internal static void Init()
        {
            foreach (var spell in EvadeDetectedSkillshots)
            {
                if (Menu.Item("smartW", true).GetValue<bool>() && W.IsReady())
                {
                    UseWDodge(spell);
                }

                if (Menu.Item("smartEDogue", true).GetValue<bool>() && !W.IsReady() &&
                    !isSafePoint(ObjectManager.Player.Position.To2D(), true).IsSafe)
                {
                    UseEDodge(spell);
                }
            }
        }

        private static void UseWDodge(Skillshot skillShot)
        {
            if (!W.IsReady() || skillShot.SpellData.Type == SkillShotType.SkillshotCircle ||
                skillShot.SpellData.Type == SkillShotType.SkillshotRing)
            {
                return;
            }

            if (skillShot.IsAboutToHit(Menu.Item("smartWDelay", true).GetValue<Slider>().Value, Me))
            {
                var sd = SpellDatabase.GetByMissileName(skillShot.SpellData.MissileSpellName);

                if (sd == null)
                {
                    return;
                }

                if (!EvadeSpellEnabled(sd.MenuItemName))
                {
                    return;
                }

                if (Menu.Item("wwDanger", true).GetValue<bool>())
                {
                    if (skillShotIsDangerous(sd.MenuItemName))
                    {
                        Me.Spellbook.CastSpell(SpellSlot.W, skillShot.Start.To3D(), skillShot.Start.To3D());
                    }
                }
                else
                {
                    if (SpellMenu.Item("DangerLevel" + sd.MenuItemName, true) != null
                        && SpellMenu.Item("DangerLevel" + sd.MenuItemName, true).GetValue<Slider>().Value >=
                        Menu.Item("smartWDanger", true).GetValue<Slider>().Value)
                    {
                        Me.Spellbook.CastSpell(SpellSlot.W, skillShot.Start.To3D(), skillShot.Start.To3D());
                    }
                }
            }
        }

        private static void UseEDodge(Skillshot skillShot)
        {
            if (!E.IsReady())
            {
                return;
            }

            var closest = float.MaxValue;
            var currentDashSpeed = 700 + Me.MoveSpeed;

            Obj_AI_Base closestTarg = null;

            foreach (
                var enemy in
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(
                        ob =>
                            ob.NetworkId != skillShot.Unit.NetworkId && TargetIsJump(ob) &&
                            ob.Distance(Me) < E.Range)
                    .OrderBy(ene => ene.Distance(Game.CursorPos, true)))
            {
                var pPos = Me.Position.To2D();
                var posAfterE = V2E(Me.Position, enemy.Position, 475);
                var dashDir = (posAfterE - Me.Position.To2D()).Normalized();

                if (isSafePoint(posAfterE).IsSafe && wontHitOnDash(skillShot, enemy, skillShot, dashDir))
                {
                    var curDist = Vector2.DistanceSquared(Game.CursorPos.To2D(), posAfterE);
                    if (curDist < closest)
                    {
                        closestTarg = enemy;
                        closest = curDist;
                    }
                }
            }

            if (closestTarg != null && closestTarg.CountEnemiesInRange(600) <= 2 &&
                SpellMenu.Item("DangerLevel" + skillShot.SpellData.MenuItemName) != null &&
                SpellMenu.Item("DangerLevel" + skillShot.SpellData.MenuItemName).GetValue<Slider>().Value >=
                Menu.Item("smartEDogueDanger", true).GetValue<Slider>().Value)
            {
                SpellManager.useENormal(closestTarg);
            }
        }

        private static bool willColide(Skillshot ss, Vector2 from, float speed, Vector2 direction, float radius)
        {
            var ssVel = ss.Direction.Normalized() * ss.SpellData.MissileSpeed;
            var dashVel = direction * speed;
            var a = ssVel - dashVel;
            var realFrom = from.Extend(direction, ss.SpellData.Delay + speed);

            if (!ss.IsAboutToHit((int) (dashVel.Length()/475*1000) + Game.Ping + 100, ObjectManager.Player))
            {
                return false;
            }

            return ss.IsAboutToHit(1000, ObjectManager.Player) &&
                   interCir(ss.MissilePosition,
                       ss.MissilePosition.Extend(ss.MissilePosition + a, ss.SpellData.Range + 50), from,
                       radius);
        }

        private static bool wontHitOnDash(Skillshot ss, Obj_AI_Base jumpOn, Skillshot skillShot, Vector2 dashDir)
        {
            var currentDashSpeed = 700 + Me.MoveSpeed;
            var intersectionPoint = LineIntersectionPoint(Me.Position.To2D(), V2E(Me.Position, jumpOn.Position, 475),
                ss.Start, ss.End);
            var arrivingTime = Vector2.Distance(Me.Position.To2D(), intersectionPoint) / currentDashSpeed;
            var skillshotPosition = ss.GetMissilePosition((int)(arrivingTime * 1000));

            return !(Vector2.DistanceSquared(skillshotPosition, intersectionPoint) <
                     ss.SpellData.Radius + Me.BoundingRadius) ||
                   willColide(skillShot, Me.Position.To2D(), 700f + Me.MoveSpeed, dashDir,
                       Me.BoundingRadius + skillShot.SpellData.Radius);
        }

        private static bool skillShotIsDangerous(string Name)
        {
            return SpellMenu.Item("IsDangerous" + Name, true) == null ||
                   SpellMenu.Item("IsDangerous" + Name, true).GetValue<bool>();
        }

        private static bool EvadeSpellEnabled(string Name)
        {
            return SpellMenu.Item("Enabled" + Name, true) != null &&
                   SpellMenu.Item("Enabled" + Name, true).GetValue<bool>();
        }
    }
}
