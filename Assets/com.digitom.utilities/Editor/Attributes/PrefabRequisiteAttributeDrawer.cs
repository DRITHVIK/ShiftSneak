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
    public class PrefabRequisiteAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            RemovePropertyValueIfNotPrefab(property);
            return base.CreatePropertyGUI(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RemovePropertyValueIfNotPrefab(property);
            property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, System.Type.GetType(property.type), false);
        }

        private void RemovePropertyValueIfNotPrefab(SerializedProperty property)
        {
            if (property.objectReferenceValue == null) return;
            var go = property.objectReferenceValue as GameObject;
            if (go)
            {
                if (!go.IsPrefab())
                {
                    Debug.LogError($"{go} must be a prefab when using {typeof(PrefabRequisiteAttribute)}!");
                    property.objectReferenceValue = null;
                }
            }
            else
            {
                Debug.LogError($"{property.objectReferenceValue} must be {typeof(GameObject)} when using {typeof(PrefabRequisiteAttribute)}!");
                property.objectReferenceValue = null;
            }
        }
    }

}

