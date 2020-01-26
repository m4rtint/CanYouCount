Shader "CanYouCount/TileRendererShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _SecondaryColor ("Secondary Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags
        {
        	"Queue"="Transparent" 
            "RenderType"="Transparent" 
            "CanUseSpriteAtlas"="True"
        }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            static const float PI = 3.14159265f;

            float4 _Color;
            float4 _SecondaryColor;
            sampler2D _MainTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Program Functions
           	v2f vert(appdata IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
                fixed blendFactor = (sin(_Time * PI * 10) + 1) * 0.5 + 0.5;
				fixed4 texColor = tex2D(_MainTex, IN.uv);
                fixed4 blendedColor = _Color * blendFactor + (1 - blendFactor) * _SecondaryColor;
                texColor *= blendedColor;
				texColor.rgb *= texColor.a;
				return texColor;
			}

            ENDCG
        }
    }
}
