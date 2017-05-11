Shader "Custom/SpriteShadow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader {
		Tags
		{
			"Queue"="Geometry"
			"RenderType"="TransparentCutout"
			"CanUseSpriteAtlas"="False"
		}
		LOD 200

		Cull Off
		//zTest Always
		zWrite On

		CGPROGRAM
		// Lambert lighting model, and enable shadows on all light types
		#pragma surface surf Lambert addshadow fullforwardshadows
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _Cutoff;

		static const float PI = 3.14159;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			clip(o.Alpha - _Cutoff);
		}

		ENDCG
	}
	FallBack "Diffuse"
}
