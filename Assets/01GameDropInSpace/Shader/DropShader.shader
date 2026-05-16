Shader "Drop/DropShader"
{
    Properties
    {
        _MainTex        ("Texture",          2D)    = "white" {}
        _Color          ("Color",            Color) = (1,1,1,1)
        _HighColor       ("High Color",       Color) = (1,1,1,1)
        _Gloss          ("Gloss",            Range(1,100)) = 10

        _ShadowStep     ("Shadow Step",      Range(0,1)) = 0
        _ShadowStepSmooth("Shadow Smooth",   Range(0,1)) = 0.1

        _SpecularStep   ("Specular Step",    Range(0,1)) = 0.5
        _SpecularStepSmooth("Specular Smooth",Range(0,1)) = 0.1

        _RimStep        ("Rim Step",         Range(0,1)) = 0.959
        _RimStepSmooth  ("Rim Smooth",       Range(0,1)) = 0.352
        _RimColor       ("Rim Color",        Color) = (0,0.5,1,1)

        _OutlineWidth   ("Outline Width",    Range(0,1)) = 0.03
        _OutlineColor   ("Outline Color",    Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        // ── PASS 1 : Outline ──────────────────────────────────────────
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front

            HLSLPROGRAM
            #pragma vertex   vert_outline
            #pragma fragment frag_outline

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };
            struct Varyings {
                float4 positionCS : SV_POSITION;
            };

            float  _OutlineWidth;
            float4 _OutlineColor;

            Varyings vert_outline(Attributes IN)
            {
                Varyings OUT;
                float3 posOS = IN.positionOS.xyz + IN.normalOS * _OutlineWidth * 0.05;
                OUT.positionCS = TransformObjectToHClip(posOS);
                return OUT;
            }

            half4 frag_outline(Varyings IN) : SV_Target
            {
                return half4(_OutlineColor.rgb, 1);
            }
            ENDHLSL
        }

        // ── PASS 2 : Toon ─────────────────────────────────────────────
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Cull Back

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };
            struct Varyings {
                float4 positionCS  : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float3 positionWS  : TEXCOORD2;
                float3 viewDirWS   : TEXCOORD3;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4  _Color;
                half4  _HighColor;
                float  _Gloss;
                float  _ShadowStep;
                float  _ShadowStepSmooth;
                float  _SpecularStep;
                float  _SpecularStepSmooth;
                float  _RimStep;
                float  _RimStepSmooth;
                half4  _RimColor;
                float  _OutlineWidth;
                half4  _OutlineColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs   nrmInputs = GetVertexNormalInputs(IN.normalOS);
                OUT.positionCS  = posInputs.positionCS;
                OUT.positionWS  = posInputs.positionWS;
                OUT.normalWS    = nrmInputs.normalWS;
                OUT.viewDirWS   = GetWorldSpaceViewDir(posInputs.positionWS);
                OUT.uv          = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex  = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half4 col  = tex * _Color;

                float3 N   = normalize(IN.normalWS);
                float3 V   = normalize(IN.viewDirWS);
                Light  L   = GetMainLight();
                float3 Ld  = normalize(L.direction);

                // Diffuse toon
                float NdL  = dot(N, Ld) * 0.5 + 0.5;
                float shadow = smoothstep(_ShadowStep, _ShadowStep + _ShadowStepSmooth, NdL);

                // Specular toon
                float3 H   = normalize(Ld + V);
                float  NdH = max(0, dot(N, H));
                float  spec = pow(NdH, _Gloss * _Gloss);
                float  specToon = smoothstep(_SpecularStep, _SpecularStep + _SpecularStepSmooth, spec);

                // Rim
                float  NdV  = 1.0 - max(0, dot(N, V));
                float  rim  = smoothstep(_RimStep, _RimStep + _RimStepSmooth, NdV);

                // Assembler
                half3 diffuse  = lerp(col.rgb * 0.4, col.rgb, shadow);
                half3 specular = specToon * _HighColor.rgb;
                half3 rimLight = rim * _RimColor.rgb;

                half3 final = diffuse + specular + rimLight;
                return half4(final, col.a);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
