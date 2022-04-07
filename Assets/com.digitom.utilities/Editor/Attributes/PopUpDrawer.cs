using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace DigitomUtilities
{
    [CustomPropertyDrawer(typeof(PopUpAttribute))]
    public class PopUpDrawer : NgnProperyDrawer
    {
        protected PopUpAttribute attributeSource;

        protected Dictionary<int, WrapperValue<int>> selects = new Dictionary<int, WrapperValue<int>>();
        protected Dictionary<int, WrapperValue<string[]>> menus = new Dictionary<int, WrapperValue<string[]>>();
        protected Dictionary<int, WrapperValue<object[]>> objContainer = new Dictionary<int, WrapperValue<object[]>>();

        protected override void Initialize(SerializedProperty property, int index)
        {
            attributeSource = (PopUpAttribute)attribute;

            var obj = ReflectionUtils.GetNestedObjectValue(target, attributeSource.selection);
            var objs = ((IEnumerable)obj).Cast<object>();
            var sel = selects.GetOrAddValue(index);
            var val = property.GetPropertyObjectValue();
            if (val != null)
                sel.Value = objs.ToList().FindIndex(x => x.Equals(val));

        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);

            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            selects.TryGetValue(index, out var sel);
            var menu = menus.GetOrAddValue(index);
            var objs = objContainer.GetOrAddValue(index);

            if (menu.Value == null)
            {
                objs.Value = ReflectionUtils.GetNestedCollection<object>(target, attributeSource.selection).ToArray();

                menu.Value = objs.Value.Select((x, i) =>
                {
                    return x.ToString();
                }).ToArray();


            }

            sel.Value = EditorGUI.Popup(position, property.displayName, sel.Value, menu.Value);
            if (sel.Value < objs.Value.Length && sel.Value > -1)
                property.SetPropertyObjectValue(objs.Value[sel.Value]);
        }
    }

}

