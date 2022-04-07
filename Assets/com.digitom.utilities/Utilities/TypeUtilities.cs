using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace DigitomUtilities
{
    public static class TypeUtilities
    {
        public static bool IsList(this object _object)
        {
            return true;
        }

        static IEnumerable<Type> OrderedQuery(this IEnumerable<Type> _query, OrderType _orderType)
        {
            if (_orderType == OrderType.None) return _query;

            if (_orderType == OrderType.Ascending)
                return _query.OrderBy(x => x.Name);
            else
                return _query.OrderByDescending(x => x.Name);
        }

        public static Type[] GetAllSubclasses(this Type _type, OrderType _orderType = OrderType.None)
        {
            var query = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => (_type.IsAssignableFrom(x) || x.IsSubclassOf(_type)
            || (x.BaseType != null ? (x.BaseType.IsGenericType && _type.IsGenericType ? x.BaseType.GetGenericTypeDefinition() == _type.GetGenericTypeDefinition() : false) : false))
            && !x.IsInterface && !x.IsAbstract && x != _type);

            return query.OrderedQuery(_orderType).ToArray();
        }

        public static string[] GetAllSubclassNames(this Type _type, OrderType _orderType = OrderType.None)
        {
            return _type.GetAllSubclasses(_orderType).Select(x => x.Name).ToArray();
        }

        public static Type[] GetAllCommonValueTypes(OrderType _orderType = OrderType.None)
        {
            var query = new Type[] 
            {
                typeof(bool),
                typeof(Color),
                typeof(float),
                typeof(int),
                typeof(string),
                typeof(Vector2),
                typeof(Vector3),
            };

            return query.OrderedQuery(_orderType).ToArray();
        }

        public static string[] GetAllCommonValueTypesNames(OrderType _orderType = OrderType.None)
        {
            return GetAllCommonValueTypes(_orderType)
                .Select(x => x.Name)
                .ToArray();
        }

        public static Type[] GetDomainAssemblyTypes(OrderType _orderType = OrderType.None)
        {
            var query = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsGenericType && !x.IsArray && !x.IsEnum);
            return query.OrderedQuery(_orderType).ToArray();
        }

        public static string[] GetDomainAssemblyTypesNames(OrderType _orderType = OrderType.None)
        {
            return GetDomainAssemblyTypes(_orderType)
                .Select(x => x.Name)
                .ToArray();
        }
    }
}


