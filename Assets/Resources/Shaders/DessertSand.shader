Shader "Custom/DessertSand"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Dune Texture", 2D) = "white" {}
		_MTAngle ("Angle", Range(0,360)) = 0
		_WindTex("Wind Texture", 2D) = "white" {}
		_WTAngle ("Angle", Range(0,360)) = 0
		_Gradient("Color Gradient", 2D) = "white" {}
		//_UseWorldCoordi("Use world coordinates", Boolean) = false
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_WindStrength ("Wind strength", Range(0,1)) = 1
		_WindAngle ("Wind direction", Range(0,360)) = 0

		_WindSpeed ("_Wind speed (m/s)", Float) = 1
		_DunesSpeedMult ("_Dunes speed multiplier (m/s)", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _WindTex;
		float4 _WindTex_ST;
		sampler2D _Gradient;

		float _MTAngle;
		float _WTAngle;
		float _WindAngle;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		half _WindStrength;
		fixed4 _Color;

		float _WindSpeed;
		float _DunesSpeedMult;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		fixed4 substract(fixed4 aColor, fixed4 bColor, fixed amount)
		{
			return aColor.r - clamp(bColor * amount, 0, 1);
		}

		float2 rotAng(float2 vec, float ang)
		{
			float sinVal = sin(radians(ang));
			float cosVal = cos(radians(ang));

			float2 tvec = vec;
			
			vec.x = (cosVal * tvec.x) - (sinVal * tvec.y);
			vec.y = (sinVal * tvec.x) + (cosVal * tvec.y);
			return vec;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			float4 factor = tex2D (_MainTex, rotAng(IN.worldPos.xz + rotAng(float2(_DunesSpeedMult * _WindSpeed, 0) * _Time.y, _WindAngle), _MTAngle) * _MainTex_ST);
			factor = substract(factor, _WindStrength, tex2D (_WindTex, rotAng(IN.worldPos.xz + rotAng(float2(_WindSpeed, 0) * _Time.y, _WindAngle), _WTAngle) * _WindTex_ST));

			factor = clamp(factor, 0, 1);

			fixed4 c = tex2D (_Gradient, pow(factor, 2)) * _Color;

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
