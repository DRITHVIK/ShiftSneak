using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitomUtilities
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class PopUpSelectionAttribute : PropertyAttribute
    {
        public readonly string titleName;
        public readonly string selection;
        public readonly string callBack;

        public PopUpSelectionAttribute(string _titleName, string _selection, string _callBack)
        {
            titleName = _titleName;
            selection = _selection;
            callBack = _callBack;
        }
    }
}
