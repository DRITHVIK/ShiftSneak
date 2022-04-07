using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(ObjectTypeAttribute))]
    public class ObjectTypeAttributeDrawer : NgnProperyDrawer
    {
        private ObjectTypeAttribute attributeSource;

        protected override void Initialize(SerializedProperty property, int index)
        {
            attributeSource = (ObjectTypeAttribute)attribute;
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);
            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            property.objectReferenceValue = EditorGUI.ObjectField(position, property.objectReferenceValue, attributeSource.type, false);
        }
    }

}

