﻿// Originally By James O'Hare, from his Gist: https://gist.github.com/Farfarer/5664694
// This takes in the cubemap generated by your cubemap camera and feeds back out an equirectangular image.
// Create a new material and give it this shader. Then give that material to the "cubemapToEquirectangularMateral" property of the dynamicAmbient.js script in this gist.
// You could probably abstract this to C#/JS code and feed it in a pre-baked cubemap to sample and then spit out an equirectangular map if you don't have render textures.
Shader "Hidden/CubemapToEquirectangular" 
{
	Properties 
	{
		_MainTex ("Cubemap (RGB)", CUBE) = "" {}
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
		
	uniform samplerCUBE _MainTex;
	uniform float _FlipX;

	#define PI 3.141592653589793
	#define HALFPI 1.57079632679

	v2f vert( appdata_img v )
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		#if STEREOPACK_TOP
		o.pos.y = (o.pos.y / 2.0) - 0.5;
		#elif STEREOPACK_BOTTOM
		o.pos.y = (o.pos.y / 2.0) + 0.5;
		#elif STEREOPACK_LEFT
		o.pos.x = (o.pos.x / 2.0) - 0.5;
		#elif STEREOPACK_RIGHT
		o.pos.x = (o.pos.x / 2.0) + 0.5;
		#endif

		float2 uv = v.texcoord.xy;
		if (_FlipX > 0.5)
		{
			uv.x = 1.0 - uv.x;
		}
		uv = uv * 2.0 - float2(0.5, 1.0);
		uv *= float2(PI, HALFPI);
		o.uv = uv;
		return o;
	}
		
	fixed4 frag(v2f i) : COLOR 
	{
		float cosy = cos(i.uv.y);
		float3 normal = float3(0.0, 0.0, 0.0);
		normal.x = cos(i.uv.x) * cosy;
		normal.y = i.uv.y;
		normal.z = cos(i.uv.x - HALFPI) * cosy;
		return texCUBE(_MainTex, normalize(normal));
	}
	ENDCG

	Subshader 
	{
		ZTest Always
		Cull Off
		ZWrite Off
		Fog{ Mode off }

		Pass
		{
			CGPROGRAM
			#pragma multi_compile __ STEREOPACK_TOP STEREOPACK_BOTTOM STEREOPACK_LEFT STEREOPACK_RIGHT
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	Fallback Off
}