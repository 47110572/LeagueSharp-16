namespace Flowers_Yasuo
{
    using Evade;
    using System.Linq;
    using Manager.Menu;
    using Manager.Events;
    using Manager.Spells;
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;

    internal class Logic
    {
        internal static Spell Q;
        internal static Spell Q3;
        internal static Spell W;
        internal static Spell E;
        internal static Spell R;
        internal static SpellSlot Ignite = SpellSlot.Unknown;
        internal static SpellSlot Flash = SpellSlot.Unknown;
        internal static Menu Menu;
        internal static bool isDashing;
        internal static int SkinID;
        internal static int lastECast;
        internal static int lastHarassTime;
        internal static Vector3 lastEPos;       
        internal static Obj_AI_Hero Me;
        internal static Orbwalking.Orbwalker Orbwalker;

        internal static void LoadYasuo()
        {
            Me = ObjectManager.Player;
            SkinID = ObjectManager.Player.BaseSkinId;

            SpellManager.Init();
            MenuManager.Init();
            EvadeManager.Init();
            EvadeTargetManager.Init();
            EventManager.Init();
            Manager.Events.Games.Mode.WallJump.InitPos();
        }

        internal static bool IsDashing => isDashing || Me.IsDashing();

        internal static void EnbaleSkin(object obj, OnValueChangeEventArgs Args)
        {
            if (!Args.GetNewValue<bool>())
            {
                ObjectManager.Player.SetSkin(ObjectManager.Player.ChampionName, SkinID);
            }
        }

        public static bool CanCastDelayR(Obj_AI_Hero target)
        {
            //copy from valvesharp
            var buff = target.Buffs.FirstOrDefault(i => i.Type == BuffType.Knockback || i.Type == BuffType.Knockup);

            return buff != null &&
                   buff.EndTime - Game.Time <=
                   (buff.EndTime - buff.StartTime) / (buff.EndTime - buff.StartTime <= 0.5 ? 1.5 : 3);
        }

        public static bool UnderTower(Vector3 pos)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.Health > 1 && turret.IsValidTarget(950, true, pos));
        }

        public static Vector3 PosAfterE(Obj_AI_Base target)
        {
            var pred = Prediction.GetPrediction(target, 375f);

            return ObjectManager.Player.ServerPosition.Extend(pred.UnitPosition, 475f);
        }

        internal static void UseItems(Obj_AI_Base target, bool IsCombo = false)
        {
            if (IsCombo)
            {
                if (Items.HasItem(3153, Me) && Items.CanUseItem(3153) && Me.HealthPercent <= 80)
                {
                    Items.UseItem(3153, target);
                }

                if (Items.HasItem(3143, Me) && Items.CanUseItem(3143) && Me.Distance(target.Position) <= 400)
                {
                    Items.UseItem(3143);
                }

                if (Items.HasItem(3144, Me) && Items.CanUseItem(3144) && target.IsValidTarget(Q.Range))
                {
                    Items.UseItem(3144, target);
                }

                if (Items.HasItem(3142, Me) && Items.CanUseItem(3142) && Me.Distance(target.Position) <= Q.Range)
                {
                    Items.UseItem(3142);
                }
            }

            if (Items.HasItem(3074, Me) && Items.CanUseItem(3074) && Me.Distance(target.Position) <= 400)
            {
                Items.UseItem(3074);
            }

            if (Items.HasItem(3077, Me) && Items.CanUseItem(3077) && Me.Distance(target.Position) <= 400)
            {
                Items.UseItem(3077);
            }
        }
    }
}
