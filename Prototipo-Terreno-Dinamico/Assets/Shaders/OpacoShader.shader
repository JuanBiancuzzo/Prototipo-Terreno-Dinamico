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

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                //float4 normals : NORMAL;
                // float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD0;
                //float4 normals : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                //o.normals = v.normals;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //return (i.color * 8) % 8;

                return i.color;
                //return i.normals;
                //return float4(0, 1, 0, 0.5);
            }

            ENDCG
        }
    }
}
