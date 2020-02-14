using System;

namespace KoC.GameEngine
{
    struct QuickMaths
    {
        public static float DegreeToRadian(double angle)
        {
            return (float) (Math.PI * angle / 180.0);
        }
    }
}
