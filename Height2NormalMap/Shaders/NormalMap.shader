Shader "Hidden/NMG/NormalMap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Factor("Factor", Vector) = (1,1,1,0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float4 _Factor;


			float3 height2normal_sobel(float3x3 c)
			{      
				float3x3 x = float3x3(   1.0, 0.0, -1.0,
												2.0, 0.0, -2.0,
												1.0, 0.0, -1.0  );
 
				float3x3 y = float3x3(   1.0,  2.0,  1.0,
												0.0,  0.0,  0.0,
											   -1.0, -2.0, -1.0 );
   
				x = x * c;
				y = y * c;
 
				float cx =  x[0][0] +x[0][2]
							   +x[1][0] +x[1][2]
							   +x[2][0] +x[2][2];
   
				float cy =  y[0][0] +y[0][1] +y[0][2]
							  +y[2][0] +y[2][1] +y[2][2];
               
				float cz =  sqrt(1-(cx*cx+cy*cy));
   
				return float3(cx, cy, cz);
			}

			float3x3 img3x3(sampler2D color_map, float2 tc, float2 d)
			{
				float3x3 c;    
				float4 col; 
				col = tex2D(color_map,tc + float2(-d.x,-d.y));
				c[0][0] = (col.r + col.g + col.b) * col.a  / 3;
				col = tex2D(color_map,tc + float2( 0,-d.y));
				c[0][1] = (col.r + col.g + col.b)* col.a  /3;
				col = tex2D(color_map,tc + float2( d.x,-d.y));
				c[0][2] = (col.r + col.g + col.b)* col.a  /3; 
       
				col = tex2D(color_map,tc + float2(-d.x, 0));
				c[1][0] = (col.r + col.g + col.b)* col.a  /3;
				col = tex2D(color_map,tc                );
				c[1][1] = (col.r + col.g + col.b) * col.a /3;
				col = tex2D(color_map,tc + float2( d.x, 0));
				c[1][2] = (col.r + col.g + col.b) * col.a /3;
       
				col = tex2D(color_map,tc + float2(-d.x, d.y));
				c[2][0] = (col.r + col.g + col.b) * col.a /3;
				col = tex2D(color_map,tc + float2( 0, d.y));
				c[2][1] = (col.r + col.g + col.b) * col.a /3;
				col = tex2D(color_map,tc + float2( d.x, d.y));
				c[2][2] = (col.r + col.g + col.b)  * col.a/3;
   
				return c;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//return fixed4(1,0,0,1);
				
				float3 normal = height2normal_sobel(img3x3(_MainTex, i.uv, _MainTex_TexelSize.xy));
				normal = normalize(normal * _Factor.xyz);
				return fixed4(normal.x * 0.5 + 0.5, normal.y * 0.5 + 0.5, normal.z * 0.5 + 0.5,1);
			}
			ENDCG
		}
	}
}
