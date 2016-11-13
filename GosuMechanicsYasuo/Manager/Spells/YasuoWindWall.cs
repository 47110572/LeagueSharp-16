namespace GosuMechanicsYasuo.Manager.Spells
{
    using LeagueSharp;

    internal class YasuoWindWall
    {
        public MissileClient pointL;
        public MissileClient pointR;
        public float endtime;

        public void setR(MissileClient R)
        {
            pointR = R;
            endtime = Game.Time + 4;
        }

        public void setL(MissileClient L)
        {
            pointL = L;
            endtime = Game.Time + 4;
        }
    }
}
