using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitomUtilities
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class SubTypeAttribute : PropertyAttribute
    {
        public readonly System.Type baseType;
        public readonly bool displayLabel;

        public SubTypeAttribute(System.Type _base, bool _displayLabel = true)
        {
            baseType = _base;
            displayLabel = _displayLabel;
        }
    }
}
