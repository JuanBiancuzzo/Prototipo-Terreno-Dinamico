Shader "Unlit/TranslucidoShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 iluminacion : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 direccionLuz = _WorldSpaceLightPos0.xyz;
                float luz = max(v.iluminacion.x, dot(v.normal, direccionLuz));
                float3 lightColor = _LightColor0.rgb;

                float3 diffuseLightcolor = lightColor * luz;
                o.color = float4(v.color.rgb * diffuseLightcolor, v.color.a);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //return i.color;
                return float4(i.uv, 0, 1);
            }

            ENDCG
        }
    }
}
