using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DigitomUtilities
{
    [System.Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class ComponentOfTypeAttribute : PropertyAttribute
    {
        public Type type;
        public Object lastObj;

        public ComponentOfTypeAttribute(Type _type)
        {
            type = _type;
        }
    }
}


