// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ButterflyShader"
{
	Properties
	{
		_Speed("Speed", float) = 1.0
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader
		{
			Pass
			{
				Cull off
				Blend SrcAlpha One
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#define PI 3.14159

				#include "UnityCG.cginc"

				uniform float _Speed;
				uniform sampler2D _MainTex;
				uniform fixed4 _Color;

				struct appdata
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					fixed4 position : SV_POSITION;
				};
			
				v2f vert (appdata v)
				{
					float x = (v.texcoord.x - 0.5);
					v.vertex.y += ((x * x) + 0.1) * (sin(_Time.z * _Speed) + 0.5) * 10;

					v2f o;
					o.position = UnityObjectToClipPos(v.vertex);
					o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
					return o;
				}
			
				fixed4 frag (v2f i) : COLOR
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
					col.rgb *= _Color.rgb;
					col.a  *= _Color.a;
					return col;
				}
				ENDCG
		}
	}
}
