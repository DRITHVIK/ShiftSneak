using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public static class GameObjectExtensions
    {
        public static GameObject FindClosestByTag(Vector3 origin, string tag)
        {
            var objs = GameObject.FindGameObjectsWithTag(tag);
            float dist = Mathf.Infinity;
            GameObject closest = null;
            for (int i = 0; i < objs.Length; i++)
            {
                var nextDist = Vector3.Distance(origin, objs[i].transform.position);
                if (nextDist < dist)
                {
                    dist = nextDist;
                    closest = objs[i];
                }
            }
            return closest;
        }

        public static bool HasComponent<T>(this GameObject go)
            where T : Component
        {
            return go.GetComponent<T>() != null;
        }
    }
}


