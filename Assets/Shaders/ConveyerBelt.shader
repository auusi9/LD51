Shader "LD51/ConveyerBelt"
{
    Properties
    {
        _PatternTex ("Pattern Texture", 2D) = "white" {}
        _BeltTexTilling ("Belt Texture Tilling", Float) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _Speed ("Speed", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Lighting Off
        ZWrite Off
        ZTest Always
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                half4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _PatternTex;
            half4 _MainTex_ST;
            half _BeltTexTilling, _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                half t = _Time.y * _Speed;
                o.uv = half2(v.uv.x, (v.uv.y * _BeltTexTilling) - t);
                return o;
            }

            half4 _Color;

            half4 frag (v2f i) : SV_Target
            {
                half4 patternCol = tex2D(_PatternTex, i.uv);
                half4 c = patternCol * _Color;
                return c;
            }
            ENDCG
        }
    }
}
