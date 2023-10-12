Shader "ComboBall/Transparent/FillTexture_Vertical" {
	Properties {
		_MainTex1 ("Filling Texture", 2D) = "white" {}
		_MainTex2 ("Base Texture", 2D) = "white" {}
		_FillUpTo ("Fill Up To", Range(0, 1)) = 0
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha
		sampler2D _MainTex1;
		sampler2D _MainTex2;
		float _FillUpTo;

		struct Input 
		{
			float2 uv_MainTex1;
			float2 uv_MainTex2;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex1, IN.uv_MainTex1);
			half4 c1 = tex2D (_MainTex2, IN.uv_MainTex2);
			if(IN.uv_MainTex1.y <= _FillUpTo)
			{
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			else
			{
				o.Albedo = c1.rgb;
				o.Alpha = c1.a;
			}
		}
		ENDCG
	} 
	Fallback "Transparent/Diffuse"
}
