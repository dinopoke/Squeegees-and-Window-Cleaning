Shader "Custom/CleanableWindow"
{
    Properties
    {
        _MainTex ("Dirty Texture", 2D) = "white" {}
        _CleanTex ("Clean Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            sampler2D _CleanTex;
            sampler2D _MaskTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 dirty = tex2D(_MainTex, i.uv);
                fixed4 clean = tex2D(_CleanTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                return lerp(dirty, clean, mask.r);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
