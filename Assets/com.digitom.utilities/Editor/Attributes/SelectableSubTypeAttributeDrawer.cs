using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DigitomUtilities
{
    [CustomPropertyDrawer(typeof(SelectableSubTypeAttribute))]
    public class SelectableSubTypeAttributeDrawer : NgnProperyDrawer
    {
        private System.Type type;
        private System.Type[] subs;
        private string[] menuNames;
        private string[] subNames;

        protected override void Initialize(SerializedProperty property, int index)
        {
            var sel = attribute as SelectableSubTypeAttribute;

            type = sel.type ?? property.GetPropertySystemType();
            subs = type.GetAllSubclasses();
            menuNames = new string[] { "Choose " + type.Name.Replace("`1", "").DisplayName() };
            subNames = type.GetAllSubclassNames().ToArray();
            menuNames = menuNames.Concat(subNames).ToArray();
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = EditorGUI.GetPropertyHeight(property, true);
            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            object obj = type.IsSubclassOf(typeof(ScriptableObject)) ? property.objectReferenceValue : property.GetPropertyObjectValue();

            if (!(attribute is SelectableSubTypeAttribute selectable)) return;
            Rect pos = new Rect(position.x, position.y, position.width, lineHeight);

            if (obj == null)
                selectable.selection = EditorGUI.Popup(pos, property.displayName, selectable.selection, menuNames);
            else
            {
                Rect selRect = new Rect(position.width - (EditorGUI.indentLevel * 15 + spacing), position.y, 20 + (EditorGUI.indentLevel * 15), lineHeight);
                selectable.selection = EditorGUI.Popup(selRect, selectable.selection, menuNames);
                string objTypeName = property.displayName.Contains("Element") ? obj.GetType().Name.DisplayName() : property.displayName;
                menuNames[0] = "Replace with . . .";
                pos = new Rect(position.x, position.y, position.width, lineHeight);
                Rect labelRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width, lineHeight);
                EditorGUI.LabelField(labelRect, obj.GetType().Name.DisplayName());
                EditorGUI.PropertyField(pos, property, new GUIContent { text = objTypeName}, true);
                
            }

            if (selectable.selection != 0)
            {
                System.Type selType = subs.Length < 1 ? type : subs[selectable.selection - 1];

                if (selType.IsSubclassOf(typeof(ScriptableObject)))
                {
                    ScriptableObject scriptObj = ScriptableObject.CreateInstance(selType);
                    scriptObj.name = scriptObj.GetType().Name.DisplayName();
                    if (property.serializedObject.targetObject is ScriptableObject)
                    {
                        if (property.objectReferenceValue)
                            AssetDatabase.RemoveObjectFromAsset(property.objectReferenceValue);
                        AssetDatabase.AddObjectToAsset(scriptObj, property.serializedObject.targetObject);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(scriptObj));
                    }

                    if (type.IsInterface)
                        Debug.LogError("Cannot assign objects to interfaces");
                    else
                        property.objectReferenceValue = scriptObj;

                }    
                else
                {
                    obj = System.Activator.CreateInstance(selType);
                    property.managedReferenceValue = obj;
                }   
                serializedTarget.ApplyModifiedPropertiesWithoutUndo();
                
                selectable.selection = 0;
            }

            serializedTarget.ApplyModifiedProperties();
        }
    }

}

