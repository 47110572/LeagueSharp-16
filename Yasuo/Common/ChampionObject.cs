namespace Flowers_Yasuo.Common
{
    using LeagueSharp;

    internal class ChampionObject
    {
        public ChampionObject(Obj_AI_Hero hero)
        {
            Hero = hero;
        }

        public Obj_AI_Hero Hero { get; private set; }
        public float LastSeen { get; set; }
    }
}
