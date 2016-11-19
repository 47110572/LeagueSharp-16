namespace Flowers_Rengar.Manager.Spells
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class SpellManager : Logic
    {
        internal static void Init()
        {
            Q = new Spell(SpellSlot.Q, Orbwalking.GetRealAutoAttackRange(ObjectManager.Player) + 325f);
            W = new Spell(SpellSlot.W, 500f);
            E = new Spell(SpellSlot.E, 1000f);

            Q.SetSkillshot(0.25f, 70f, 1500f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.25f, 70f, 1500f, true, SkillshotType.SkillshotLine);

            Ignite = Me.GetSpellSlot("SummonerDot");
        }

        internal static bool MaxBrutal => ObjectManager.Player.Mana == 4;

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
