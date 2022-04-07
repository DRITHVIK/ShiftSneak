using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public interface IReferenceValue
    {
        string Name { get; }
        void SetObjectValue(object _value);
        object GetObjectValue();
    }
}


