#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct NormalMapFileGenerator
    {
        public NormalMapGenerator generator;
        public Texture2D destinationMap;
        public int customDestinationWidth;
        public int customDestinationHeight;
        public bool HasMap(Texture tex)
        {
            return generator.baseHeightMap == tex || generator.overideHeightMap == tex || generator.overideNormalMap == tex;
        }
        public int DestinationWidth
        {
            get
            {
                if (customDestinationWidth > 0)
                    return customDestinationWidth;

                int a = generator.baseHeightMap != null ? generator.baseHeightMap.width : 0;
                int b = generator.overideHeightMap != null ? generator.overideHeightMap.width : 0;
                int c = generator.overideNormalMap != null ? generator.overideNormalMap.width : 0;
                return Mathf.Max(a, b, c);
            }
        }

        public int DestinationHeight
        {
            get
            {
                if (customDestinationWidth > 0)
                    return customDestinationWidth;

                int a = generator.baseHeightMap != null ? generator.baseHeightMap.height : 0;
                int b = generator.overideHeightMap != null ? generator.overideHeightMap.height : 0;
                int c = generator.overideNormalMap != null ? generator.overideNormalMap.height : 0;
                return Mathf.Max(a, b, c);
            }
        }
        public void UpdateDestMap()
        {
            if (destinationMap == null)
                return;
            var path = AssetDatabase.GetAssetPath(destinationMap);
            CreateTexture(path);
            Debug.Log("Created texture: " + path);

        }
        public Texture2D CreateTexture(string targetPath)
        {
            RenderTextureDescriptor rd = new RenderTextureDescriptor(DestinationWidth, DestinationHeight)
            {
                sRGB = false
            };

            var compliteRT = RenderTexture.GetTemporary(rd);
            generator.Apply(compliteRT);
            Texture2D saveTex = new Texture2D(DestinationWidth, DestinationHeight, TextureFormat.RGBA32, false, true);
            RenderTexture.active = compliteRT;
            saveTex.ReadPixels(new Rect(0, 0, compliteRT.width, compliteRT.height), 0, 0);
            saveTex.Apply();
            RenderTexture.ReleaseTemporary(compliteRT);
            File.WriteAllBytes(targetPath, ImageConversion.EncodeToPNG(saveTex));
            AssetDatabase.ImportAsset(targetPath);
            Object.DestroyImmediate(saveTex);
            return AssetDatabase.LoadMainAssetAtPath(targetPath) as Texture2D;
        }
    }
}
#endif
