using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public interface IGenericValue
    {
        T GetValue<T>();
        void SetValue<T>(T _value);
    }
}


