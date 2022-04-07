using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [System.Serializable]
    public class ScriptableValue<T> : ScriptableObject, IValue<T>
    {
        [SerializeField] private T value;
        public T Value { get => value; set => this.value = value; }
    }
}


