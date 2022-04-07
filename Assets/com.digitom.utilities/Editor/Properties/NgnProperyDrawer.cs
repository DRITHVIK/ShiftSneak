using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DigitomUtilities
{
    public abstract class NgnProperyDrawer : PropertyDrawer
    {
        protected SerializedObject serializedTarget;
        protected object target;
        protected object source;
        protected System.Type sourceType;
        protected string displayName;
        protected float propertyHeight;
        public float lineHeight;
        protected float spacing;
        protected float space;
        protected Rect fieldRect;

        protected Dictionary<string, int> properties = new Dictionary<string, int>();
        protected Dictionary<int, WrapperBool> inits = new Dictionary<int, WrapperBool>();
        protected Dictionary<int, WrapperBool> expandeds = new Dictionary<int, WrapperBool>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int index = properties.GetOrAddValue(property.propertyPath, properties.Count);
            var initialized = inits.GetOrAddValue(index);

            if (!initialized.Value)
            {
                Setup(property);
                Initialize(property, index);

                serializedTarget.ApplyModifiedProperties();
                initialized.Value = true;
            }
            return SetPropertyHeight(property, label, index);
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propField = new PropertyField(property);
            return propField;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int index = properties.GetOrAddValue(property.propertyPath, properties.Count);
            var initialized = inits.GetOrAddValue(index);

            if (!initialized.Value)
            {
                Setup(property);
                Initialize(property, index);

                serializedTarget.ApplyModifiedProperties();
                initialized.Value = true;
            }

            EditorGUI.BeginProperty(position, label, property);
            SetOnGUI(position, property, label, index);
            serializedTarget?.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        protected virtual void Setup(SerializedProperty property)
        {
            serializedTarget = property.serializedObject;
            target = serializedTarget.targetObject;
            source = property.GetPropertyObjectValue();
            sourceType = property.GetPropertySystemType();
            displayName = property.displayName;
            lineHeight = EditorGUIUtility.singleLineHeight;
            spacing = EditorGUIUtility.standardVerticalSpacing;
            space = lineHeight + spacing;
        }

        protected virtual void Initialize(SerializedProperty property, int index) { }

        protected virtual float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);
            return propertyHeight;
        }

        protected virtual void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index) { }

    }
}


