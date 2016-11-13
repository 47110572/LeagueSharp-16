namespace GosuMechanicsYasuo.Manager.Events
{
    using System;
    using SharpDX;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Color = System.Drawing.Color;

    internal class DrawManager : Logic
    {
        private static Vector2 spot1 = new Vector2(7274, 5908);
        private static Vector2 spot2 = new Vector2(8222, 3158);
        private static Vector2 spot3 = new Vector2(3674, 7058);
        private static Vector2 spot4 = new Vector2(3788, 7422);
        private static Vector2 spot5 = new Vector2(8372, 9606);
        private static Vector2 spot6 = new Vector2(6650, 11766);
        private static Vector2 spot7 = new Vector2(1678, 8428);
        private static Vector2 spot8 = new Vector2(10832, 7446);
        private static Vector2 spot9 = new Vector2(11160, 7504);
        private static Vector2 spot10 = new Vector2(6424, 5208);
        private static Vector2 spot11 = new Vector2(13172, 6508);
        private static Vector2 spot12 = new Vector2(11222, 7856);
        private static Vector2 spot13 = new Vector2(10372, 8456);
        private static Vector2 spot14 = new Vector2(4324, 6258);
        private static Vector2 spot15 = new Vector2(6488, 11192);
        private static Vector2 spot16 = new Vector2(7672, 8906);
        private static Vector2 spotA = new Vector2(10922, 6908);
        private static Vector2 spotB = new Vector2(7616, 4074);
        private static Vector2 spotC = new Vector2(2232, 8412);
        private static Vector2 spotD = new Vector2(7046, 5426);
        private static Vector2 spotE = new Vector2(8322, 2658);
        private static Vector2 spotF = new Vector2(3676, 7968);
        private static Vector2 spotG = new Vector2(3892, 6466);
        private static Vector2 spotH = new Vector2(12582, 6402);
        private static Vector2 spotI = new Vector2(11072, 8306);
        private static Vector2 spotJ = new Vector2(10882, 8416);
        private static Vector2 spotK = new Vector2(3730, 8080);
        private static Vector2 spotL = new Vector2(6574, 12256);
        private static Vector2 spotM = new Vector2(7244, 10890);
        private static Vector2 spotN = new Vector2(7784, 9494);
        private static Vector2 spotO = new Vector2(6984, 10980);

        internal static void Init(EventArgs args)
        {
            if (!Me.IsDead && !MenuGUI.IsShopOpen && !MenuGUI.IsChatOpen && !MenuGUI.IsScoreboardOpen)
            {
                if (Menu.Item("DrawQ", true).GetValue<bool>() && Q.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, Q.Range, Color.LightGreen);
                }

                if (Menu.Item("DrawQ3", true).GetValue<bool>() && Q3.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, Q3.Range, Color.LightGreen);
                }

                if (Menu.Item("DrawW", true).GetValue<bool>() && W.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, W.Range, Color.LightGreen);
                }

                if (Menu.Item("DrawE", true).GetValue<bool>() && E.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, E.Range, Color.LightGreen);
                }

                if (Menu.Item("DrawR", true).GetValue<bool>() && R.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, R.Range, Color.LightGreen);
                }

                if (Menu.Item("DrawSpots", true).GetValue<bool>())
                {
                    Render.Circle.DrawCircle(spot1.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot2.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot3.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot4.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot5.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot6.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot7.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot8.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot9.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot10.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot11.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot12.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot13.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot14.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot15.To3D(), 150, Color.Red, 2);
                    Render.Circle.DrawCircle(spot16.To3D(), 150, Color.Red, 2);

                    Render.Circle.DrawCircle(spotA.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotB.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotC.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotD.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotE.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotF.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotG.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotH.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotI.To3D(), 120, Color.Green, 1);
                    Render.Circle.DrawCircle(spotJ.To3D(), 120, Color.Green, 1);
                    Render.Circle.DrawCircle(spotL.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotM.To3D(), 200, Color.Green, 1);
                    Render.Circle.DrawCircle(spotN.To3D(), 400, Color.Green, 1);
                    Render.Circle.DrawCircle(spotO.To3D(), 200, Color.Green, 1);
                }
            }
        }
    }
}
