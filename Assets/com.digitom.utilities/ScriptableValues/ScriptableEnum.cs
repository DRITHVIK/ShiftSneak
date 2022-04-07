using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [CreateAssetMenu(menuName = "Ngn/Values/Enum")]
    public class ScriptableEnum : ScriptableValue<int> 
    {
        public string type;
    }
}


