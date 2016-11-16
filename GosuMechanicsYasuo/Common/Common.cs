namespace GosuMechanicsYasuo.Common
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Evade;
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;

    public static class Common
    {
        public static bool interCir(Vector2 sE, Vector2 L, Vector2 C, float r)
        {
            var d = L - sE;
            var f = sE - C;
            var a = Vector2.Dot(d, d);
            var b = 2 * Vector2.Dot(f, d);
            var c = Vector2.Dot(f, f) - r * r;
            var discriminant = b * b - 4 * a * c;

            if (discriminant >= 0)
            {
                discriminant = (float)Math.Sqrt(discriminant);

                var t1 = (-b - discriminant) / (2 * a);
                var t2 = (-b + discriminant) / (2 * a);

                if (t1 >= 0 && t1 <= 1)
                {
                    return true;
                }

                return t2 >= 0 && t2 <= 1;
            }

            return false;
        }

        public static Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
        {
            var A1 = pe1.Y - ps1.Y;
            var B1 = ps1.X - pe1.X;
            var C1 = A1 * ps1.X + B1 * ps1.Y;
            var A2 = pe2.Y - ps2.Y;
            var B2 = ps2.X - pe2.X;
            var C2 = A2 * ps2.X + B2 * ps2.Y;
            var delta = A1 * B2 - A2 * B1;

            return delta == 0 ? new Vector2(-1, -1) : new Vector2((B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta);
        }

        public static bool goesThroughWall(Vector3 vec1, Vector3 vec2)
        {
            if (Logic.wall.endtime < Game.Time || Logic.wall.pointL == null)
            {
                return false;
            }

            var inter = LineIntersectionPoint(vec1.To2D(), vec2.To2D(), Logic.wall.pointL.Position.To2D(),
                Logic.wall.pointR.Position.To2D());
            var wallW = 300 + 50 * ObjectManager.Player.GetSpell(SpellSlot.W).Level;

            if (Logic.wall.pointL.Position.To2D().Distance(inter) > wallW ||
                Logic.wall.pointR.Position.To2D().Distance(inter) > wallW)
            {
                return false;
            }

            var dist = vec1.Distance(vec2);

            return !(vec1.To2D().Distance(inter) + vec2.To2D().Distance(inter) - 30 > dist);
        }

        public static bool TargetIsJump(Obj_AI_Base enemy, List<Obj_AI_Hero> ignore = null)
        {
            if (enemy.IsValid && enemy.IsEnemy && !enemy.IsInvulnerable && !enemy.MagicImmune && !enemy.IsDead &&
                !(enemy is FollowerObject))
            {
                if (ignore != null)
                {
                    foreach (var ign in ignore)
                    {
                        if (ign.NetworkId == enemy.NetworkId)
                        {
                            return false;
                        }
                    }
                }

                foreach (var buff in enemy.Buffs)
                {
                    if (buff.Name == "YasuoDashWrapper")
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public static Vector2 GetNextPos(Obj_AI_Hero target)
        {
            if (target == null)
            {
                return Vector2.Zero;
            }

            var dashPos = target.Position.To2D();

            if (target.IsMoving && target.Path.Length != 0)
            {
                var tpos = target.Position.To2D();
                var path = target.Path[0].To2D() - tpos;

                path.Normalize();
                dashPos = tpos + (path * 100);
            }

            return dashPos;
        }

        public static bool IsKnockedUp(Obj_AI_Hero target)
        {
            return target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Knockback);
        }

        public static bool CanCastDelayR(Obj_AI_Hero target)
        {
            //copy from valvesharp
            var buff = target.Buffs.FirstOrDefault(i => i.Type == BuffType.Knockback || i.Type == BuffType.Knockup);

            return buff != null &&
                   buff.EndTime - Game.Time <=
                   (buff.EndTime - buff.StartTime)/(buff.EndTime - buff.StartTime <= 0.5 ? 1.5 : 3);
        }

        public static bool AlliesNearTarget(Obj_AI_Base target, float range)
        {
            return HeroManager.Allies.Where(t => t.Distance(target) < range).Any(t => t != null);
        }

        public static bool isDangerous(Obj_AI_Base target, float range)
        {
            return HeroManager.Enemies.Where(t => t.Distance(PosAfterE(target)) < range).Any(t => t != null);
        }

        public static bool UnderTower(Vector2 pos)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Any(i => i.Health > 0 && i.Distance(pos) <= 950 && i.IsEnemy);
        }

        public static Vector2 PosAfterE(Obj_AI_Base target)
        {
            return
                ObjectManager.Player.ServerPosition.Extend(target.ServerPosition,
                        ObjectManager.Player.Distance(target) < 410f ? 475f : ObjectManager.Player.Distance(target) + 65f)
                    .To2D();
        }

        public static Vector3 AfterEPos(Obj_AI_Base target)
        {
            return
                ObjectManager.Player.ServerPosition.Extend(target.ServerPosition,
                        ObjectManager.Player.Distance(target) < 410f ? 475f : ObjectManager.Player.Distance(target) + 65f);
        }

        public static Vector2 V2E(Vector3 from, Vector3 direction, float distance)
        {
            return (@from + distance * Vector3.Normalize(direction - @from)).To2D();
        }

        public static float DistanceToPlayer(this Obj_AI_Base source)
        {
            return ObjectManager.Player.Distance(source);
        }

        public static float DistanceToPlayer(this Vector3 position)
        {
            return position.To2D().DistanceToPlayer();
        }

        public static float DistanceToPlayer(this Vector2 position)
        {
            return ObjectManager.Player.Distance(position);
        }
    }
}
