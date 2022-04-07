using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(PopUpSelectionAttribute))]
    public class PopUpSelectionDrawer : NgnProperyDrawer
    {
        private PopUpSelectionAttribute attributeSource;

        protected override void Initialize(SerializedProperty property, int index)
        {
            attributeSource = (PopUpSelectionAttribute)attribute;
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);

            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var names = ReflectionUtils.GetNestedValue<string[]>(target, attributeSource.selection).ToList();
            names.Insert(0, attributeSource.titleName);
            property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, names.ToArray());

            if (property.intValue != 0)
            {
                var method = ReflectionUtils.GetMethod(target, attributeSource.callBack);
                method.Invoke(target, new object[] { property.intValue - 1 });
                property.intValue = 0;
                serializedTarget.ApplyModifiedProperties();
            }
        }
    }

}

