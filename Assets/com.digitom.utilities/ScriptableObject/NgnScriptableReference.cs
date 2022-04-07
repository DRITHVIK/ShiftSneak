using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    [System.Serializable]
    public class NgnScriptableReference<T> : IHaveInitializer, IScriptableReference where T : ScriptableObject
    {
        public ReferenceType referenceType = ReferenceType.Interface;
        public T objectReference;
        
        public virtual ScriptableObject CreateObjectReferenceInstance()
        {
            return ScriptableObject.CreateInstance<T>();
        }

        public virtual void Init()
        {
            if (referenceType == ReferenceType.Instance)
            {
                objectReference = ScriptableObject.Instantiate(objectReference);
            }
        }

    }
}


