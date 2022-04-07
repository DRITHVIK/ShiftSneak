using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace DigitomUtilities
{
    [CustomPropertyDrawer(typeof(ExpandableObjectAttribute))]
    public class ExpandableObjectDrawer : NgnProperyDrawer
    {
        private Dictionary<int, WrapperValue<long>> localIDs = new Dictionary<int, WrapperValue<long>>();
        private Dictionary<int, WrapperValue<int>> createSel = new Dictionary<int, WrapperValue<int>>();
        private Dictionary<int, WrapperValue<int>> interfaceSel = new Dictionary<int, WrapperValue<int>>();
        private Dictionary<int, WrapperValue<string[]>> createNames = new Dictionary<int, WrapperValue<string[]>>();
        private Dictionary<int, WrapperValue<string[]>> interfaceNames = new Dictionary<int, WrapperValue<string[]>>();
        private Dictionary<int, WrapperValue<System.Type[]>> subTypes = new Dictionary<int, WrapperValue<System.Type[]>>();

        protected override void Initialize(SerializedProperty property, int index)
        {
            var cNames = createNames.GetOrAddValue(index);
            var iNames = interfaceNames.GetOrAddValue(index);
            var types = subTypes.GetOrAddValue(index);

            types.Value = new System.Type[] { null };
            types.Value = types.Value.Concat(sourceType.GetAllSubclasses()).ToArray();

            cNames.Value = new string[] { "+" };
            iNames.Value = new string[] { "i" };
            var names = sourceType.GetAllSubclassNames();
            cNames.Value = cNames.Value.Concat(names).ToArray();
            iNames.Value = iNames.Value.Concat(names).ToArray();
        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            propertyHeight = lineHeight;

            if (property.objectReferenceValue)
            {
                var serObj = new SerializedObject(property.objectReferenceValue);
                var exp = expandeds.GetOrAddValue(index);
                if (exp.Value)
                {
                    var it = serObj.GetIterator();
                    while (it.NextVisible(true))
                    {
                        if (Skip(it)) continue;
                        propertyHeight += EditorGUI.GetPropertyHeight(it, true) + spacing;
                    }
                    propertyHeight += spacing;
                }
            }
            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var exp = expandeds.GetOrAddValue(index);

            //expandable object dropdown
            if (property.objectReferenceValue != null)
            {

                float buttonSize = 20;
                var deleteButtonRect = new Rect(position.width - spacing, position.y, buttonSize, lineHeight);
                var propRect = new Rect(position.x, position.y, position.width - deleteButtonRect.width - spacing, lineHeight);
                EditorGUI.PropertyField(propRect, property);
                var serObj = new SerializedObject(property.objectReferenceValue);

                var style = EditorStyles.foldout;
                var dropRect = new Rect(position.x - 13 + (EditorGUI.indentLevel * 15), position.y, EditorGUIUtility.labelWidth, lineHeight);
                //arrow in foldout not selectable unless you do this?
                //pos.x -= 15;
                //exp.Value = EditorGUI.Foldout(pos, exp.Value, GUIContent.none, GUIStyle.none);
                exp.Value = EditorGUI.Toggle(dropRect, exp.Value, style);
                //pos.x += 15;
                //EditorGUI.Foldout(pos, exp.Value, GUIContent.none);

                //dropdown properties
                var dropPropRect = new Rect(position.x, position.y, position.width, lineHeight);
                if (exp.Value)
                {
                    EditorGUI.indentLevel++;
                    //draw box around property
                    var rectDraw = new Rect(position.x + EditorGUI.indentLevel * 15 - 15, position.y + space - spacing,
                        position.width - (EditorGUI.indentLevel * 15) + spacing + 15, propertyHeight - lineHeight);
                    EditorGUI.HelpBox(rectDraw, "", MessageType.None);

                    var it = serObj.GetIterator();
                    dropPropRect.y += space;

                    while (it.NextVisible(true))
                    {
                        if (Skip(it)) continue;
                        EditorGUI.PropertyField(dropPropRect, it, true);
                        dropPropRect.y += EditorGUI.GetPropertyHeight(it, true) + spacing;
                    }

                    EditorGUI.indentLevel--;
                }
                serObj.ApplyModifiedProperties();

                //delete button
                if (GUI.Button(deleteButtonRect, "x"))
                {
                    if (EditorUtility.IsPersistent(serializedTarget.targetObject))
                    {
                        AssetDatabase.RemoveObjectFromAsset(property.objectReferenceValue);
                        AssetDatabase.SaveAssets();
                    }
                    property.objectReferenceValue = null;
                    serializedTarget.ApplyModifiedProperties();
                    return;
                }

            }
            else
            {
                subTypes.TryGetValue(index, out var types);
                createNames.TryGetValue(index, out var cNames);
                interfaceNames.TryGetValue(index, out var iNames);
                var iSel = interfaceSel.GetOrAddValue(index);
                var cSel = createSel.GetOrAddValue(index);

                float buttonSize = 20;
                var createButtonRect = new Rect(position.width - spacing - (EditorGUI.indentLevel * 15), position.y, buttonSize + (EditorGUI.indentLevel * 15), lineHeight);
                var interfaceButtonRect = new Rect(position.width - createButtonRect.width - spacing * 2, position.y, buttonSize + (EditorGUI.indentLevel * 15), lineHeight);
                var propRect = new Rect(position.x, position.y, position.width - createButtonRect.width - interfaceButtonRect.width - spacing * 2 + (2 * (EditorGUI.indentLevel * 15)), lineHeight);
                var style = EditorStyles.miniButton;

                if (types.Value.Length > 1)
                {
                    //interface subtype selection menu
                    iSel.Value = EditorGUI.Popup(interfaceButtonRect, iSel.Value, iNames.Value, style);
                    if (iSel.Value != 0)
                    {
                        var t = types.Value[iSel.Value];
                        var obj = ScriptableObject.CreateInstance(t);
                        if (EditorUtility.IsPersistent(serializedTarget.targetObject))
                        {
                            AssetDatabase.AddObjectToAsset(obj, serializedTarget.targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        property.objectReferenceValue = obj;
                        serializedTarget.ApplyModifiedProperties();
                        iSel.Value = 0;
                        return;
                    }

                    //creation subtype menu
                    cSel.Value = EditorGUI.Popup(createButtonRect, cSel.Value, cNames.Value, style);
                    if (cSel.Value != 0)
                    {
                        var t = types.Value[cSel.Value];
                        var obj = AssetUtilities.CreateAssetWithSavePrompt(t, Application.dataPath);
                        if (obj != null)
                            property.objectReferenceValue = obj;
                        cSel.Value = 0;
                        return;
                    }
                }
                else
                {
                    //create instant interface of this type
                    if (GUI.Button(interfaceButtonRect, "i"))
                    {
                        var obj = ScriptableObject.CreateInstance(sourceType);
                        property.objectReferenceValue = obj;
                        serializedTarget.ApplyModifiedProperties();
                        return;
                    }

                    //create scriptable object of this type
                    if (GUI.Button(createButtonRect, "+", style))
                    {
                        var obj = AssetUtilities.CreateAssetWithSavePrompt(property.GetPropertySystemType(), Application.dataPath);
                        if (obj != null)
                            property.objectReferenceValue = obj;
                    }
                }


                EditorGUI.PropertyField(propRect, property);
            }

        }

        bool Skip(SerializedProperty prop)
        {
            return prop.name == "m_Script" ||
                prop.propertyType == SerializedPropertyType.ArraySize ||
                prop.name == "x" ||
                prop.name == "y" ||
                prop.name == "z" ||
                prop.name == "w" ||
                prop.IsArrayElement();
        }
    }
}


