using UnityEngine;
namespace Height2NormalMap
{
    [System.Serializable]
    public struct BlurHeightMapFilter : IFilter
    {
        public GaussianBlurFilter preBlur;
        public HeightMapFilter heigthMap;
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
            var tmpHeightRT = RenderTexture.GetTemporary(rd);
            heigthMap.Apply(blurRT, tmpHeightRT);
            RenderTexture.ReleaseTemporary(blurRT);
            postBlur.Apply(tmpHeightRT, destination);
            RenderTexture.ReleaseTemporary(tmpHeightRT);
        }
    }
}
