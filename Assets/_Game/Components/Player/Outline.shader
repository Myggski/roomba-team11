Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 0.03
        [MaterialToggle] _UseConsistentWidth ("Use Consistent Width", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Stencil
            {
                Ref 2
                Comp Always
                Pass Replace
            }

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            Interpolators vert (MeshData meshData)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(meshData.vertex);
                o.uv = meshData.uv0;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }

        Pass
        {
            Stencil {
                Ref 2
                Comp NotEqual
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 worldPos : TEXCOORD1;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float4 test : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };
            

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _UseConsistentWidth;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                
                float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
                float cameraDistance = 1 - distance(worldPosition, _WorldSpaceCameraPos) / _OutlineWidth;
                float outlineToRemove = (_UseConsistentWidth == 0) * (_OutlineWidth - (_OutlineWidth * cameraDistance));
                float outline = clamp(_OutlineWidth - outlineToRemove, 0, _OutlineWidth);
                
                float4 clipPosition = UnityObjectToClipPos(v.vertex);
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
                clipPosition.xy += normalize(clipNormal.xy) / _ScreenParams.xy * outline * clipPosition.w * 2;
                
                o.vertex = clipPosition;

                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    
    Fallback "Diffuse"
}
