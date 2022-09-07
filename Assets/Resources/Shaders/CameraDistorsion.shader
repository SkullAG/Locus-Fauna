Shader "Custom/CameraDistorsion"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DistTex ("Distorsion texture", 2D) = "black" {}
		_MinDistDis ("Min distorsion distance", float) = 20
		_MaxDistDis ("Max distorsion distance", float) = 100
		_DistStrength ("Distorsion strength", Range(0,1)) = 1
		_Speed ("Speed (cameras/s)", float) = 0.01
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 depth : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _DistTex;
			float4 _DistTex_ST;

			float _MinDistDis;
			float _MaxDistDis;
			float _Speed;

			half _DistStrength;

			sampler2D _CameraDepthTexture;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				UNITY_TRANSFER_DEPTH(o.depth);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float depthValue = LinearEyeDepth (tex2D (_CameraDepthTexture, i.uv));
				//float depthValue = LinearEyeDepth (tex2D (_CameraDepthTexture, i.uv));
				//float depthValue = tex2D (_CameraDepthTexture, i.uv);

				depthValue = clamp((depthValue - _MinDistDis) / _MaxDistDis, 0, 1);

				//float2 distortuv = float2(_Speed * _Time.y, 0);

				fixed4 distort = tex2D(_DistTex, i.uv * _DistTex_ST + float2(_Speed * _Time.y, 0)) * tex2D(_DistTex, i.uv * _DistTex_ST - float2(0 , _Speed * _Time.y));

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + (distort * _DistStrength * depthValue));
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
