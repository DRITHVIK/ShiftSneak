using Bolt;
using Ludiq;
using UnityEngine;
using System;

namespace Digitom.VSPhysics.RunTime
{
    public class PhysicsOverlapSphere : PhysicsOverlapBase
    {
        [DoNotSerialize] public ValueInput rad;

        private float _rad;

        protected override Collider[] DetectColliders(Flow flow) => Physics.OverlapSphere(_pos, _rad, _mask);
        protected override void DrawGizmos(Flow flow) => Gizmos.DrawWireSphere(Vector3.zero, _rad);

        protected override void DefineInputs()
        {
            rad = ValueInput(nameof(rad), (float)default);
        }

        protected override void GetInputValues(Flow flow)
        {
            _rad = flow.GetValue<float>(rad);
        }
    }
}


