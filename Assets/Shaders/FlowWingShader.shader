Shader "Custom/FlowingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Range(0.1, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                // 월드 좌표를 클립 좌표로 변환
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 텍스처 좌표 변환
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV 좌표를 가져옵니다
                float2 uv = i.uv;
                // UV 좌표를 일정 속도로 이동시킵니다
                uv.x = frac(uv.x + _Time.y * _Speed);
                // 텍스처의 색상을 가져옵니다
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
