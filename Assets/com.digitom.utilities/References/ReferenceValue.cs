using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public abstract class ReferenceValue<T0, T1> : NgnScriptableReference<T0>, IValue<T1>, IReferenceValue, IReferenceValueManagerVisitor
        where T0 : ScriptableValue<T1>
    {
        public string valueName;
        public string Name { get => valueName; }
        public bool isReference;
        public int refInd;
        [HideInInspector] [SerializeReference] public IReferenceValue[] referenceList;
        public T1 Value
        {
            get
            {
                if (isReference)
                    return (T1)referenceList[refInd - 1].GetObjectValue();
                else if (objectReference)
                    return objectReference.Value;
                else
                    return default;
            }
            set
            {
                if (isReference)
                    referenceList[refInd - 1].SetObjectValue(value);
                else if (objectReference)
                    objectReference.Value = value;
                else
                    Debug.LogError("No object reference set on " + this.Name);
            }
        }

        public void SetObjectValue(object _value)
        {
            Value = (T1)_value;
        }

        public object GetObjectValue()
        {
            return Value;
        }

        public abstract IReferenceValue[] GetValueReferences(ReferenceValueManager _manager);
    }

    [System.Serializable]
    public class ReferenceBool : ReferenceValue<ScriptableBool, bool>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.boolValues;
        }
    }
    [System.Serializable]
    public class ReferenceEnum : ReferenceValue<ScriptableEnum, int>
    {
        public string type;
        public ReferenceEnum(System.Type _type) { type = _type.AssemblyQualifiedName; }

        public override ScriptableObject CreateObjectReferenceInstance()
        {
            var obj = base.CreateObjectReferenceInstance() as ScriptableEnum;
            obj.type = type;
            return obj;
        }

        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.enumValues;
        }
    }
    [System.Serializable]
    public class ReferenceInt : ReferenceValue<ScriptableInt, int>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.intValues;
        }
    }
    [System.Serializable]
    public class ReferenceFloat : ReferenceValue<ScriptableFloat, float>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.floatValues;
        }
    }
    [System.Serializable]
    public class ReferenceObject : ReferenceValue<ScriptableObjectValue, Object>
    {
        [SubType(typeof(Object))] public string type;
        public ReferenceObject(System.Type _type) { type = _type.AssemblyQualifiedName; }

        public override ScriptableObject CreateObjectReferenceInstance()
        {
            var obj = base.CreateObjectReferenceInstance() as ScriptableObjectValue;
            obj.type = type;
            return obj;
        }

        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.objectValues;
        }
    }
    [System.Serializable]
    public class ReferenceQuaternion : ReferenceValue<ScriptableQuaternion, Quaternion>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.quaternionValues;
        }
    }
    [System.Serializable]
    public class ReferenceString : ReferenceValue<ScriptableString, string>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.stringValues;
        }
    }
    [System.Serializable]
    public class ReferenceVector2 : ReferenceValue<ScriptableVector2, Vector2>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.vector2Values;
        }
    }
    [System.Serializable]
    public class ReferenceVector3 : ReferenceValue<ScriptableVector3, Vector3>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.vector3Values;
        }
    }
    [System.Serializable]
    public class ReferenceVector4 : ReferenceValue<ScriptableVector4, Vector4>
    {
        public override IReferenceValue[] GetValueReferences(ReferenceValueManager _manager)
        {
            return _manager.vector4Values;
        }
    }
}


