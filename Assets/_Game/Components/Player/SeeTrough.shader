Shader "Unlit/SeeTrough"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BehindWallColor ("Behind Wall Color", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off 
            ZTest GEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _BehindWallColor;

            Interpolators vert (MeshData meshData)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(meshData.vertex);
                o.uv = meshData.uv0;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv) * _BehindWallColor;
                return col;
            }
            ENDCG
        }
    }
    
     Fallback "Diffuse"
}
