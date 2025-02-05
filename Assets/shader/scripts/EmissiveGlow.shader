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

            // For transparency blending:
            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityShaderVariables.cginc"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;

            float4    _GlowColor;
            float     _GlowStrength;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = UnityObjectToClipPos(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
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