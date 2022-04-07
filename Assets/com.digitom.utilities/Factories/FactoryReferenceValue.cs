using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public static class FactoryReferenceValue
    {
        public static object CreateReferenceValueObject(System.Type _type)
        {
            if (_type == typeof(bool))
                return new ReferenceBool();
            else if (_type.IsEnum)
                return new ReferenceEnum(_type);
            else if (_type == typeof(int))
                return new ReferenceInt();
            else if (_type == typeof(float))
                return new ReferenceFloat();
            else if (_type == typeof(Object) || _type.IsAssignableFrom(typeof(Object)) || _type.IsSubclassOf(typeof(Object)))
                return new ReferenceObject(_type);
            else if (_type == typeof(Quaternion))
                return new ReferenceQuaternion();
            else if (_type == typeof(string))
                return new ReferenceString();
            else if (_type == typeof(Vector2))
                return new ReferenceVector2();
            else if (_type == typeof(Vector3))
                return System.Activator.CreateInstance(typeof(ReferenceVector3));
            else if (_type == typeof(Vector4))
                return new ReferenceVector4();

            Debug.LogError("Could not convert type: " + _type);
            return default;
        }
    }
}


