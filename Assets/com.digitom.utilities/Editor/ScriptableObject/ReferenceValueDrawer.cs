using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(IReferenceValue), true)]
    public class ReferenceValueDrawer : NgnScriptableReferenceDrawer
    {
        private Dictionary<int, WrapperValue<IReferenceValue[]>> refArrays = new Dictionary<int, WrapperValue<IReferenceValue[]>>();

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            var height = lineHeight;
            var referenceType = property.FindPropertyRelative("referenceType");
            if (referenceType.intValue == (int)ReferenceType.Interface)
            {
                var objectReference = property.FindPropertyRelative("objectReference");
                if (objectReference.objectReferenceValue)
                {
                    var serObj = new SerializedObject(objectReference.objectReferenceValue);
                    var value = serObj.FindProperty("value");
                    height += EditorGUI.GetPropertyHeight(value, true);
                    height -= lineHeight;
                }
            }
            return height;
        }

        protected override void Initialize(SerializedProperty property, int index)
        {
            base.Initialize(property, index);

            var comp = target as Component;
            var refMan = comp.GetComponent<ReferenceValueManager>();
            if (refMan && target.GetType() != typeof(ReferenceValueManager))
            {
                var isReference = property.FindPropertyRelative("isReference");
                isReference.boolValue = true;

                var refArray = refArrays.GetOrAddValue(index);
                refArray.Value = refMan.GetValueReferences(source as IReferenceValueManagerVisitor);
            }
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            base.SetOnGUI(position, property, label, index);
            var comp = target as Component;
            var refMan = comp.GetComponent<ReferenceValueManager>();
            var isReference = property.FindPropertyRelative("isReference");
            var refInd = property.FindPropertyRelative("refInd");
            if (refMan)
            {
                if (!(target is ReferenceValueManager))
                    isReference.boolValue = refInd.intValue > 0;
                else
                {
                    isReference.boolValue = false;
                    refInd.intValue = 0;
                }


            }
            else
            {
                isReference.boolValue = false;
                refInd.intValue = 0;
            }

        }

        protected override string GetPropertyName(SerializedProperty property)
        {
            var valueName = property.FindPropertyRelative("valueName");
            return valueName.stringValue != "" ? valueName.stringValue : base.GetPropertyName(property);
        }

        protected override void DisplayLabelField(Rect position, SerializedProperty property, string name, int index)
        {
            var comp = target as Component;
            var refMan = comp.GetComponent<ReferenceValueManager>();
            if (refMan)
            {
                var valueName = property.FindPropertyRelative("valueName");
                if (target is ReferenceValueManager)
                {
                    var textFieldPos = new Rect(position.x, position.y, position.width - 18, lineHeight);
                    valueName.stringValue = EditorGUI.TextField(textFieldPos, valueName.stringValue);
                }

                else
                    base.DisplayLabelField(position, property, name, index);
            }
            else
                base.DisplayLabelField(position, property, name, index);
        }

        protected override void DisplayObjectField(Rect position, SerializedProperty property, System.Type type, int index)
        {
            var comp = target as Component;
            var refMan = comp.GetComponent<ReferenceValueManager>();
            if (refMan)
            {
                var isReference = property.FindPropertyRelative("isReference");

                var refInd = property.FindPropertyRelative("refInd");
                isReference.boolValue = refInd.intValue > 0;
                if (isReference.boolValue)
                {
                    //reference selection
                    var refArray = refArrays.GetOrAddValue(index);
                    refArray.Value = refMan.GetValueReferences(source as IReferenceValueManagerVisitor);
                    if (refArray.Value == null) return;
                    refInd.intValue = Mathf.Clamp(refInd.intValue, 0, refArray.Value.Length);

                    //set reference list on target
                    var referenceList = property.FindPropertyRelative("referenceList");
                    referenceList.SetPropertyObjectValue(refArray.Value);
                    serializedTarget.ApplyModifiedProperties();

                    if (refArray.Value.Length > 0)
                    {
                        var selected = refArray.Value[refInd.intValue - 1];
                        string refName = selected.Name;
                        var obj = selected.GetObjectValue();
                        if (obj != null)
                        {
                            var refValue = obj.ToString();
                            EditorGUI.LabelField(position, refName + ": " + refValue);
                        }
                    }
                    else
                        EditorGUI.LabelField(position, "empty");

                }
                else
                    base.DisplayObjectField(position, property, property.GetPropertySystemType(), index);

            }
            else
                base.DisplayObjectField(position, property, property.GetPropertySystemType(), index);
        }

        protected override void DisplayInterfaceField(Rect position, SerializedProperty property, int index)
        {
            var objectReference = property.FindPropertyRelative("objectReference");
            var isReference = property.FindPropertyRelative("isReference");
            isReference.boolValue = false;
            if (objectReference.objectReferenceValue)
            {
                var serObj = new SerializedObject(objectReference.objectReferenceValue);
                var value = serObj.FindProperty("value");
                EditorGUI.PropertyField(position, value, GUIContent.none, true);
                serObj.ApplyModifiedProperties();
            }
        }

        protected override void DisplayReferenceTypeSelectionField(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var comp = target as Component;
            var refMan = comp.GetComponent<ReferenceValueManager>();
            if (refMan)
            {
                var referenceType = property.FindPropertyRelative("referenceType");
                var refInd = property.FindPropertyRelative("refInd");
                if (!(target is ReferenceValueManager))
                {
                    var refArray = refArrays.GetOrAddValue(index);
                    refArray.Value = refMan.GetValueReferences(source as IReferenceValueManagerVisitor);
                    if (refArray.Value == null) return;
                    float buttonSize = GetRightWidth(property);
                    //reference selection
                    var pos = new Rect(position.width - (EditorGUI.indentLevel * 15 + spacing), position.y, buttonSize + EditorGUI.indentLevel * 15, lineHeight);

                    var names = refArray.Value.Select((x, i) => "[" + i + "] " + x.Name).ToList();
                    names.Insert(0, "[none]");

                    refInd.intValue = EditorGUI.Popup(pos, refInd.intValue, names.ToArray());
                    if (refInd.intValue == 0)
                        referenceType.intValue = (int)ReferenceType.Interface;
                    else
                        referenceType.intValue = 0;
                }
                else
                    base.DisplayReferenceTypeSelectionField(position, property, label, index);
            }
            else
                base.DisplayReferenceTypeSelectionField(position, property, label, index);
        }

        protected override void DisplayFoldoutArrow(Rect pos, WrapperBool exp)
        {
        }

        protected override void DisplayObjectDropDownField(Rect position, SerializedProperty property, GUIContent label, int index)
        {
        }

    }

}

