Shader "LD51/FireDisplacement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplaceTex ("Displacement Texture", 2D) = "white" {}
        _Magnitude ("Magnitude", Range(0, 1)) = 1
        _Speed ("Speed", Range(0, 2)) = 1
        _UnscaledTime ("Unscaled Time", Float) = 1
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
                half4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
            };
            
            v2f vert (appdata v)
            {               
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex, _DisplaceTex;
            half _Magnitude, _Speed, _UnscaledTime;
            
            fixed4 frag (v2f i) : SV_Target
            {
                half timeDistorsion =_UnscaledTime * _Speed;
                half2 disp = tex2D(_DisplaceTex, half2(i.uv.x + timeDistorsion, i.uv.y)).xy;
                disp = ((disp * 2) - 1) * _Magnitude;
                
                half4 color = tex2D(_MainTex, i.uv + disp);
                
                return color;
            }
            ENDCG
        }
    }
}
