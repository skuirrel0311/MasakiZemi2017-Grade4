Shader "Custom/CustomToon"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (0,0,0,0)
		_OutlineWidth ("Outline Width", Range(.0001, 0.03)) = .005
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry" }
		LOD 100

		Pass{
			Cull Front

			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			float4 _OutlineColor;
			float _OutlineWidth;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				float distance = UnityObjectToViewPos(v.vertex).z;
				v.vertex.xyz += v.normal * -distance * _OutlineWidth;

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				return _OutlineColor;
			}
			ENDCG
		}

		Pass
		{
			Cull Back

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
