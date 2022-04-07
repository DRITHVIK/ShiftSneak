using Bolt;
using Ludiq;
using UnityEngine;
using System;

namespace Digitom.VSPhysics.RunTime
{
    public class PhysicsOverlapBox : PhysicsOverlapBase
    {
        [DoNotSerialize] public ValueInput size;

        private Vector3 _size;

        protected override Collider[] DetectColliders(Flow flow) => Physics.OverlapBox(_pos, _size / 2, _rot, _mask);
        protected override void DrawGizmos(Flow flow) => Gizmos.DrawWireCube(Vector3.zero, _size);

        protected override void DefineInputs()
        {
            size = ValueInput(nameof(size), (Vector3)default);
        }

        protected override void GetInputValues(Flow flow)
        {
            _size = flow.GetValue<Vector3>(size);
        }
    }
}


