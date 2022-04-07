using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(ReferenceObject))]
    public class ReferenceObjectDrawer : ReferenceValueDrawer
    {

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            var height = lineHeight;
            var comp = target as Component;
            var refMan = comp.GetComponent<ReferenceValueManager>();
            if (refMan)
            {
                var referenceType = property.FindPropertyRelative("referenceType");
                if (target is ReferenceValueManager && referenceType.intValue == (int)ReferenceType.Interface)
                {
                    height += space;
                }
            }

            return height;
        }

        protected override void Initialize(SerializedProperty property, int index)
        {
            base.Initialize(property, index);
            
        }

        protected override void DisplayObjectField(Rect position, SerializedProperty property, System.Type type, int index)
        {
            var t = property.FindPropertyRelative("type");
            var tValue = System.Type.GetType(t.stringValue);
            base.DisplayObjectField(position, property, tValue, index);
        }

        protected override void DisplayInterfaceField(Rect position, SerializedProperty property, int index)
        {
            var type = property.FindPropertyRelative("type");
            var typeValue = type.stringValue != "" ? System.Type.GetType(type.stringValue) : typeof(Object);
            //object reference value
            var objectReference = property.FindPropertyRelative("objectReference");
            if (objectReference.objectReferenceValue)//is interface activated?
            {
                var comp = target as Component;
                var refMan = comp.GetComponent<ReferenceValueManager>();
                var serObj = new SerializedObject(objectReference.objectReferenceValue);
                var t = serObj.FindProperty("type");
                var value = serObj.FindProperty("value");
                if (refMan)
                {
                    if (!(target is ReferenceValueManager))
                    {
                        var tValue = t.stringValue != "" ? System.Type.GetType(t.stringValue) : typeof(Object);
                        value.objectReferenceValue = EditorGUI.ObjectField(position, value.objectReferenceValue, tValue, true);
                    }
                    else
                    {
                        EditorGUI.PropertyField(position, t);

                        position.y += lineHeight;
                        value.objectReferenceValue = EditorGUI.ObjectField(position, value.objectReferenceValue, System.Type.GetType(t.stringValue), true);
                    }
                        
                }
                else
                    value.objectReferenceValue = EditorGUI.ObjectField(position, value.objectReferenceValue, typeValue, true);
                serObj.ApplyModifiedProperties();

            }
        }

    }

}

