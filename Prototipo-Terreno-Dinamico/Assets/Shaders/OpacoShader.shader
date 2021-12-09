Shader "Unlit/OpacoShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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
                float2 iluminacion : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 direccionLuz = _WorldSpaceLightPos0.xyz;
                float t = max(v.iluminacion.x, dot(v.normal, direccionLuz));
                float luz = lerp(0.1, 1, t);

                float3 lightColor = _LightColor0.rgb;

                float3 diffuseLightcolor = lightColor * luz;
                o.color = float4(v.color.rgb * diffuseLightcolor, v.color.a);

                o.color = float4(v.iluminacion.xxx, v.color.a);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}
