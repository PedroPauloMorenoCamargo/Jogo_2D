Shader "Unlit/background"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Top Color", Color) = (1, 0, 0, 1)  // Color at the top of the screen
        _Color2 ("Bottom Color", Color) = (0, 0, 1, 1)  // Color at the bottom of the screen
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Background" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency
            ZWrite Off                      // Disable depth writing to avoid depth sorting issues
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color1;  // Top color
            fixed4 _Color2;  // Bottom color

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Vertical interpolation between _Color1 and _Color2 based on UV Y-coordinate
                float factor = i.uv.y;  // Get the vertical position of the pixel (0 at bottom, 1 at top)
                fixed4 interpolatedColor = lerp(_Color2, _Color1, factor);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Combine texture color with the interpolated background color
                col.rgb *= interpolatedColor.rgb;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                // Return color with full opacity
                return col;
            }
            ENDCG
        }
    }
}
