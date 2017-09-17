Shader "DadFightClub/ScreenSpaceTexture"
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_ScreenTex("Screenspace Tex", 2D) = "grey" {}
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}

		Pass
		{
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma target 3.0
	
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _ScreenTex;

			struct VI
			{
				float4 pos : POSITION;
				float3 norm : NORMAL;
				float4 uv : TEXCOORD0;
			};

			struct VO
			{
				float4 pos : SV_POSITION;
				half3 uv : TEXCOORD0;
				float3 wPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
			};

			VO vert(VI vi)
			{
				VO vo;
				vo.pos = UnityObjectToClipPos(vi.pos.xyz);
				vo.uv = vi.uv;
				vo.wPos = mul(unity_ObjectToWorld, vi.pos);
				vo.screenPos = ComputeScreenPos(vi.pos);

				return vo;
			}

			fixed4 frag(VO i) : SV_Target
			{
				fixed4 tex = tex2D(_MainTex, i.uv);
				float2 screenUV = i.screenPos.xy / i.screenPos.w;
				fixed4 pixel = tex2D(_ScreenTex, screenUV);

				return pixel;
			}

			ENDCG
		}
	}
}