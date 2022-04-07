using Bolt;
using Ludiq;
using UnityEngine;
using DigitomUtilities;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Digitom.VSPhysics.RunTime
{
    public class PhysicsOverlapCapsule : PhysicsOverlapBase
    {
        [DoNotSerialize] public ValueInput rad;
        [DoNotSerialize] public ValueInput height;

        private float _rad;
        private float _height;

        protected override Collider[] DetectColliders(Flow flow) 
        {
            var matrix = Matrix4x4.TRS(_pos, _rot, Vector3.one);
            var point1 = matrix.MultiplyPoint(new Vector3(0, (_height / 2) - _rad, 0));
            var point2 = matrix.MultiplyPoint(new Vector3(0, -(_height / 2) + _rad, 0));
            return Physics.OverlapCapsule(point1, point2, _rad, _mask);
        }
        protected override void DrawGizmos(Flow flow) => GizmoUtilities.DrawWireCapsule(_pos, _rot, _rad, _height, _debugColor);

        protected override void DefineInputs()
        {
            rad = ValueInput(nameof(rad), (float)default);
            height = ValueInput(nameof(height), (float)default);
        }

        protected override void GetInputValues(Flow flow)
        {
            _rad = flow.GetValue<float>(rad);
            var h = flow.GetValue<float>(height);
            _height = Mathf.Clamp(h, _rad * 2, Mathf.Infinity);
            flow.SetValue(height, _height);
        }
    }
}


