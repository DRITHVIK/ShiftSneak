using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(NgnScriptableReference<>), true)]
    public class NgnScriptableReferenceDrawer : NgnProperyDrawer
    {
        private Dictionary<int, WrapperValue<int>> lastReferenceTypes = new Dictionary<int, WrapperValue<int>>();

        protected override void Initialize(SerializedProperty property, int index)
        {
            var referenceType = property.FindPropertyRelative("referenceType");
            var last = lastReferenceTypes.GetOrAddValue(index);
            last.Value = referenceType.intValue;
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            //set height
            propertyHeight = lineHeight;
            var objectReference = property.FindPropertyRelative("objectReference");
            if (objectReference.objectReferenceValue)
            {
                var serObj = new SerializedObject(objectReference.objectReferenceValue);
                var exp = expandeds.GetOrAddValue(index);
                if (exp.Value)
                {
                    var it = serObj.GetIterator();
                    while (it.NextVisible(true))
                    {
                        if (Skip(it)) continue;
                        propertyHeight += EditorGUI.GetPropertyHeight(it, true) + spacing;
                    }
                    propertyHeight += lineHeight;
                }
            }

            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var objectReference = property.FindPropertyRelative("objectReference");
            var referenceType = property.FindPropertyRelative("referenceType");
            var last = lastReferenceTypes.GetOrAddValue(index);

            //label field
            string name = GetPropertyName(property);
            var pos = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, lineHeight);
            DisplayLabelField(pos, property, name, index);

            if (objectReference.objectReferenceValue != null)
            {
                //arrow in foldout not selectable unless you do this?
                var exp = expandeds.GetOrAddValue(index);
                DisplayFoldoutArrow(pos, exp);

                if (exp.Value)
                {
                    DisplayObjectDropDownField(position, property, label, index);
                }
            }


            float buttonSize = GetRightWidth(property);
            pos = new Rect(position.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15 + spacing, position.y,
                    position.width - EditorGUIUtility.labelWidth - buttonSize + EditorGUI.indentLevel * 15 - spacing * 2, lineHeight);
            //object field
            if (referenceType.intValue != (int)ReferenceType.Interface)
            {
                var type = property.GetPropertySystemType();
                DisplayObjectField(pos, property, type, index);
            }
            else
                DisplayInterfaceField(pos, property, index);

            DisplayReferenceTypeSelectionField(position, property, label, index);

            //reset fields
            if (!Application.isPlaying)
            {
                if (objectReference.objectReferenceValue)
                {
                    if (last.Value != referenceType.intValue)
                    {
                        if (last.Value == (int)ReferenceType.Interface || referenceType.intValue == (int)ReferenceType.Interface)
                            objectReference.objectReferenceValue = null;
                    }
                }

                //create an interface layout
                if (referenceType.intValue == (int)ReferenceType.Interface)
                {
                    if (!objectReference.objectReferenceValue)
                    {
                        var creator = source as IScriptableReference;
                        if (creator != null)
                        {
                            var obj = creator.CreateObjectReferenceInstance();
                            objectReference.objectReferenceValue = obj;
                        }

                    }

                }

                last.Value = referenceType.intValue;
            }

        }

        bool Skip(SerializedProperty prop)
        {
            return prop.name == "m_Script" ||
                prop.name == "size" ||
                prop.IsArrayElement();
        }

        protected virtual void DisplayLabelField(Rect position, SerializedProperty property, string name, int index)
        {
            EditorGUI.LabelField(position, name);
        }

        protected virtual void DisplayFoldoutArrow(Rect pos, WrapperBool exp)
        {
            pos.x -= 10;
            exp.Value = EditorGUI.Foldout(pos, exp.Value, GUIContent.none, GUIStyle.none);
            pos.x += 10;
            EditorGUI.Foldout(pos, exp.Value, GUIContent.none);
        }

        protected virtual void DisplayObjectField(Rect position, SerializedProperty property, System.Type type, int index)
        {
            var objectReference = property.FindPropertyRelative("objectReference");
            objectReference.objectReferenceValue = EditorGUI.ObjectField(position, objectReference.objectReferenceValue, type, false);
        }

        protected virtual void DisplayInterfaceField(Rect position, SerializedProperty property, int index)
        {
        }

        protected virtual void DisplayObjectDropDownField(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var pos = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, lineHeight);
            EditorGUI.indentLevel++;
            //draw box around property
            var rectDraw = new Rect(position.x + EditorGUI.indentLevel * 15 - 15, position.y + space - spacing,
                position.width - (EditorGUI.indentLevel * 15) + spacing + 15, propertyHeight - lineHeight);
            EditorGUI.HelpBox(rectDraw, "", MessageType.None);

            var objectReference = property.FindPropertyRelative("objectReference");
            var serObj = new SerializedObject(objectReference.objectReferenceValue);
            var it = serObj.GetIterator();
            pos.width = position.width;
            pos.y += space;
            while (it.NextVisible(true))
            {
                if (Skip(it)) continue;
                EditorGUI.PropertyField(pos, it, true);
                pos.y += EditorGUI.GetPropertyHeight(it, true) + spacing;

            }
            serObj.ApplyModifiedProperties();
            EditorGUI.indentLevel--;

        }

        protected virtual void DisplayReferenceTypeSelectionField(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            float buttonSize = GetRightWidth(property);
            var referenceType = property.FindPropertyRelative("referenceType");
            //reference selection
            var pos = new Rect(position.width - (EditorGUI.indentLevel * 15 + spacing), position.y, buttonSize + EditorGUI.indentLevel * 15, lineHeight);
            referenceType.intValue = EditorGUI.Popup(pos, referenceType.intValue, System.Enum.GetNames(typeof(ReferenceType)));
        }

        protected virtual float GetRightWidth(SerializedProperty property)
        {
            return 20;
        }

        protected virtual string GetPropertyName(SerializedProperty property)
        {
            return property.displayName;
        }

    }

}

