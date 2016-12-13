namespace Flowers_Riven_Reborn.Manager.Events
{
    using Spells;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = myCommon.Orbwalking;

    internal class AnimationManager : Logic
    {
        internal static void Init(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs Args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (Orbwalking.isNone || Orbwalking.isFlee)
            {
                return;
            }

            if (Args.Animation.Contains("c29"))
            {
                lastQTime = Utils.TickCount;
                qStack = 1;
                SpellManager.ResetQA(Menu.Item("Q1Delay", true).GetValue<Slider>().Value);
            }
            else if (Args.Animation.Contains("c39"))
            {
                lastQTime = Utils.TickCount;
                qStack = 2;
                SpellManager.ResetQA(Menu.Item("Q2Delay", true).GetValue<Slider>().Value);
            }
            else if (Args.Animation.Contains("c49"))
            {
                lastQTime = Utils.TickCount;
                qStack = 0;
                SpellManager.ResetQA(Menu.Item("Q3Delay", true).GetValue<Slider>().Value);
            }
        }
    }
}