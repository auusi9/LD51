Shader "LD51/ItemPattern"
{
    Properties
    {
        _PatternTex ("Pattern Texture", 2D) = "white" {}
        _PatternSize ("Pattern Size", Range(0, 10)) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _ColorAtten ("Color Attenuation", Range(0, 1)) = 0.1
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
                half3 worldPos : TEXCOORD1;
            };

            half _PatternSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz * (10.1f - _PatternSize);

                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex, _PatternTex;
            half4 _MainTex_ST, _Color;
            half _ColorAtten;

            half4 frag (v2f i) : SV_Target
            {
                half4 texCol = tex2D(_MainTex, i.uv);
                texCol *= _Color;
                half4 patternCol = tex2D(_PatternTex, i.worldPos);
                patternCol.rgb = _Color - _ColorAtten;
                half4 c = texCol;
                c.rgb = (patternCol.rgb * patternCol.a) + (texCol * (1 - patternCol.a));
                return c;
            }
            ENDCG
        }
    }
}
