using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DigitomUtilities
{
    [System.Serializable]
    public class CallMethod
    {
        public Object targetObject;
        public int componentInd;
        public int methodInd;
        public List<ReferenceValueProperty> parameters = new List<ReferenceValueProperty>();
        public ReferenceValueProperty methodReturnValue;

        public void Invoke()
        {
            if (!targetObject) return;

            var comp = targetObject as Component;
            //component list
            Component[] components = comp.GetComponents<Component>();
            var types = new System.Type[components.Length + 1];
            types[0] = typeof(GameObject);
            for (int i = 0; i < components.Length; i++)
            {
                types[i + 1] = components[i].GetType();
            }

            //reflection to get chosen method of chosen component
            var type = types[componentInd];
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .OrderBy(x => x.Name).ToArray();
            var chosenMethod = methods[methodInd];
            var paras = parameters?.Select(x => x.GetObjectValue()).ToArray();
            //select gameobject itself or components
            object obj = components[0].gameObject;
            if (componentInd > 0)
                obj = components[componentInd - 1];

            //invoke method and return value
            var called = chosenMethod.Invoke(obj, paras);
            if (called != null)
                methodReturnValue.SetObjectValue(called);
        }
    }
}

