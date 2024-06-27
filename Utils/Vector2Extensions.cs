using System;
using System.Numerics;

namespace VisionPoints.Utils
{
    public static class Vector2Extensions
    {
        public static double Angle(this Vector2 vector)
            => Math.Atan2(vector.X, vector.Y);
    }
}