Shader "Custom/EmissiveGlow"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _GlowColor("Glow Color", Color) = (1, 1, 1, 1)
        _GlowStrength("Glow Strength", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 200

        Pass
        {
            Name "ForwardPass"
            Tags { "LightMode" = "UniversalForward" }                   // Lightmode unlit test

            // For transparency blending:
            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_instancing                            // for instancing in single-pass stereo rendering
            #pragma instancing_options assumeuniformscaling nomatrices  // optional performance setting for GPU instancing


            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnityCG.cginc"
            #include "UnityShaderVariables.cginc"
            #include "UnityInstancing.cginc"

           struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID                         // for instancing in single-pass stereo rendering (input)
            };

             struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD1;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO                            // for instancing in single-pass stereo rendering (output)
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;

            float4    _GlowColor;
            float     _GlowStrength;

            Varyings vert (Attributes IN)
            {
                UNITY_SETUP_INSTANCE_ID(IN);                        // sets up the instance ID being used

                Varyings OUT;                
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);         // initializes stereo output

                OUT.positionCS = UnityObjectToClipPos(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.normalWS = normalize(mul((float3x3)unity_ObjectToWorld, IN.normalOS));
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // sampling the base texture, 
                // strengthen / weaken glow
                // combining color channels (additive glow)
                // adding alpha by saturating the sum of baseColor and glowColor alpha
                // note: if finalAlpha < 1 we have partial transparency

                float4 baseColor = tex2D(_MainTex, IN.uv);

                if (_GlowStrength == 0)
                {
                    // Display the main texture normally when GlowStrength is 0
                    return 0;
                }

                float4 glowColor = _GlowColor * _GlowStrength;
                float3 finalRGB = baseColor.rgb + glowColor.rgb;
                float finalAlpha = saturate(baseColor.a + glowColor.a);

                return float4(finalRGB, finalAlpha);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit" //remove
}