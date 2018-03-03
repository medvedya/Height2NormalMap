using UnityEngine;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct BlurNormalMapFilter : IFilter
    {
        public GaussianBlurFilter preBlur;
        public SobelNormalMapFilter normalMap;
        public GaussianBlurFilter postBlur;

        public void Apply(Texture source, RenderTexture destination)
        {
            int w = source.width, h = source.height;
            var rd = new RenderTextureDescriptor(w, h)
            {
                sRGB = false
            };
            var blurRT = RenderTexture.GetTemporary(rd);
            preBlur.Apply(source, blurRT);
            var tmpNormalRT = RenderTexture.GetTemporary(rd);
            normalMap.Apply(blurRT, tmpNormalRT);
            RenderTexture.ReleaseTemporary(blurRT);
            var blurRT2 = RenderTexture.GetTemporary(rd);
            postBlur.Apply(tmpNormalRT, blurRT2);
            RenderTexture.ReleaseTemporary(tmpNormalRT);
            (new NormalizeNormalMapFilter()).Apply(blurRT2, destination);
            RenderTexture.ReleaseTemporary(blurRT2);
        }
    }
}
