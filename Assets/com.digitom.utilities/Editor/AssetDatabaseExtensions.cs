using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace DigitomUtilities
{
    public static class AssetDatabaseExtensions
    {
        public static string GetPathFolder(string _path)
        {
            DirectoryInfo parentDir = Directory.GetParent(_path);
            _path = parentDir.FullName;
            _path = _path.Substring(_path.IndexOf("Assets"));
            _path = _path.Replace("\\", "/");

            return _path;
        }

        public static string GetObjectPathFolder(Object _obj)
        {
            var path = AssetDatabase.GetAssetPath(_obj);
            return GetPathFolder(path);
        }
    }
}



