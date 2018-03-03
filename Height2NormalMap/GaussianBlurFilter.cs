using UnityEngine;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct GaussianBlurFilter : IFilter
    {
        [Range(0, 50)]
        public int iteration;
        [Range(0, 2)]
        public float sampleFactor;
        private static Material blurMaterial;
        public void Apply(Texture source, RenderTexture destination)
        {

            if (blurMaterial == null)
            {
                blurMaterial = new Material(Shader.Find("Hidden/NMG/GaussianBlur"));
            }
            RenderTexture rt1, rt2;
            int w = Mathf.RoundToInt(((float)source.width) * sampleFactor);
            int h = Mathf.RoundToInt(((float)source.height) * sampleFactor);
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            var rd = new RenderTextureDescriptor(w, h)
            {
                sRGB = false
            };

            rt1 = RenderTexture.GetTemporary(rd);
            rt2 = RenderTexture.GetTemporary(rd);
            Graphics.Blit(source, rt1);

            for (var i = 0; i < iteration; i++)
            {
                Graphics.Blit(rt1, rt2, blurMaterial, 1);
                Graphics.Blit(rt2, rt1, blurMaterial, 2);
            }
            Graphics.Blit(rt1, destination);

            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }
    }
}
