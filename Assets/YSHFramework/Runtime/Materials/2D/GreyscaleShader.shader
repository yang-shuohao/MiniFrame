Shader "Sprites/GreyscaleShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)

        _Greyscale("Greyscale", Range(0, 1)) = 1

        [HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
        [HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM

                #pragma vertex SpriteVert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_instancing
                #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

                #include "UnitySprites.cginc"

                half _Greyscale;

                fixed4 frag(v2f IN) : SV_Target
                {
                    fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

                // 计算灰度值
                float grey = dot(c.rgb, float3(0.3, 0.59, 0.11));
                // 线性插值
                c.rgb = lerp(c.rgb, grey.xxx, _Greyscale);

                c.rgb *= c.a;

                return c;
            }

            ENDCG
        }
        }

            Fallback "Sprites/Default"
}
