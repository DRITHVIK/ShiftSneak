using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    //have to use this class until serialize reference is stable...
    [System.Serializable]
    public class ReferenceValueProperty
    {
        public string name;
        public string valueType;
        public ReferenceValueProperty(string _name, string _valueType)
        {
            name = _name;
            valueType = _valueType;

            boolValue.valueName = name;
            enumValue.valueName = name;
            intValue.valueName = name;
            floatValue.valueName = name;
            objectValue.valueName = name;
            quaternionValue.valueName = name;
            stringValue.valueName = name;
            vector2Value.valueName = name;
            vector3Value.valueName = name;
            vector4Value.valueName = name;
        }

        public ReferenceBool boolValue;
        public ReferenceEnum enumValue;
        public ReferenceInt intValue;
        public ReferenceFloat floatValue;
        public ReferenceObject objectValue;
        public ReferenceQuaternion quaternionValue;
        public ReferenceString stringValue;
        public ReferenceVector2 vector2Value;
        public ReferenceVector3 vector3Value;
        public ReferenceVector4 vector4Value;

        public void SetObjectValue(object _value)
        {
            var type = valueType != "" ? System.Type.GetType(valueType) : _value.GetType();
            if (type == typeof(bool))
                boolValue.Value = (bool)_value;
            else if (type.IsEnum)
                enumValue.Value = (int)_value;
            else if (type == typeof(int))
                intValue.Value = (int)_value;
            else if (type == typeof(float))
                floatValue.Value = (float)_value;
            else if (type == typeof(Object) || type.IsAssignableFrom(typeof(Object)) || type.IsSubclassOf(typeof(Object)))
                objectValue.Value = (Object)_value;
            else if (type == typeof(Quaternion))
                quaternionValue.Value = (Quaternion)_value;
            else if (type == typeof(string))
                stringValue.Value = (string)_value;
            else if (type == typeof(Vector2))
                vector2Value.Value = (Vector2)_value;
            else if (type == typeof(Vector3))
                vector3Value.Value = (Vector3)_value;
            else if (type == typeof(Vector4))
                vector4Value.Value = (Vector4)_value;
            else
                Debug.Log("Could not set valuetype: " + type);
        }

        public object GetObjectValue()
        {
            var type = System.Type.GetType(valueType);
            if (type == typeof(bool))
                return boolValue.Value;
            else if (type.IsEnum)
                return enumValue.Value;
            else if (type == typeof(int))
                return intValue.Value;
            else if (type == typeof(float))
                return floatValue.Value;
            else if (type == typeof(Object) || type.IsAssignableFrom(typeof(Object)) || type.IsSubclassOf(typeof(Object)))
                return objectValue.Value;
            else if (type == typeof(Quaternion))
                return quaternionValue.Value;
            else if (type == typeof(string))
                return stringValue.Value;
            else if (type == typeof(Vector2))
                return vector2Value.Value;
            else if (type == typeof(Vector3))
                return vector3Value.Value;
            else if (type == typeof(Vector4))
                return vector4Value.Value;

            Debug.Log("Could not return valuetype: " + type);
            return default;
        }
    }
}


