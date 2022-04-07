using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DigitomUtilities
{
    public static class ObjectUtilities
    {
        public static bool IsPrefab(this GameObject go)
        {
            return PrefabUtility.IsPartOfAnyPrefab(go);
        }
    }
}


