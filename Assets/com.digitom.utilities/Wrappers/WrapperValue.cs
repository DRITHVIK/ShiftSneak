using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DigitomUtilities
{
    [System.Serializable]
    public class WrapperValue<T> : IValue<T>
    {
        [SerializeField] private T value;
        public T Value { get => value; set => this.value = value; }
    }

    [System.Serializable] public class WrapperBool : WrapperValue<bool> { }
    [System.Serializable] public class WrapperArrayString : WrapperValue<string[]> { }
    [System.Serializable] public class WrapperArrayMethodInfo : WrapperValue<MethodInfo[]> { }
    [System.Serializable] public class WrapperArrayParameterInfo : WrapperValue<ParameterInfo[]> { }
}

