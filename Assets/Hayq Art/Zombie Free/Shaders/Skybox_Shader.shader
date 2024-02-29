
Shader "Hayq Art / Clay City / Skybox_Day"
{
	Properties
	{

	}

	SubShader
	{


		Tags { "RenderType" = "Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0



		Pass
		{
			Name "Unlit"
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM



			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			struct Gradient
			{
				int type;
				int colorsLength;
				int alphasLength;
				float4 colors[8];
				float2 alphas[8];
			};

			Gradient NewGradient(int type, int colorsLength, int alphasLength,
			float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
			float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
			{
				Gradient g;
				g.type = type;
				g.colorsLength = colorsLength;
				g.alphasLength = alphasLength;
				g.colors[0] = colors0;
				g.colors[1] = colors1;
				g.colors[2] = colors2;
				g.colors[3] = colors3;
				g.colors[4] = colors4;
				g.colors[5] = colors5;
				g.colors[6] = colors6;
				g.colors[7] = colors7;
				g.alphas[0] = alphas0;
				g.alphas[1] = alphas1;
				g.alphas[2] = alphas2;
				g.alphas[3] = alphas3;
				g.alphas[4] = alphas4;
				g.alphas[5] = alphas5;
				g.alphas[6] = alphas6;
				g.alphas[7] = alphas7;
				return g;
			}

			float4 SampleGradient(Gradient gradient, float time)
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c - 1].w) / (0.00001 + (gradient.colors[c].w - gradient.colors[c - 1].w)) * step(c, (float)gradient.colorsLength - 1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a - 1].y) / (0.00001 + (gradient.alphas[a].y - gradient.alphas[a - 1].y)) * step(a, (float)gradient.alphasLength - 1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}



			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;

				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				Gradient gradient27 = NewGradient(0, 2, 2, float4(0.7764706, 1, 0.9977165, 0), float4(0.1745283, 0.9123334, 1, 0.03207446), 0, 0, 0, 0, 0, 0, float2(1, 0), float2(1, 1), 0, 0, 0, 0, 0, 0);
				float2 texCoord43 = i.ase_texcoord1.xy * float2(1,0.1) + float2(0,0);


				finalColor = SampleGradient(gradient27, texCoord43.y);
				return finalColor;
			}
			ENDCG
		}
	}


}
