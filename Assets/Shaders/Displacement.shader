Shader "LD51/Displacement"
{
    Properties
    {
        _DisplaceTex ("Displacement Texture", 2D) = "white" {}
        _TexSize ("Texture Size", Range(0, 10)) = 1
        _Magnitude ("Magnitude", Range(0, 1)) = 1
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
                half4 color : COLOR;
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
                half3 worldPos : TEXCOORD1;
                half4 color : COLOR;
            };

            half _TexSize;
            
            v2f vert (appdata v)
            {               
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz * (10.1f - _TexSize);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex, _DisplaceTex;
            half4 _MainTex_ST, _Color;
            half _Magnitude;
            
            fixed4 frag (v2f i) : SV_Target
            {
                half2 disp = tex2D(_DisplaceTex, half2(i.worldPos.xy)).xy;
                disp = ((disp * 2) - 1) * _Magnitude;
                half4 texCol = tex2D(_MainTex, i.uv + disp);
                texCol *= i.color;
                
                return texCol;
            }
            ENDCG
        }
    }
}
