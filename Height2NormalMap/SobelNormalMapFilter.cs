using UnityEngine;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct SobelNormalMapFilter : IFilter
    {
        [Range(0, 1)]
        public float bumpEffect;
        private static Material normalMaplMaterial;

        public void Apply(Texture source, RenderTexture destination)
        {
            if (normalMaplMaterial == null)
            {
                normalMaplMaterial = new Material(Shader.Find("Hidden/NMG/NormalMap"));
            }
            float v = bumpEffect * 2f - 1f;
            float z = 1f - v;
            float xy = 1f + v;
            normalMaplMaterial.SetVector("_Factor", new Vector4(xy, xy, z, 1));


            Graphics.Blit(source, destination, normalMaplMaterial);
        }
    }
}
