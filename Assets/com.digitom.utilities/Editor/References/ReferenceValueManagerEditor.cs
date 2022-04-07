using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(ReferenceValueManager))]
    [CanEditMultipleObjects]
    public class ReferenceValueManagerEditor : Editor
    {
        private SerializedProperty it;
        private int index;
        void OnEnable()
        {
            it = serializedObject.GetIterator();
            index = 0;
        }

        public override void OnInspectorGUI()
        {
            //index selection
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Property Names:", "Values:");
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(5), GUILayout.ExpandWidth(false));
            index = EditorGUILayout.Popup(index, GetValueNames());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            if (index != 0)
            {
                //add value to selected list
                var prop = serializedObject.FindProperty(GetValueNames()[index] + "Values");
                prop.InsertArrayElementAtIndex(prop.arraySize);
                var ele = prop.GetArrayElementAtIndex(prop.arraySize - 1);
                var objRef = ele.FindPropertyRelative("objectReference");
                var valueName = ele.FindPropertyRelative("valueName");
                valueName.stringValue = "";
                objRef.objectReferenceValue = null;
                index = 0;
            }

            //values list
            it = serializedObject.GetIterator();
            while (it.NextVisible(true))
            {
                if (Skip(it)) continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(it, true);

                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(12));
                if (GUILayout.Button("x"))
                {
                    var pathFixed = it.propertyPath.Replace("Array.", "");
                    var split = pathFixed.Split('.');
                    var path = split[split.Length - 2];
                    var prop = serializedObject.FindProperty(path);
                    prop.DeleteArrayElementAtIndex(it.ArrayElementIndex());
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        bool Skip(SerializedProperty prop)
        {
            return !(prop.GetPropertyObjectValue() is IReferenceValue);
        }

        string[] GetValueNames()
        {
            it = serializedObject.GetIterator();
            var names = new List<string>();
            names.Add("+");
            while (it.NextVisible(true))
            {
                if (it.IsTargetMainVariable() && it.name != "m_Script")
                    names.Add(it.name.Replace("Values", ""));
            }
            return names.ToArray();
        }
    }
}
