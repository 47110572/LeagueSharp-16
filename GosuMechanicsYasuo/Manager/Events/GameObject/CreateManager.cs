namespace GosuMechanicsYasuo.Manager.Events
{
    using System;
    using LeagueSharp;

    internal class CreateManager : Logic
    {
        internal static void Init(GameObject sender, EventArgs Args)
        {
            var missile = sender as MissileClient;

            if (missile != null)
            {
                var missle = missile;

                if (missle.SData.Name == "yasuowmovingwallmisl")
                {
                    wall.setL(missle);
                }

                if (missle.SData.Name == "yasuowmovingwallmisl")
                {
                    wallCasted = true;
                }

                if (missle.SData.Name == "yasuowmovingwallmisr")
                {
                    wall.setR(missle);
                }
            }
        }
    }
}
