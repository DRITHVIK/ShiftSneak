using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public interface IEventableComponentFactory<T0, T> where T0: IEventable
    {
        T0 CreateSystemInstance(T _component);
    }
}

