using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kroz.Util
{
    public static class Calculate
    {
        static Calculate() { } //default initializer

        public static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180.0);
        }

        public static double RadianToDegree(double radian)
        {

            return radian * (180.0 / Math.PI);
        }
    }
}
