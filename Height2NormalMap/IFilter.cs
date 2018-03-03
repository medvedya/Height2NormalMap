using UnityEngine;
namespace Height2NormalMap
{
    public interface IFilter
    {
        void Apply(Texture source, RenderTexture destination);
    }
}
