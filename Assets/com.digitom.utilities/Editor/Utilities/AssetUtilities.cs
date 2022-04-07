using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitomUtilities
{
	public static class AssetUtilities
	{
		// Creates a new ScriptableObject via the default Save File panel
		public static ScriptableObject CreateAssetWithSavePrompt(System.Type type, string path)
		{
			path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", path);
			if (path == "") return null;
			ScriptableObject asset = ScriptableObject.CreateInstance(type);
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
			EditorGUIUtility.PingObject(asset);
			return asset;
		}
	}
}


