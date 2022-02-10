Shader "Unlit/ColorLerp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StartColor ("Start Color", Color) = (0, 0, 0, 1)
        _EndColor ("End Color", Color) = (1, 1, 1, 1)
        _BackgroundColor ("Background Color", Color) = (1, 1, 1, 1)
        _CurrentPosition  ("Current Position", Range(0.0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _StartColor;
            float4 _BackgroundColor;
            float4 _EndColor;
            float _UseSolidBackground;
            float _CurrentPosition;

            Interpolators vert (MeshData meshData)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(meshData.vertex);
                o.uv = meshData.uv;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float4 color = lerp(_StartColor, _EndColor, _CurrentPosition);
                float barMask = _CurrentPosition > i.uv.y;
                
                float4 outColor = lerp(_BackgroundColor, color, barMask);
                
                return outColor;
            }
            ENDCG
        }
    }
}
