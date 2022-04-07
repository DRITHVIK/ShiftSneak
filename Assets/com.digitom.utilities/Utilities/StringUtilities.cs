using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

namespace DigitomUtilities
{
    public static class StringUtilities
    {
        public static string DisplayName(this string _string)
        {
            return Regex.Replace(_string, "([a-z])([A-Z])", "$1 $2");
        }
    }
}


