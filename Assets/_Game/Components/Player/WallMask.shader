Shader "Unlit/SimpleMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Frequency ("Frequency", Range(1, 250)) = 100
        _Amplitude ("Amplitude", Range(0, 2)) = 0.1
        _Speed ("Speed", Range(0, 10)) = 1
        _Angle ("Angle", Range(0, 360)) = 0
        _Radius ("Radius", Range(1, 5)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Geometry-1" }  // Write to the stencil buffer before drawing any geometry to the screen
        LOD 100
        
        Blend One OneMinusSrcAlpha
        ColorMask 0 // Don't write to any colour channels
        ZWrite Off // Don't write to the Depth buffer
        
        // Write the value 1 to the stencil buffer
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            float4x4 Transition(float3 vertex)
            {
                return float4x4(
                    float4(1, 0, 0, vertex.x),
                    float4( 0, 1, 0, vertex.y),
                    float4( 0, 0, 1, vertex.z),
                    float4(0, 0, 0, 1));
            }

            float4x4 GetRotationMatrix(float thea)
            {
                return float4x4(
                    float4( cos(thea), -sin(thea), 0, 0),
                    float4( sin(thea), cos(thea),  0, 0),
                    float4( 0, 0, 0, 0),
                    float4(0, 0, 0, 1));
            }

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Frequency;
            float _Amplitude;
            float _Speed;
            float _Radius;
            float _Angle;

            Interpolators vert (MeshData v)
            {
                Interpolators i;
                float thea = _Angle * (6.28 / 360);
                
                v.vertex.xyz += v.normal * sin(v.vertex * _Frequency + (_Time.y * _Speed)) * _Amplitude;
                
                v.vertex = mul(v.vertex, Transition(v.vertex.xyz));
                v.vertex = mul(v.vertex, GetRotationMatrix(thea));
                v.vertex = mul(v.vertex, Transition(-v.vertex.xyz));
                
                i.vertex = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(v.vertex.xyz, 0) * float4(_Radius, _Radius, _Radius, 0));
                i.vertex.z += 0.2; // <--- Sets the billboard-mask in front and do not cover the object
                
                return i;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}