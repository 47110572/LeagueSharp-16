namespace Flowers_Yasuo.Common
{
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;

    public static class Common
    {
        public static bool CanCastDelayR(Obj_AI_Hero target)
        {
            //copy from valvesharp
            var buff = target.Buffs.FirstOrDefault(i => i.Type == BuffType.Knockback || i.Type == BuffType.Knockup);

            return buff != null &&
                   buff.EndTime - Game.Time <=
                   (buff.EndTime - buff.StartTime)/(buff.EndTime - buff.StartTime <= 0.5 ? 1.5 : 3);
        }

        public static bool UnderTower(Vector2 pos)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, true, pos.To3D()));
        }

        public static Vector2 PosAfterE(Obj_AI_Base target)
        {
            return
                ObjectManager.Player.ServerPosition.Extend(target.ServerPosition,
                        ObjectManager.Player.Distance(target) < 410f ? 475f : ObjectManager.Player.Distance(target) + 65f)
                    .To2D();
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
