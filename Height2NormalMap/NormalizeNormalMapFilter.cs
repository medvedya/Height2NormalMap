using UnityEngine;
namespace Height2NormalMap
{
    public struct NormalizeNormalMapFilter : IFilter
    {
        private static Material normolizeNormalMapMaterial;
        public void Apply(Texture source, RenderTexture destination)
        {
            if (normolizeNormalMapMaterial == null)
            {
                normolizeNormalMapMaterial = new Material(Shader.Find("Hidden/NMG/NormalizeNormalMap"));
            }
            Graphics.Blit(source, destination, normolizeNormalMapMaterial);
        }
    }
}
