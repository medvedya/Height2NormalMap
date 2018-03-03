using UnityEngine;
namespace Height2NormalMap
{
    public enum HeightAndNormalMapСombinerMode { RGB_normal, RG_normal_B_height, ALL_height }
    [System.Serializable]
    public struct HeightAndNormalMapСombiner
    {
        public HeightAndNormalMapСombinerMode mode;
        public void Do(Texture normalMap, Texture heightMap, RenderTexture destination)
        {
            switch (mode)
            {
                case HeightAndNormalMapСombinerMode.RGB_normal:
                    Graphics.Blit(normalMap, destination);
                    break;
                case HeightAndNormalMapСombinerMode.RG_normal_B_height:
                    PutHeightMapToBlueChanel(normalMap, heightMap, destination);
                    break;
                case HeightAndNormalMapСombinerMode.ALL_height:
                    Graphics.Blit(heightMap, destination);
                    break;
                default:
                    break;
            }
        }
        private static Material putHeightMapToBlueChanelMaterial;
        private static void PutHeightMapToBlueChanel(Texture current, Texture heightMap, RenderTexture destination)
        {
            if (putHeightMapToBlueChanelMaterial == null)
            {
                putHeightMapToBlueChanelMaterial = new Material(Shader.Find("Hidden/NMG/PutHeightMapToBlueChanel"));
            }
            putHeightMapToBlueChanelMaterial.SetTexture("_HeightMap", heightMap);
            Graphics.Blit(current, destination, putHeightMapToBlueChanelMaterial);
        }
    }
}
