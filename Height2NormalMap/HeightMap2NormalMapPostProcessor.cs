#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Height2NormalMap
{
    public class HeightMap2NormalMapPostProcessor : AssetPostprocessor
    {
        static List<Texture> loadedTextures = new List<Texture>();
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                var tex = AssetDatabase.LoadAssetAtPath<Texture>(str);
                if (tex != null)
                {
                    loadedTextures.Add(tex);
                }
            }
            if (loadedTextures.Count > 0)
            {
                var guids = AssetDatabase.FindAssets("t:Height2NormalMapAsset");
                Height2NormalMapPreview[] previewObjects = null;
                if (guids.Length > 0)
                {
                    previewObjects = GameObject.FindObjectsOfType<Height2NormalMapPreview>();
                }
                foreach (var item in guids)
                {
                    var obj = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(item)) as Height2NormalMapAsset;
                    foreach (var texItem in loadedTextures)
                    {
                        obj.UpdateIfYourMap(texItem, previewObjects);
                    }
                }
                loadedTextures.Clear();
            }
        }
    }
}
#endif
