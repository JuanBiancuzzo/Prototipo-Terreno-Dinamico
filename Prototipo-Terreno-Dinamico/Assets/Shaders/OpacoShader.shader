Shader "Unlit/OpacoShader"
{
    Properties
    {
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        
        //Blend One One // additive blending

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 direccionLuz = _WorldSpaceLightPos0.xyz;
                float luz = saturate(max(v.uv.x, dot(v.normal, direccionLuz)));

                //float4 lightColor = float4(_LightColor0.rgb, 1);
                o.color = v.color * luz;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}
