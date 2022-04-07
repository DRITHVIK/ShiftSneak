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
        private Collider[] betterCols = new Collider[5];
        protected override Collider[] DetectColliders(Flow flow) { Physics.OverlapBoxNonAlloc(_pos, _size / 2, betterCols, _rot, _mask); return betterCols; }
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


