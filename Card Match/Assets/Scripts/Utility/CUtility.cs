using System;
using UnityEngine;

namespace Game.Utility
{
    public static class CUtility
    {
        #region Util

        public static void ReleaseMemory()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public static string RoundToOneDecimalPlace(float aValue)
        {
            string aReturnVal = aValue.ToString("0.0");
            if (aReturnVal.Contains(".0"))
            {
                return ((int)aValue).ToString();
            }

            return aReturnVal;
        }

        public static string RoundToTwoDecimalPlaces(float aValue)
        {
            string aReturnVal = aValue.ToString("0.00");
            if (aReturnVal.Contains(".00"))
            {
                return aValue.ToString();
            }

            return aReturnVal;
        }

        //Rounding float to particular points
        public static float RoundingToFloat(float a, int digits = 2)
        {
            return (float)Math.Round(a, digits);
        }

        #endregion
    }
}