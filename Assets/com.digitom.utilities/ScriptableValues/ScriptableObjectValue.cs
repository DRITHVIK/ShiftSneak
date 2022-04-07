using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [CreateAssetMenu(menuName = "Ngn/Values/Object")]
    public class ScriptableObjectValue : ScriptableValue<Object> 
    {
        [SubType(typeof(Object), false)] public string type;
    }
}


