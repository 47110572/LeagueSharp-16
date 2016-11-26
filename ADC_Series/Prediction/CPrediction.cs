using LeagueSharp;
using SharpDX;

namespace Flowers_ADC_Series.Prediction
{
    internal static class CPrediction
    {
        internal struct Position
        {
            public readonly Obj_AI_Hero Hero;
            public readonly Vector3 UnitPosition;

            public Position(Obj_AI_Hero hero, Vector3 unitPosition)
            {
                Hero = hero;
                UnitPosition = unitPosition;
            }
        }
    }
}
