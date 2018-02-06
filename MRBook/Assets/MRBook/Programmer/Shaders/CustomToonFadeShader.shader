// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Custom/CustomToonFadeShader"
{
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_Height("Height", Float) = 1
	}

	SubShader{
		Pass
		{
			Zwrite On
			ColorMask 0
			Lighting OFF
		}

		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Zwrite Off
		ZTest LEqual
		LOD 200

		CGPROGRAM
#pragma surface surf ToonRamp Standard fullforwardshadows alpha

		sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = s.Alpha;
		return c;
	}


	sampler2D _MainTex;
	float4 _Color;
	float _Height;

	struct Input {
		float3 worldPos;
		float2 uv_MainTex : TEXCOORD0;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;

		o.Alpha = step(IN.worldPos.y, _Height);
	}
	ENDCG

	}

		Fallback "Diffuse"
}
