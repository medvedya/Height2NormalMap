using UnityEngine;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct NormalMapGenerator
    {
        public Texture baseHeightMap;
        public HeightAndNormalMapСombiner сombiner;
        public Texture overideNormalMap;
        public BlurNormalMapFilter normalMap;
        public Texture overideHeightMap;
        public BlurHeightMapFilter heightMap;
        public void Apply(RenderTexture destination)
        {
            RenderTextureDescriptor rd = new RenderTextureDescriptor(destination.width, destination.height)
            {
                sRGB = false
            };

            RenderTexture normalRT = RenderTexture.GetTemporary(rd);
            if (сombiner.mode != HeightAndNormalMapСombinerMode.ALL_height)
            {
                if (overideNormalMap == null && baseHeightMap != null)
                {
                    normalMap.Apply(baseHeightMap, normalRT);
                }
                if (overideNormalMap != null)
                {
                    Graphics.Blit(overideNormalMap, normalRT);
                }
            }
            RenderTexture heigtRT = RenderTexture.GetTemporary(rd);
            {
                Texture usedHeightMapSourse = (overideHeightMap != null ? overideHeightMap : baseHeightMap);
                if (usedHeightMapSourse != null)
                {
                    heightMap.Apply(usedHeightMapSourse, heigtRT);
                }
            }
            сombiner.Do(normalRT, heigtRT, destination);
            RenderTexture.ReleaseTemporary(normalRT);
            RenderTexture.ReleaseTemporary(heigtRT);
        }

    }
}
