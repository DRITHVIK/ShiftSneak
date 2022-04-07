using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace DigitomUtilities
{
    public static class DictionaryUtilities
    {
        public static T GetOrAddValue<T0, T>(this Dictionary<T0, T> dictionary, T0 key, T value = default)
        {
            if (!dictionary.ContainsKey(key))
            {
                if (value == null) value = Activator.CreateInstance<T>();
                dictionary.Add(key, value);
            }
                
            dictionary.TryGetValue(key, out T val);
            return val;
        }

    }
}


