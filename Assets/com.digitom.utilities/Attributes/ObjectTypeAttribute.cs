using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [System.Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class ObjectTypeAttribute : PropertyAttribute
    {
        public Type type;

        public ObjectTypeAttribute(Type _type)
        {
            type = _type;
        }
    }
}


