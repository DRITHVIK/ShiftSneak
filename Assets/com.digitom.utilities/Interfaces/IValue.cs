using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public interface IValue<T>
    {
        T Value { get; set; }
    }
}


