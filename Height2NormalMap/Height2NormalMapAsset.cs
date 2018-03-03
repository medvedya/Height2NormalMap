#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Height2NormalMap
{
    [CreateAssetMenu()]
    [ExecuteInEditMode]
    public class Height2NormalMapAsset : ScriptableObject
    {
        public NormalMapFileGenerator generator = new NormalMapFileGenerator()
        {
            generator = new NormalMapGenerator()
            {
                heightMap = new BlurHeightMapFilter()
                {
                    preBlur = new GaussianBlurFilter()
                    {
                        iteration = 0,
                        sampleFactor = 1
                    },
                    postBlur = new GaussianBlurFilter()
                    {
                        iteration = 0,
                        sampleFactor = 1
                    },
                    heigthMap = new HeightMapFilter()
                    {
                        factor = 1
                    }
                },
                normalMap = new BlurNormalMapFilter()
                {
                    normalMap = new SobelNormalMapFilter()
                    {
                        bumpEffect = 0.5f
                    },
                    preBlur = new GaussianBlurFilter()
                    {
                        iteration = 0,
                        sampleFactor = 1
                    },
                    postBlur = new GaussianBlurFilter()
                    {
                        iteration = 1,
                        sampleFactor = 1
                    }
                }
            }
        };

        public NormalMapFileGenerator editGenerator;
        [SerializeField]
        public bool showPreview = true;
        void Awake()
        {
            RevertEditProperty();
        }
        public void RevertEditProperty()
        {
            editGenerator = generator;
        }
        public void ApplyEditProperty()
        {
            generator = editGenerator;
            generator.UpdateDestMap();
        }
        public void GenerateFromEditorGenerator(RenderTexture destination)
        {
            editGenerator.generator.Apply(destination);
        }
        public void UpdateIfYourMap(Texture tex, Height2NormalMapPreview[] previewObjects = null)
        {
            if (editGenerator.HasMap(tex))
            {
                var a = previewObjects == null ? FindObjectsOfType<Height2NormalMapPreview>() : previewObjects;
                foreach (var item in a)
                {
                    item.UpdateMapIfMine(this);
                }
            }
            if (generator.HasMap(tex))
            {
                generator.UpdateDestMap();
            }
        }

    }
}
#endif
