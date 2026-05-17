Shader "Drop/ForceField"
{
    Properties
    {
        _Color              ("Color",               Color)  = (0, 0.5, 1, 1)
        _FresnelPower       ("Fresnel Power",        Range(0.1, 10)) = 1.6
        _ScrollDirection    ("Scroll Direction",     Vector) = (4.86, 3.84, 2.8, -1.32)

        [Header(Glow)]
        _GlowIntensity      ("Glow Intensity",       Range(0, 5)) = 2.0
        _PulseSpeed         ("Pulse Speed",          Range(0, 5)) = 1.5
        _PulseAmplitude     ("Pulse Amplitude",      Range(0, 1)) = 0.2

        [Header(Lines)]
        _LinesTiling        ("Lines Tiling",         Range(1, 50)) = 20.0
        _LinesIntensity     ("Lines Intensity",      Range(0, 2)) = 0.5
        _LinesSharpness     ("Lines Sharpness",      Range(1, 50)) = 10.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"      = "Transparent"
            "RenderPipeline"  = "UniversalPipeline"
            "Queue"           = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD0;
                float3 viewDirWS  : TEXCOORD1;
                float2 uv         : TEXCOORD2;
                float3 positionWS : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                half4  _Color;
                float  _FresnelPower;
                float4 _ScrollDirection;
                float  _GlowIntensity;
                float  _PulseSpeed;
                float  _PulseAmplitude;
                float  _LinesTiling;
                float  _LinesIntensity;
                float  _LinesSharpness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs   nrmInputs = GetVertexNormalInputs(IN.normalOS);
                OUT.positionCS = posInputs.positionCS;
                OUT.positionWS = posInputs.positionWS;
                OUT.normalWS   = nrmInputs.normalWS;
                OUT.viewDirWS  = GetWorldSpaceViewDir(posInputs.positionWS);
                OUT.uv         = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 N = normalize(IN.normalWS);
                float3 V = normalize(IN.viewDirWS);

                // ── Fresnel ───────────────────────────────────────────
                float fresnel = pow(1.0 - saturate(dot(N, V)), _FresnelPower);

                // ── Pulsation ─────────────────────────────────────────
                float pulse = 1.0 + sin(_Time.y * _PulseSpeed) * _PulseAmplitude;

                // ── Lignes animees ────────────────────────────────────
                float2 scrollUV = IN.uv * _LinesTiling;
                scrollUV.x += _Time.y * _ScrollDirection.x * 0.1;
                scrollUV.y += _Time.y * _ScrollDirection.y * 0.1;

                // Lignes horizontales + verticales qui se croisent
                float lineH = pow(abs(sin(scrollUV.y * 3.14159)), _LinesSharpness);
                float lineV = pow(abs(sin(scrollUV.x * 3.14159)), _LinesSharpness);
                float lines = saturate(lineH + lineV) * _LinesIntensity;

                // ── Scintillement ─────────────────────────────────────
                float scintille = sin(_Time.y * _ScrollDirection.z + IN.uv.x * 10.0) * 0.5 + 0.5;
                scintille *= sin(_Time.y * _ScrollDirection.w + IN.uv.y * 8.0) * 0.5 + 0.5;

                // ── Assembler ─────────────────────────────────────────
                half3 col = _Color.rgb * (fresnel + lines + scintille * 0.3) * _GlowIntensity * pulse;

                // Alpha — surtout visible sur les bords (Fresnel)
                float alpha = saturate(fresnel * 2.0 + lines * 0.5) * _Color.a;

                return half4(col, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
