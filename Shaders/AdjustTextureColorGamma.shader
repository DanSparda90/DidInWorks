Shader "Custom/AdjustTextureColorGamma"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(-2, 2)) = 1
        _Contrast("Contrast", Range(2, 3)) = 2
        _Gamma("Gamma", Range(0.1, 2)) = 1
        _Color("Color Tint", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            uniform sampler2D _MainTex;
            float _Brightness;
            float _Contrast;
            float _Gamma;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Adjust brightness, contrast, gamma, and apply color tint
                col.rgb = pow(col.rgb * _Brightness + (_Contrast - 1.0) * (col.rgb - 0.5) + 0.5, 1.0 / _Gamma);
                col.rgb *= _Color.rgb;
                col.a *= _Color.a;

                return col;
            }
            ENDCG
        }
    }
}
