Shader "CustomRenderTexture/CustomDayNightSkybox"
{
     Properties
    {
        _Cubemap1 ("Day Cubemap", CUBE) = "" {}
        _Cubemap2 ("Night Cubemap", CUBE) = "" {}
        _Blend ("Blend", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float3 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            TEXTURECUBE(_Cubemap1);
            SAMPLER(sampler_Cubemap1);
            TEXTURECUBE(_Cubemap2);
            SAMPLER(sampler_Cubemap2);
            float _Blend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.worldPos = TransformObjectToWorld(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(i.worldPos);
                half4 dayColor = SAMPLE_TEXTURECUBE(_Cubemap1, sampler_Cubemap1, viewDir);
                half4 nightColor = SAMPLE_TEXTURECUBE(_Cubemap2, sampler_Cubemap2, viewDir);
                return lerp(dayColor, nightColor, _Blend);
            }
            ENDHLSL
        }
    }
    FallBack "Skybox/Cubemap"
}
