using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitomUtilities
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class PopUpAttribute : PropertyAttribute
    {
        public readonly string selection;
        public readonly string callBack;

        public PopUpAttribute(string _selection)
        {
            selection = _selection;
        }
    }
}
