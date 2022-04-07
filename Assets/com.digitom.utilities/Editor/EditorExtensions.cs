using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DigitomUtilities
{
    public static class EditorExtensions
    {
        public static float GetWidth(this string _string)
        {
            var content = new GUIContent { text = _string };
            return new GUIStyle().CalcSize(content).x;
        }
    }
}



