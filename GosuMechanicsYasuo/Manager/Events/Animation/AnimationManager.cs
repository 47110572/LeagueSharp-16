namespace GosuMechanicsYasuo.Manager.Events
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class AnimationManager : Logic
    {
        internal static void Init(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs Args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (Args.Animation != "Spell3")
            {
                isDashing = true;

                Utility.DelayAction.Add(300, () =>
                {
                    if (Me.IsDashing())
                    {
                        isDashing = false;
                    }
                });

                Utility.DelayAction.Add(450, () => isDashing = false);
            }
        }
    }
}