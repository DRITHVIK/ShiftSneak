using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public static class BitUtilities
    {
        public static bool IsInMask(this int bit, int mask)
        {
            return mask == (mask | (1 << bit));
        }
    }
}


