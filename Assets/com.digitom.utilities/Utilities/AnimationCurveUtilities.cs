using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public static class AnimationCurveUtilities
    {
        public static Keyframe Copy(this Keyframe key)
        {
            return new Keyframe
            {
                inTangent = key.inTangent,
                inWeight = key.inWeight,
                outTangent = key.outTangent,
                outWeight = key.outWeight,
                time = key.time,
                value = key.value,
                weightedMode = key.weightedMode
            };
        }
    }
}


