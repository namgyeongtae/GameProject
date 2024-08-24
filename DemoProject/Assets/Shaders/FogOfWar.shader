Shader "Custom/FogOfWar"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _PlayerPosition("Player Position", Vector) = (0, 0, 0, 0)
        _Color ("Color", Color) = (0, 0, 0, 0)
        _Radius("Radius", Range(0, 1)) = 0.035
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _PlayerPosition;
            float _Radius;
            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                float dist = distance(i.uv, _PlayerPosition.xy);
                float alpha = smoothstep(_Radius * 0.6, _Radius, dist);
                
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
