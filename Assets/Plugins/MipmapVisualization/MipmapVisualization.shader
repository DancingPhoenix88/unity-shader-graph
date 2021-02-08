// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mipmap Visualization" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		uniform sampler2D _MainTex;
        struct v2f
        {
        	float4 pos : POSITION;
            float2 tex : TEXCOORD0;
        };
        v2f vert (float4 pos : POSITION, float2 tex : TEXCOORD0)
        {
        	v2f o;
            o.pos = UnityObjectToClipPos(pos);
            o.tex = tex;
            return o;
       	}
       	float4 frag (v2f i) : COLOR
       	{
			return tex2D(_MainTex, i.tex);
       	}
	ENDCG
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
	}
	FallBack Off
}