namespace GosuMechanicsYasuo.Common
{
    using System.Collections.Generic;
    using Evade;
    using LeagueSharp;

    public struct IsSafeResult
    {
        public bool IsSafe;
        public List<Skillshot> SkillshotList;
        public List<Obj_AI_Base> casters;
    }
}
