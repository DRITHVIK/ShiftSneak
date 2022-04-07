using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public interface ITickableComponentFactory<T0, T> where T0: ITickable
    {
        T0 CreateSystemInstance(T _component);
    }
}

