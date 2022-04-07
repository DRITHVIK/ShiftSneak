using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(ReferenceValueProperty))]
    public class ReferenceValuePropertyDrawer : NgnProperyDrawer
    {
        protected Dictionary<int, WrapperValue<SerializedProperty>> chosenProps = new Dictionary<int, WrapperValue<SerializedProperty>>();

        protected override void Initialize(SerializedProperty property, int index)
        {
            
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            //set height
            propertyHeight = lineHeight;
            var chosenProp = chosenProps.GetOrAddValue(index);
            if (chosenProp.Value != null)
                propertyHeight = EditorGUI.GetPropertyHeight(chosenProp.Value);
            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var name = property.FindPropertyRelative("name");
            var valueType = property.FindPropertyRelative("valueType");
            var boolValue = property.FindPropertyRelative("boolValue");
            var enumValue = property.FindPropertyRelative("enumValue");
            var intValue = property.FindPropertyRelative("intValue");
            var floatValue = property.FindPropertyRelative("floatValue");
            var objectValue = property.FindPropertyRelative("objectValue");
            var quaternionValue = property.FindPropertyRelative("quaternionValue");
            var stringValue = property.FindPropertyRelative("stringValue");
            var vector2Value = property.FindPropertyRelative("vector2Value");
            var vector3Value = property.FindPropertyRelative("vector3Value");
            var vector4Value = property.FindPropertyRelative("vector4Value");

            var chosenProp = chosenProps.GetOrAddValue(index);
            var type = System.Type.GetType(valueType.stringValue);
            if (type == null)
                type = typeof(Object);

            if (type == typeof(bool))
                chosenProp.Value = boolValue;
            else if (type.IsEnum)
                chosenProp.Value = enumValue;
            else if (type == typeof(int))
                chosenProp.Value = intValue;
            else if (type == typeof(float))
                chosenProp.Value = floatValue;
            else if (type == typeof(Object))
                chosenProp.Value = objectValue;
            else if (type == typeof(Quaternion))
                chosenProp.Value = quaternionValue;
            else if (type == typeof(string))
                chosenProp.Value = stringValue;
            else if (type == typeof(Vector2))
                chosenProp.Value = vector2Value;
            else if (type == typeof(Vector3))
                chosenProp.Value = vector3Value;
            else if (type == typeof(Vector4))
                chosenProp.Value = vector4Value;
            else
                chosenProp.Value = objectValue;

            if (chosenProp.Value != null)
            {
                var t = chosenProp.Value.FindPropertyRelative("type");
                var v = chosenProp.Value.FindPropertyRelative("valueName");
                v.stringValue = name.stringValue;
                if (t != null)
                    t.stringValue = valueType.stringValue;
                EditorGUI.PropertyField(position, chosenProp.Value, true);
            }
                

        }

    }

}

