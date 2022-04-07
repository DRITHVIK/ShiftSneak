using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace DigitomUtilities
{
    [CustomPropertyDrawer(typeof(PrefabRequisiteAttribute))]
    public class DefaultValueAttributeDrawer : NgnProperyDrawer
    {

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            RemovePropertyValueIfNotPrefab(property);
            return base.CreatePropertyGUI(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RemovePropertyValueIfNotPrefab(property);
            EditorGUI.PropertyField(position, property, label);
        }

        private void RemovePropertyValueIfNotPrefab(SerializedProperty property)
        {
            var attributeSource = (DefaultValueAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == default)
                    property.objectReferenceValue = (Object)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.AnimationCurve)
            {
                if (property.animationCurveValue == default)
                    property.animationCurveValue = (AnimationCurve)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Boolean)
            {
                if (property.boolValue == default)
                    property.boolValue = (bool)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Color)
            {
                if (property.colorValue == default)
                    property.colorValue = (Color)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.ArraySize ||
                property.propertyType == SerializedPropertyType.Enum ||
                property.propertyType == SerializedPropertyType.Integer ||
                property.propertyType == SerializedPropertyType.LayerMask)
            {
                if (property.intValue == default)
                    property.intValue = (int)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                if (property.floatValue == default)
                    property.floatValue = (float)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Quaternion)
            {
                if (property.quaternionValue == default)
                    property.quaternionValue = (Quaternion)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Rect)
            {
                if (property.rectValue == default)
                    property.rectValue = (Rect)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.RectInt)
            {
                if (property.rectIntValue.Equals(default))
                    property.rectIntValue = (RectInt)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.String)
            {
                if (property.stringValue.Equals(default))
                    property.stringValue = (string)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Vector2)
            {
                if (property.vector2Value == default)
                    property.vector2Value = (Vector2)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                if (property.vector2IntValue == default)
                    property.vector2IntValue = (Vector2Int)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Vector3)
            {
                if (property.vector3Value == default)
                    property.vector3Value = (Vector3)attributeSource.value;
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                if (property.vector3IntValue == default)
                    property.vector3IntValue = (Vector3Int)attributeSource.value;
            }
            
        }
    }
}

