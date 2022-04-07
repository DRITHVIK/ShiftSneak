using Bolt;
using UnityEngine;
using System;
using DigitomUtilities;
using Ludiq;

namespace Digitom.VSPhysics.RunTime
{
    [UnitCategory("Physics/Overlap")]
    public abstract class PhysicsOverlapBase : Unit
    {
        [DoNotSerialize, PortLabelHidden] public ControlInput enter;
        [DoNotSerialize, PortLabelHidden] public ControlOutput exit;
        [DoNotSerialize, Inspectable] public ValueInput mask;
        [DoNotSerialize] public ValueInput pos;
        [DoNotSerialize] public ValueInput rot;
        [DoNotSerialize] public ControlInput debug;
        [DoNotSerialize] public ValueInput debugColor;
        [DoNotSerialize] public ValueOutput isDetected;
        [DoNotSerialize] public ValueOutput detectedCols;
        [DoNotSerialize] public ValueOutput onEnterCols;
        [DoNotSerialize] public ValueOutput onExitCols;
        [DoNotSerialize] public ValueOutput onStayCols;
        [DoNotSerialize] public ValueOutput onFirstCols;
        [DoNotSerialize] public ValueOutput onEmptyCols;
        [DoNotSerialize] public ControlOutput onEnter;
        [DoNotSerialize] public ControlOutput onExit;
        [DoNotSerialize] public ControlOutput onStay;
        [DoNotSerialize] public ControlOutput onFirst;
        [DoNotSerialize] public ControlOutput onEmpty;

        protected LayerMask _mask;
        protected Vector3 _pos;
        protected Quaternion _rot;
        protected Color _debugColor;
        protected bool _isDetected;
        protected Collider[] _detected;
        protected Collider[] _onEnterCols;
        protected Collider[] _onExitCols;
        protected Collider[] _onStayCols;
        protected Collider[] _onFirstCols;
        protected Collider[] _onEmptyCols;
        protected ArrayTriggerChecker<Collider> _triggerChecker = new ArrayTriggerChecker<Collider>();

        protected override void Definition()
        {
            //control inputs
            enter = ControlInput(nameof(enter), (flow)=> 
            {
                GetBaseInputValues(flow);
                GetInputValues(flow);
                _detected = DetectColliders(flow);
                _isDetected = _detected.Length > 0;
                //trigger stuff
                _triggerChecker.CheckArray(_detected);
                if (_triggerChecker.IsTriggerEntered(out _onEnterCols))
                    flow.Invoke(onEnter);
                if (_triggerChecker.IsTriggerExited(out _onExitCols))
                    flow.Invoke(onExit);
                if (_triggerChecker.IsTriggerStayed(out _onStayCols))
                    flow.Invoke(onStay);
                if (_triggerChecker.IsTriggerFirst(out _onFirstCols))
                    flow.Invoke(onFirst);
                if (_triggerChecker.IsTriggerEmpty(out _onEmptyCols))
                    flow.Invoke(onEmpty);

                return exit;
            });

            //value inputs
            mask = ValueInput(nameof(mask), (LayerMask)default);
            pos = ValueInput(nameof(pos), (Vector3)default);
            rot = ValueInput(nameof(rot), Quaternion.identity);
            DefineInputs();

            //value outputs
            isDetected = ValueOutput(nameof(isDetected), _=> _isDetected);
            detectedCols = ValueOutput(nameof(detectedCols), _=> _detected);
            onEnterCols = ValueOutput(nameof(onEnterCols), _=> _onEnterCols);
            onExitCols = ValueOutput(nameof(onExitCols), _=> _onExitCols);
            onStayCols = ValueOutput(nameof(onStayCols), _=> _onStayCols);
            onFirstCols = ValueOutput(nameof(onFirstCols), _ => _onFirstCols);
            onEmptyCols = ValueOutput(nameof(onEmptyCols), _=> _onEmptyCols);

            debug = ControlInput(nameof(debug), (flow) =>
            {
                //Debug logic here
                if (!Application.isPlaying)
                {
                    GetBaseInputValues(flow);
                    GetInputValues(flow);
                }
                    
                Gizmos.color = _debugColor;
                Gizmos.matrix = Matrix4x4.TRS(_pos, _rot, Vector3.one);
                DrawGizmos(flow);
                return null;
            });

            exit = ControlOutput(nameof(exit));
            onEnter = ControlOutput(nameof(onEnter));
            onExit = ControlOutput(nameof(onExit));
            onStay = ControlOutput(nameof(onStay));
            onFirst = ControlOutput(nameof(onFirst));
            onEmpty = ControlOutput(nameof(onEmpty));

            //debug value inputs
            debugColor = ValueInput(nameof(debugColor), Color.white);
        }

        protected abstract void DefineInputs();
        protected virtual void GetBaseInputValues(Flow flow)
        {
            _mask = flow.GetValue<LayerMask>(mask);
            _pos = flow.GetValue<Vector3>(pos);
            _rot = rot.hasValidConnection ? flow.GetValue<Quaternion>(rot) : Quaternion.identity;
            _debugColor = flow.GetValue<Color>(debugColor);
        }
        protected abstract void GetInputValues(Flow flow);
        protected abstract Collider[] DetectColliders(Flow flow);
        protected abstract void DrawGizmos(Flow flow);

    }
}


