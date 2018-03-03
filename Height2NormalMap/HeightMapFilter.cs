using UnityEngine;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct HeightMapFilter : IFilter
    {
        [Range(0, 2)]
        public float factor;
        Material mat;
        public void Apply(Texture source, RenderTexture destination)
        {
            if (mat == null)
            {
                mat = new Material(Shader.Find("Hidden/NMG/HeightMap"));
            }
            mat.SetFloat("_Factor", factor);
            Graphics.Blit(source, destination, mat);
        }
    }
}
