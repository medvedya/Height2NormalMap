#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Height2NormalMap
{
    [ExecuteInEditMode]
    public class Height2NormalMapPreview : MonoBehaviour
    {

        [SerializeField] Height2NormalMapAsset heightMap2NormalMapAsset;

        [SerializeField] string normalMapShaderAttribute = "_BumpMap";
        [SerializeField] bool toAlphaGreenNormalMapConversion = false;
        MaterialPropertyBlock mpb;

        RenderTexture rt;
        RenderTexture rtGAFormat;
        [SerializeField] TextureWrapMode wrapMode;
        NormalMapFileGenerator oldProps;
        private static Material RGB2GAMat;
        void OnWillRenderObject()
        {
            if (heightMap2NormalMapAsset == null ||  oldProps.Equals(heightMap2NormalMapAsset.editGenerator)) return;
            oldProps = heightMap2NormalMapAsset.editGenerator;
            UpdateMap();

        }
        public void UpdateMapIfMine(Height2NormalMapAsset asset)
        {
            if (asset == heightMap2NormalMapAsset)
            {
                UpdateMap();
            }
        }
        private void Awake()
        {
            UpdateMap();
        }
        void OnValidate()
        {
            UpdateMap();
        }
        private void Update()
        {
            if (rt == null)
            {
                UpdateMap();
            }
        }
        public void UpdateMap()
        {
            if (mpb == null)
            {
                mpb = new MaterialPropertyBlock();
            }
            mpb.Clear();
            if (rt != null)
            {
                RenderTexture.ReleaseTemporary(rt);
            }
            if (rtGAFormat != null)
            {
                RenderTexture.ReleaseTemporary(rtGAFormat);

            }
            if (heightMap2NormalMapAsset != null)
            {
                RenderTextureDescriptor rd = new RenderTextureDescriptor(heightMap2NormalMapAsset.editGenerator.DestinationWidth, heightMap2NormalMapAsset.editGenerator.DestinationHeight)
                {
                    sRGB = false
                };
                rt = RenderTexture.GetTemporary(rd);
                rt.wrapMode = wrapMode;
                heightMap2NormalMapAsset.GenerateFromEditorGenerator(rt);
                var usedTexture = rt;
                if (toAlphaGreenNormalMapConversion)
                {
                    rtGAFormat = RenderTexture.GetTemporary(rd);
                    if (RGB2GAMat == null)
                    {
                        RGB2GAMat = new Material(Shader.Find("Hidden/NMG/RGB2GANormalMap"));
                    }
                    Graphics.Blit(rt, rtGAFormat, RGB2GAMat);
                    usedTexture = rtGAFormat;
                }
                mpb.SetTexture(normalMapShaderAttribute, usedTexture);
            }
            var r = GetComponent<Renderer>();
            if (r != null)
            {
                r.SetPropertyBlock(mpb);
            }

        }
    }
}
#endif