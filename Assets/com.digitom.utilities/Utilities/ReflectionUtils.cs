using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System;

namespace DigitomUtilities
{
    public static class ReflectionUtils
    {

        public static IEnumerable<T> GetNestedCollection<T>(object _startTarget, string _path)
        {
            var obj = GetNestedObjectValue(_startTarget, _path);
            return ((IEnumerable)obj).Cast<T>();
        }

        public static T GetNestedValue<T>(object _startTarget, string _path)
        {
            var obj = GetNestedObjectValue(_startTarget, _path);
            if (obj != null)
                return TryConvert<T>(obj);
            else
                return default;
        }

        static T TryConvert<T>(object _obj)
        {
            if (_obj is T)
                return (T)_obj;
            else
            {
                Debug.LogError("Cannot convert " + _obj + " to " + typeof(T));
                return default;
            }
                       
        }

        public static void SetNestedObjectValue(object _startTarget, string _path, object _value)
        {
            object obj = _startTarget;
            var path = _path;
            var pathSplit = path.Replace("Array.", "").Split('.');
            for (int i = 0; i < pathSplit.Length; i++)
            {
                path = pathSplit[i];
                if (path.Contains("["))
                {
                    var index = System.Convert.ToInt32(new string(path.Where(c => char.IsDigit(c)).ToArray()));

                    var col = ((IEnumerable)obj).Cast<object>();
                    if (i < pathSplit.Length - 1)
                        obj = col.ElementAt(index);
                    else
                        col.ToArray()[index] = _value;
                }
                else
                {
                    if (i < pathSplit.Length - 1)
                        obj = GetObjectValue(obj, path);
                    else
                        SetObjectValue(obj, path, _value);
                }
            }


        }

        public static object GetNestedObjectValue(object _startTarget, string _path)
        {
            return FindNestedObject(_startTarget, _path);
        }

        static object FindNestedObject(object _target, string _path)
        {
            var obj = _target;
            var path = _path;
            var pathSplit = path.Replace("Array.", "").Split('.');
            for (int i = 0; i < pathSplit.Length; i++)
            {
                if (obj == null) continue;
                path = pathSplit[i];
                if (path.Contains("["))
                {
                    var index = System.Convert.ToInt32(new string(path.Where(c => char.IsDigit(c)).ToArray()));
                    var col = ((IEnumerable)obj).Cast<object>();
                    if (col != null)
                    {
                        if (index < col.Count())
                            obj = col.ElementAt(index);
                    }
                }
                else
                {
                    obj = GetObjectValue(obj, path);
                }
            }

            return obj;
        }

        public static void SetObjectValue(object _target, string _query, object _value, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            FieldInfo fInfo = GetField(_target, _query, _bindings);
            if (fInfo != null) { fInfo.SetValue(_target, _value); }

            PropertyInfo pInfo = GetProperty(_target, _query, _bindings);
            if (pInfo != null) pInfo.SetValue(_target, _value);

            MethodInfo mInfo = GetMethod(_target, _query, _bindings);
            if (mInfo != null) mInfo.Invoke(_target, new object[] { _value });
        }

        public static object GetObjectValue(object _target, string _query, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            FieldInfo fInfo = GetField(_target, _query, _bindings);
            if (fInfo != null) { return fInfo.GetValue(_target); }

            PropertyInfo pInfo = GetProperty(_target, _query, _bindings);
            if (pInfo != null) return pInfo.GetValue(_target);

            MethodInfo mInfo = GetMethod(_target, _query, _bindings);
            if (mInfo != null) return mInfo.Invoke(_target, null);

            return default;
        }

        public static FieldInfo GetField(object _target, string _path, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return _target.GetType().GetField(_path, _bindings);
        }

        public static MethodInfo GetMethod(object _target, string _path, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return _target.GetType().GetMethod(_path, _bindings);
        }

        public static MethodInfo[] GetMethods(object _target, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return _target.GetType().GetMethods(_bindings);
        }

        public static PropertyInfo GetProperty(object _target, string _path, BindingFlags _bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return _target.GetType().GetProperty(_path, _bindings);
        }
    }
}


