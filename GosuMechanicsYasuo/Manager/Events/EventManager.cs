namespace GosuMechanicsYasuo.Manager.Events
{
    using Evade;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class EventManager
    {
        internal static void Init()
        {
            Game.OnUpdate += LoopManager.Init;
            Obj_AI_Base.OnPlayAnimation += AnimationManager.Init;
            Interrupter2.OnInterruptableTarget += InterruptManager.Init;
            AntiGapcloser.OnEnemyGapcloser += GapcloserManager.Init;
            SkillshotDetector.OnDeleteMissile += MissileManager.Init;
            SkillshotDetector.OnDetectSkillshot += SkillshotManager.Init;
            GameObject.OnCreate += CreateManager.Init;
            GameObject.OnDelete += DeleteManager.Init;
            Drawing.OnDraw += DrawManager.Init;
        }
    }
}
