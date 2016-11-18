namespace Flowers_Yasuo.Manager.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SharpDX;
    using Spells;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Color = System.Drawing.Color;

    internal class DrawManager : Logic
    {
        public static List<Vector2> WallJumpPos = new List<Vector2>();

        internal static void InitPos()
        {
            WallJumpPos.Add(new Vector2(7274, 5908));
            WallJumpPos.Add(new Vector2(8222, 3158));
            WallJumpPos.Add(new Vector2(7784, 9494));
            WallJumpPos.Add(new Vector2(6574, 12256));
            WallJumpPos.Add(new Vector2(3730, 8080));
            WallJumpPos.Add(new Vector2(10882, 8416));
            WallJumpPos.Add(new Vector2(11072, 8306));
            WallJumpPos.Add(new Vector2(12582, 6402));
            WallJumpPos.Add(new Vector2(3892, 6466));
            WallJumpPos.Add(new Vector2(8322, 2658));
            WallJumpPos.Add(new Vector2(7046, 5426));
            WallJumpPos.Add(new Vector2(2232, 8412));
            WallJumpPos.Add(new Vector2(7672, 8906));
            WallJumpPos.Add(new Vector2(4324, 6258));
            WallJumpPos.Add(new Vector2(3674, 7058));
            WallJumpPos.Add(new Vector2(8372, 9606));
            WallJumpPos.Add(new Vector2(6650, 11766));
            WallJumpPos.Add(new Vector2(1678, 8428));
            WallJumpPos.Add(new Vector2(6424, 5208));
            WallJumpPos.Add(new Vector2(13172, 6508));
            WallJumpPos.Add(new Vector2(11222, 7856));
            WallJumpPos.Add(new Vector2(10372, 8456));
        }

        internal static void Init(EventArgs args)
        {
            if (!Me.IsDead && !MenuGUI.IsShopOpen && !MenuGUI.IsChatOpen && !MenuGUI.IsScoreboardOpen)
            {
                if (Menu.Item("DrawQ", true).GetValue<bool>() && Q.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, Q.Range, Color.FromArgb(17, 245, 224), 3);
                }

                if (Menu.Item("DrawQ3", true).GetValue<bool>() && SpellManager.HaveQ3)
                {
                    Render.Circle.DrawCircle(Me.Position, Q3.Range, Color.FromArgb(0, 149, 255), 3);
                }

                if (Menu.Item("DrawW", true).GetValue<bool>() && W.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, W.Range, Color.FromArgb(249, 21, 237), 3);
                }

                if (Menu.Item("DrawE", true).GetValue<bool>() && E.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, E.Range, Color.FromArgb(51, 254, 216), 3);
                }

                if (Menu.Item("DrawR", true).GetValue<bool>() && R.IsReady())
                {
                    Render.Circle.DrawCircle(Me.Position, R.Range, Color.FromArgb(247, 10, 10), 3);
                }

                if (Menu.Item("DrawSpots", true).GetValue<bool>())
                {
                    foreach (var pos in WallJumpPos.Where(x => x.Distance(Me) <= 1200))
                    {
                        Render.Circle.DrawCircle(pos.To3D(), 150, Color.FromArgb(251, 209, 0), 1);
                    }
                }

                if (Menu.Item("DrawStackQ", true).GetValue<bool>() && Q.Level > 0)
                {
                    var stackQ = Menu.Item("StackQ", true).GetValue<KeyBind>();
                    var MePos = Drawing.WorldToScreen(Me.Position);

                    Drawing.DrawText(MePos[0] - 50, MePos[1] + 25, Color.Red,
                        "Stack Q(" + new string(System.Text.Encoding.Default.GetChars(BitConverter.GetBytes(stackQ.Key))));
                    Drawing.DrawText(MePos[0] + 29, MePos[1] + 25, Color.Red, "): " + (stackQ.Active ? "On" : "Off"));
                }

                if (Menu.Item("DrawAutoQ", true).GetValue<bool>() && Q.Level > 0)
                {
                    var autoQ = Menu.Item("AutoQ", true).GetValue<KeyBind>();
                    var MePos = Drawing.WorldToScreen(Me.Position);

                    Drawing.DrawText(MePos[0] - 35, MePos[1] + 45, Color.Red,
                        "Auto Q(" + new string(System.Text.Encoding.Default.GetChars(BitConverter.GetBytes(autoQ.Key))));
                    Drawing.DrawText(MePos[0] + 29, MePos[1] + 45, Color.Red, "): " + (autoQ.Active ? "On" : "Off"));
                }

                if (Menu.Item("DrawRStatus", true).GetValue<bool>() && R.Level > 0)
                {
                    var comboR = Menu.Item("ComboR", true).GetValue<KeyBind>();
                    var MePos = Drawing.WorldToScreen(Me.Position);

                    Drawing.DrawText(MePos[0] - 50, MePos[1] + 65, Color.Red,
                        "Combo R(" + new string(System.Text.Encoding.Default.GetChars(BitConverter.GetBytes(comboR.Key))));
                    Drawing.DrawText(MePos[0] + 29, MePos[1] + 65, Color.Red, "): " + (comboR.Active ? "On" : "Off"));
                }
            }
        }
    }
}
