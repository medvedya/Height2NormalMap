using UnityEngine;
namespace HeightMap2NormalMap
{
    public interface IFilter
    {
        void Apply(Texture source, RenderTexture destination);
    }
}
