using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitomUtilities
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class PrefabRequisiteAttribute : PropertyAttribute
    {
    }
}
