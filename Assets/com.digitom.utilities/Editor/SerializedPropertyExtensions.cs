using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace DigitomUtilities
{
    public static class SerializedPropertyExtensions
    {

        public static void SetPropertyObjectValue(this SerializedProperty _property, object _value)
        {
            object obj = _property.serializedObject.targetObject;
            var path = _property.propertyPath;
            ReflectionUtils.SetNestedObjectValue(obj, path, _value);
        }

        public static object GetPropertyObjectValue(this SerializedProperty _property)
        {
            object obj = _property.serializedObject.targetObject;
            var path = _property.propertyPath;
            return ReflectionUtils.GetNestedObjectValue(obj, path);
        }

        public static System.Type GetPropertySystemType(this SerializedProperty _property)
        {
            object obj = _property.serializedObject.targetObject;
            var type = obj.GetType();
            var path = _property.propertyPath;
            var info = GetPropertyFieldInfo(obj, path);
            if (info != null) return info.FieldType;

            var pathSplit = path.Replace("Array.", "").Split('.');

            if (pathSplit.Length < 2)
            {
                Debug.LogError("Single path is not being serialized. Make sure field can be serialized or public!");
                return null;
            }

            for (int i = 0; i < pathSplit.Length; i++)
            {
                if (obj == null) continue;
                path = pathSplit[i];
                if (path.Contains("["))
                {
                    if (type.IsArray)
                        type = type.GetElementType();
                    else
                        type = type.GetGenericArguments().Single();

                    var index = System.Convert.ToInt32(new string(path.Where(c => char.IsDigit(c)).ToArray()));
                    var col = ((IEnumerable)obj).Cast<object>();
                    if (col != null)
                    {
                        if (index < col.Count())
                            obj = col.ElementAt(index);
                    }

                }
                else if (obj != null)
                {
                    info = GetPropertyFieldInfo(obj, path);
                    if (info != null)
                    {
                        type = info.FieldType;
                        obj = info.GetValue(obj);
                    }
                    
                }
            }
            return type;
        }

        public static FieldInfo GetPropertyFieldInfo(this SerializedProperty _property)
        {
            object obj = _property.serializedObject.targetObject;
            var type = obj.GetType();
            var path = _property.propertyPath;
            var info = type.GetField(path);
            if (info != null) return info;

            var pathSplit = path.Replace("Array.", "").Split('.');

            for (int i = 0; i < pathSplit.Length; i++)
            {
                path = pathSplit[i];
                if (path.Contains("["))
                {
                    if (type.IsArray)
                        type = type.GetElementType();
                    else
                        type = type.GetGenericArguments().Single();

                    var index = System.Convert.ToInt32(new string(path.Where(c => char.IsDigit(c)).ToArray()));
                    var col = ((IEnumerable)obj).Cast<object>();
                    if (col != null)
                    {
                        if (index < col.Count())
                            obj = col.ElementAt(index);
                    }
                        
                }
                else if (obj != null )
                {
                    info = GetPropertyFieldInfo(obj, path);
                    type = info.FieldType;
                    obj = info.GetValue(obj);
                }
            }

            return info;
        }

        static FieldInfo GetPropertyFieldInfo(object _target, string _path, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return _target.GetType().GetField(_path, _bindings);
        }

        public static int ArrayElementIndex(this SerializedProperty _property)
        {
            var path = _property.propertyPath;
            var pathSplit = path.Replace("Array.", "").Split('.');
            var last = pathSplit[pathSplit.Length - 1];
            return System.Convert.ToInt32(new string(last.Where(c => char.IsDigit(c)).ToArray()));
        }

        public static bool IsArrayElement(this SerializedProperty _property)
        {
            var path = _property.propertyPath;
            var pathSplit = path.Replace("Array.", "").Split('.');
            return pathSplit[pathSplit.Length - 1].Contains("[");
        }

        public static bool IsTargetMainVariable(this SerializedProperty _property)
        {
            var path = _property.propertyPath;
            return !path.Contains(".");
        }
    }
}

