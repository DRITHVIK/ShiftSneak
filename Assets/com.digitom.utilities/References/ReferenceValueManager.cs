using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public class ReferenceValueManager : MonoBehaviour
    {
        public ReferenceBool[] boolValues;
        public ReferenceEnum[] enumValues;
        public ReferenceInt[] intValues;
        public ReferenceFloat[] floatValues;
        public ReferenceObject[] objectValues;
        public ReferenceQuaternion[] quaternionValues;
        public ReferenceString[] stringValues;
        public ReferenceVector2[] vector2Values;
        public ReferenceVector3[] vector3Values;
        public ReferenceVector4[] vector4Values;

        private void Awake()
        {
            InitializeValues();
        }

        void InitializeValues()
        {
            for (int i = 0; i < boolValues.Length; i++)
                boolValues[i].Init();
            for (int i = 0; i < enumValues.Length; i++)
                enumValues[i].Init();
            for (int i = 0; i < intValues.Length; i++)
                intValues[i].Init();
            for (int i = 0; i < floatValues.Length; i++)
                floatValues[i].Init();
            for (int i = 0; i < objectValues.Length; i++)
                objectValues[i].Init();
            for (int i = 0; i < quaternionValues.Length; i++)
                quaternionValues[i].Init();
            for (int i = 0; i < stringValues.Length; i++)
                stringValues[i].Init();
            for (int i = 0; i < vector2Values.Length; i++)
                vector2Values[i].Init();
            for (int i = 0; i < vector3Values.Length; i++)
                vector3Values[i].Init();
            for (int i = 0; i < vector4Values.Length; i++)
                vector4Values[i].Init();
        }

        public IReferenceValue[] GetValueReferences(IReferenceValueManagerVisitor _visitor)
        {
            return _visitor.GetValueReferences(this);
        }
    }
}


