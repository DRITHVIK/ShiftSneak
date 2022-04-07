using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(ComponentOfTypeAttribute))]
    public class ComponentOfTypeDrawer : NgnProperyDrawer
    {
        private ComponentOfTypeAttribute attributeSource;

        protected override void Initialize(SerializedProperty property, int index)
        {
            attributeSource = (ComponentOfTypeAttribute)attribute;
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);
            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            property.objectReferenceValue = EditorGUI.ObjectField(position, property.displayName, property.objectReferenceValue, typeof(Component), true);
            var obj = property.objectReferenceValue;
            if (obj)
            {
                if (!obj.GetType().GetInterfaces().Contains(attributeSource.type))
                {
                    Debug.LogError(obj + " is not of type " + attributeSource.type);
                    if (attributeSource.lastObj)
                        property.objectReferenceValue = attributeSource.lastObj;
                    else
                        property.objectReferenceValue = null;
                }
                attributeSource.lastObj = obj;
            }
        }
    }

}

