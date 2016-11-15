namespace GosuMechanicsYasuo.Manager.Events
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class EventManager : Logic
    {
        internal static void Init()
        {
            Game.OnUpdate += LoopManager.Init;
            Obj_AI_Base.OnProcessSpellCast += SpellCastManager.Init;
            Obj_AI_Base.OnPlayAnimation += AnimationManager.Init;
            Interrupter2.OnInterruptableTarget += InterruptManager.Init;
            AntiGapcloser.OnEnemyGapcloser += GapcloserManager.Init;
            GameObject.OnCreate += CreateManager.Init;
            GameObject.OnDelete += DeleteManager.Init;
            Drawing.OnDraw += DrawManager.Init;
        }
    }
}
