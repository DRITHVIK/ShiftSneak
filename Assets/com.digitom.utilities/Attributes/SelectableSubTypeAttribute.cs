using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [System.Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectableSubTypeAttribute : PropertyAttribute
    {
        public Type type;
        public int selection;
    }
}


