Shader "Custom/WorldDistortionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionAmount ("Distortion Amount", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include built-in shader libraries
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float _DistortionAmount;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                
                // Apply distortion to the Y-coordinate of the vertex
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                worldPos.y += log(abs(worldPos.y) + 1.0) * _DistortionAmount;

                // Transform the position back to clip space
                o.vertex = UnityObjectToClipPos(float4(worldPos, 1.0));
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
