using UnityEngine;

namespace DigitomUtilities
{
    public static class Vector3Utilities
    {
        public static bool IsNaN(this Vector3 value)
        {
            return float.IsNaN(value.x) && float.IsNaN(value.y) && float.IsNaN(value.x);
        }
    }
}


