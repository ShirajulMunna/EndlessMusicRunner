Shader "Custom/FlowingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Range(0.1, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        float _Speed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv = IN.uv_MainTex;
            uv.x += frac(_Time.y * _Speed * 0.1);
            fixed4 c = tex2D(_MainTex, uv);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}