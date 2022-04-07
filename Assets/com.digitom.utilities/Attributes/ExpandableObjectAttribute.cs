using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [AttributeUsage(AttributeTargets.All)]
    public class ExpandableObjectAttribute : PropertyAttribute
    {
        public bool expanded;
    }
}

