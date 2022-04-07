using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace DigitomUtilities
{
    [CustomPropertyDrawer(typeof(SubTypeAttribute))]
    public class SubTypeDrawer : NgnProperyDrawer
    {
        protected SubTypeAttribute attributeSource;

        protected Dictionary<int, WrapperValue<int>> selects = new Dictionary<int, WrapperValue<int>>();
        protected Dictionary<int, WrapperValue<System.Type[]>> subTypesContainer = new Dictionary<int, WrapperValue<System.Type[]>>();
        protected Dictionary<int, WrapperValue<string[]>> subTypeNamesContainer = new Dictionary<int, WrapperValue<string[]>>();

        protected override void Initialize(SerializedProperty property, int index)
        {
            attributeSource = (SubTypeAttribute)attribute;
            if (property.stringValue == "")
                property.stringValue = attributeSource.baseType.AssemblyQualifiedName;
            //set selection if field already has value
            var sel = selects.GetOrAddValue(index);
            var names = subTypeNamesContainer.GetOrAddValue(index);
            var types = subTypesContainer.GetOrAddValue(index);
            types.Value = TypeUtilities.GetAllSubclasses(attributeSource.baseType, OrderType.Ascending);
            names.Value = TypeUtilities.GetAllSubclassNames(attributeSource.baseType, OrderType.Ascending);
            sel.Value = types.Value.ToList().FindIndex(x => x == System.Type.GetType(property.stringValue));
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);

            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            selects.TryGetValue(index, out var sel);
            subTypesContainer.TryGetValue(index, out var types);
            subTypeNamesContainer.TryGetValue(index, out var names);


            sel.Value = attributeSource.displayLabel ? EditorGUI.Popup(position, property.displayName, sel.Value, names.Value)
                : EditorGUI.Popup(position, sel.Value, names.Value);
            property.stringValue = types.Value[sel.Value].AssemblyQualifiedName;
        }
    }

}

