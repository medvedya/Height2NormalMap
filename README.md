# Height2NormalMap
This tool create a normal map from height map.
It use Gaussian blur and Sobel operator for generation.
Generation has four steps:
* first blur
* generate normal map by Sobel operator
* second blur
* normalizing normal map.

It also can put height map to blue channel of texture.

It has Height2NormalMapPreview component to preview bump effect in Scene View when you configure generator options.
It uses AssetPostprocessor to update normal map texture automatically when height map changing. 
It was tested on Unity 2017.

Demonstration https://youtu.be/IC_reA68L1w



