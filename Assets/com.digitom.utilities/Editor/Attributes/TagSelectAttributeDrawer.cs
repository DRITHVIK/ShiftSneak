using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace DigitomUtilities
{
    [CustomPropertyDrawer(typeof(TagSelectAttribute))]
    public class TagSelectAttributeDrawer : PropertyDrawer
    {
        protected TagSelectAttribute attributeSource;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            attributeSource = (TagSelectAttribute)attribute;
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }

}

