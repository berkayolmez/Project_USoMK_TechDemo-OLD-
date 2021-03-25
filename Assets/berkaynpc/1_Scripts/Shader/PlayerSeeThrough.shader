Shader "PlayerSeeThrough"
{
    Properties
    {
        _Position("PlayerPosition", Vector) = (0.5, 0.5, 0, 0)
        _Size("Size", Float) = 1
        Vector1_4c8346c3e1d04caaa3e99436a21c6463("Smoothness", Range(0, 1)) = 0.5
        Vector1_aeb586a2664d4d429e0412ff5b266119("Opacity", Range(0, 1)) = 1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue" = "Transparent"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            float2 lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 sh;
            #endif
            float4 fogFactorAndVertexLight;
            float4 shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 TangentSpaceNormal;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if defined(LIGHTMAP_ON)
            float2 interp4 : TEXCOORD4;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 interp5 : TEXCOORD5;
            #endif
            float4 interp6 : TEXCOORD6;
            float4 interp7 : TEXCOORD7;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz = input.positionWS;
            output.interp1.xyz = input.normalWS;
            output.interp2.xyzw = input.tangentWS;
            output.interp3.xyz = input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp4.xy = input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp5.xyz = input.sh;
            #endif
            output.interp6.xyzw = input.fogFactorAndVertexLight;
            output.interp7.xyzw = input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp4.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp5.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp6.xyzw;
            output.shadowCoord = input.interp7.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float2 _Position;
        float _Size;
        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
        float Vector1_aeb586a2664d4d429e0412ff5b266119;
        CBUFFER_END

            // Object and Global properties

            // Graph Functions

            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }

            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A - B;
            }

            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A / B;
            }

            void Unity_Length_float2(float2 In, out float Out)
            {
                Out = length(In);
            }

            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }

            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }

            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float3 NormalTS;
                float3 Emission;
                float Metallic;
                float Smoothness;
                float Occlusion;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                surface.NormalTS = IN.TangentSpaceNormal;
                surface.Emission = float3(0, 0, 0);
                surface.Metallic = 0;
                surface.Smoothness = 0.5;
                surface.Occlusion = 1;
                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                output.WorldSpacePosition = input.positionWS;
                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }

                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                HLSLPROGRAM

                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile_fog
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag

                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>

                // Keywords
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
                #pragma multi_compile _ _SHADOWS_SOFT
                #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
                #pragma multi_compile _ _GBUFFER_NORMALS_OCT
                // GraphKeywords: <None>

                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define VARYINGS_NEED_VIEWDIRECTION_WS
                #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_GBUFFER
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                // --------------------------------------------------
                // Structs and Packing

                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float3 normalWS;
                    float4 tangentWS;
                    float3 viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    float2 lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 sh;
                    #endif
                    float4 fogFactorAndVertexLight;
                    float4 shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 WorldSpacePosition;
                    float4 ScreenPosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float3 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    float3 interp3 : TEXCOORD3;
                    #if defined(LIGHTMAP_ON)
                    float2 interp4 : TEXCOORD4;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 interp5 : TEXCOORD5;
                    #endif
                    float4 interp6 : TEXCOORD6;
                    float4 interp7 : TEXCOORD7;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz = input.positionWS;
                    output.interp1.xyz = input.normalWS;
                    output.interp2.xyzw = input.tangentWS;
                    output.interp3.xyz = input.viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    output.interp4.xy = input.lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.interp5.xyz = input.sh;
                    #endif
                    output.interp6.xyzw = input.fogFactorAndVertexLight;
                    output.interp7.xyzw = input.shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.normalWS = input.interp1.xyz;
                    output.tangentWS = input.interp2.xyzw;
                    output.viewDirectionWS = input.interp3.xyz;
                    #if defined(LIGHTMAP_ON)
                    output.lightmapUV = input.interp4.xy;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.sh = input.interp5.xyz;
                    #endif
                    output.fogFactorAndVertexLight = input.interp6.xyzw;
                    output.shadowCoord = input.interp7.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float2 _Position;
                float _Size;
                float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                float Vector1_aeb586a2664d4d429e0412ff5b266119;
                CBUFFER_END

                    // Object and Global properties

                    // Graph Functions

                    void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                    {
                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                    }

                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }

                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }

                    void Unity_Divide_float(float A, float B, out float Out)
                    {
                        Out = A / B;
                    }

                    void Unity_Multiply_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A / B;
                    }

                    void Unity_Length_float2(float2 In, out float Out)
                    {
                        Out = length(In);
                    }

                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }

                    void Unity_Saturate_float(float In, out float Out)
                    {
                        Out = saturate(In);
                    }

                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                    {
                        Out = smoothstep(Edge1, Edge2, In);
                    }

                    // Graph Vertex
                    struct VertexDescription
                    {
                        float3 Position;
                        float3 Normal;
                        float3 Tangent;
                    };

                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        description.Position = IN.ObjectSpacePosition;
                        description.Normal = IN.ObjectSpaceNormal;
                        description.Tangent = IN.ObjectSpaceTangent;
                        return description;
                    }

                    // Graph Pixel
                    struct SurfaceDescription
                    {
                        float3 BaseColor;
                        float3 NormalTS;
                        float3 Emission;
                        float Metallic;
                        float Smoothness;
                        float Occlusion;
                        float Alpha;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                        float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                        float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                        float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                        Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                        float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                        Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                        float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                        Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                        float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                        Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                        float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                        Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                        float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                        float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                        Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                        float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                        Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                        float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                        float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                        Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                        float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                        Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                        float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                        Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                        float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                        Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                        float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                        Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                        float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                        float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                        Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                        float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                        Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                        surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                        surface.NormalTS = IN.TangentSpaceNormal;
                        surface.Emission = float3(0, 0, 0);
                        surface.Metallic = 0;
                        surface.Smoothness = 0.5;
                        surface.Occlusion = 1;
                        surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                    {
                        VertexDescriptionInputs output;
                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                        output.ObjectSpaceNormal = input.normalOS;
                        output.ObjectSpaceTangent = input.tangentOS;
                        output.ObjectSpacePosition = input.positionOS;

                        return output;
                    }

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                        output.WorldSpacePosition = input.positionWS;
                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"

                    ENDHLSL
                }
                Pass
                {
                    Name "ShadowCaster"
                    Tags
                    {
                        "LightMode" = "ShadowCaster"
                    }

                        // Render State
                        Cull Back
                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                        ZTest LEqual
                        ZWrite On
                        ColorMask 0

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        HLSLPROGRAM

                        // Pragmas
                        #pragma target 4.5
                        #pragma exclude_renderers gles gles3 glcore
                        #pragma multi_compile_instancing
                        #pragma multi_compile _ DOTS_INSTANCING_ON
                        #pragma vertex vert
                        #pragma fragment frag

                        // DotsInstancingOptions: <None>
                        // HybridV1InjectedBuiltinProperties: <None>

                        // Keywords
                        // PassKeywords: <None>
                        // GraphKeywords: <None>

                        // Defines
                        #define _SURFACE_TYPE_TRANSPARENT 1
                        #define _NORMALMAP 1
                        #define _NORMAL_DROPOFF_TS 1
                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define VARYINGS_NEED_POSITION_WS
                        #define FEATURES_GRAPH_VERTEX
                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                        #define SHADERPASS SHADERPASS_SHADOWCASTER
                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                        // --------------------------------------------------
                        // Structs and Packing

                        struct Attributes
                        {
                            float3 positionOS : POSITION;
                            float3 normalOS : NORMAL;
                            float4 tangentOS : TANGENT;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };
                        struct Varyings
                        {
                            float4 positionCS : SV_POSITION;
                            float3 positionWS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };
                        struct SurfaceDescriptionInputs
                        {
                            float3 WorldSpacePosition;
                            float4 ScreenPosition;
                        };
                        struct VertexDescriptionInputs
                        {
                            float3 ObjectSpaceNormal;
                            float3 ObjectSpaceTangent;
                            float3 ObjectSpacePosition;
                        };
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_POSITION;
                            float3 interp0 : TEXCOORD0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output;
                            output.positionCS = input.positionCS;
                            output.interp0.xyz = input.positionWS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output;
                            output.positionCS = input.positionCS;
                            output.positionWS = input.interp0.xyz;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float2 _Position;
                        float _Size;
                        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                        float Vector1_aeb586a2664d4d429e0412ff5b266119;
                        CBUFFER_END

                            // Object and Global properties

                            // Graph Functions

                            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                            {
                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                            }

                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                            {
                                Out = A + B;
                            }

                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                            {
                                Out = UV * Tiling + Offset;
                            }

                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                            {
                                Out = A - B;
                            }

                            void Unity_Divide_float(float A, float B, out float Out)
                            {
                                Out = A / B;
                            }

                            void Unity_Multiply_float(float A, float B, out float Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                            {
                                Out = A / B;
                            }

                            void Unity_Length_float2(float2 In, out float Out)
                            {
                                Out = length(In);
                            }

                            void Unity_OneMinus_float(float In, out float Out)
                            {
                                Out = 1 - In;
                            }

                            void Unity_Saturate_float(float In, out float Out)
                            {
                                Out = saturate(In);
                            }

                            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                            {
                                Out = smoothstep(Edge1, Edge2, In);
                            }

                            // Graph Vertex
                            struct VertexDescription
                            {
                                float3 Position;
                                float3 Normal;
                                float3 Tangent;
                            };

                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                            {
                                VertexDescription description = (VertexDescription)0;
                                description.Position = IN.ObjectSpacePosition;
                                description.Normal = IN.ObjectSpaceNormal;
                                description.Tangent = IN.ObjectSpaceTangent;
                                return description;
                            }

                            // Graph Pixel
                            struct SurfaceDescription
                            {
                                float Alpha;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Build Graph Inputs

                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                            {
                                VertexDescriptionInputs output;
                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                output.ObjectSpaceNormal = input.normalOS;
                                output.ObjectSpaceTangent = input.tangentOS;
                                output.ObjectSpacePosition = input.positionOS;

                                return output;
                            }

                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                output.WorldSpacePosition = input.positionWS;
                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif
                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                return output;
                            }


                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                            ENDHLSL
                        }
                        Pass
                        {
                            Name "DepthOnly"
                            Tags
                            {
                                "LightMode" = "DepthOnly"
                            }

                                // Render State
                                Cull Back
                                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                ZTest LEqual
                                ZWrite On
                                ColorMask 0

                                // Debug
                                // <None>

                                // --------------------------------------------------
                                // Pass

                                HLSLPROGRAM

                                // Pragmas
                                #pragma target 4.5
                                #pragma exclude_renderers gles gles3 glcore
                                #pragma multi_compile_instancing
                                #pragma multi_compile _ DOTS_INSTANCING_ON
                                #pragma vertex vert
                                #pragma fragment frag

                                // DotsInstancingOptions: <None>
                                // HybridV1InjectedBuiltinProperties: <None>

                                // Keywords
                                // PassKeywords: <None>
                                // GraphKeywords: <None>

                                // Defines
                                #define _SURFACE_TYPE_TRANSPARENT 1
                                #define _NORMALMAP 1
                                #define _NORMAL_DROPOFF_TS 1
                                #define ATTRIBUTES_NEED_NORMAL
                                #define ATTRIBUTES_NEED_TANGENT
                                #define VARYINGS_NEED_POSITION_WS
                                #define FEATURES_GRAPH_VERTEX
                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                #define SHADERPASS SHADERPASS_DEPTHONLY
                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                // Includes
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                // --------------------------------------------------
                                // Structs and Packing

                                struct Attributes
                                {
                                    float3 positionOS : POSITION;
                                    float3 normalOS : NORMAL;
                                    float4 tangentOS : TANGENT;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    uint instanceID : INSTANCEID_SEMANTIC;
                                    #endif
                                };
                                struct Varyings
                                {
                                    float4 positionCS : SV_POSITION;
                                    float3 positionWS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };
                                struct SurfaceDescriptionInputs
                                {
                                    float3 WorldSpacePosition;
                                    float4 ScreenPosition;
                                };
                                struct VertexDescriptionInputs
                                {
                                    float3 ObjectSpaceNormal;
                                    float3 ObjectSpaceTangent;
                                    float3 ObjectSpacePosition;
                                };
                                struct PackedVaryings
                                {
                                    float4 positionCS : SV_POSITION;
                                    float3 interp0 : TEXCOORD0;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };

                                PackedVaryings PackVaryings(Varyings input)
                                {
                                    PackedVaryings output;
                                    output.positionCS = input.positionCS;
                                    output.interp0.xyz = input.positionWS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }
                                Varyings UnpackVaryings(PackedVaryings input)
                                {
                                    Varyings output;
                                    output.positionCS = input.positionCS;
                                    output.positionWS = input.interp0.xyz;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }

                                // --------------------------------------------------
                                // Graph

                                // Graph Properties
                                CBUFFER_START(UnityPerMaterial)
                                float2 _Position;
                                float _Size;
                                float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                CBUFFER_END

                                    // Object and Global properties

                                    // Graph Functions

                                    void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                    {
                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                    }

                                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                    {
                                        Out = A + B;
                                    }

                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                    {
                                        Out = UV * Tiling + Offset;
                                    }

                                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                    {
                                        Out = A * B;
                                    }

                                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                    {
                                        Out = A - B;
                                    }

                                    void Unity_Divide_float(float A, float B, out float Out)
                                    {
                                        Out = A / B;
                                    }

                                    void Unity_Multiply_float(float A, float B, out float Out)
                                    {
                                        Out = A * B;
                                    }

                                    void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                    {
                                        Out = A / B;
                                    }

                                    void Unity_Length_float2(float2 In, out float Out)
                                    {
                                        Out = length(In);
                                    }

                                    void Unity_OneMinus_float(float In, out float Out)
                                    {
                                        Out = 1 - In;
                                    }

                                    void Unity_Saturate_float(float In, out float Out)
                                    {
                                        Out = saturate(In);
                                    }

                                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                    {
                                        Out = smoothstep(Edge1, Edge2, In);
                                    }

                                    // Graph Vertex
                                    struct VertexDescription
                                    {
                                        float3 Position;
                                        float3 Normal;
                                        float3 Tangent;
                                    };

                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                    {
                                        VertexDescription description = (VertexDescription)0;
                                        description.Position = IN.ObjectSpacePosition;
                                        description.Normal = IN.ObjectSpaceNormal;
                                        description.Tangent = IN.ObjectSpaceTangent;
                                        return description;
                                    }

                                    // Graph Pixel
                                    struct SurfaceDescription
                                    {
                                        float Alpha;
                                    };

                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                    {
                                        SurfaceDescription surface = (SurfaceDescription)0;
                                        float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                        float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                        float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                        float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                        Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                        float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                        Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                        float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                        Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                        float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                        Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                        float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                        Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                        float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                        float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                        Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                        float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                        Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                        float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                        float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                        Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                        float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                        Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                        float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                        Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                        float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                        Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                        float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                        Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                        float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                        float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                        Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                        float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                        Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                        surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                        return surface;
                                    }

                                    // --------------------------------------------------
                                    // Build Graph Inputs

                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                    {
                                        VertexDescriptionInputs output;
                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                        output.ObjectSpaceNormal = input.normalOS;
                                        output.ObjectSpaceTangent = input.tangentOS;
                                        output.ObjectSpacePosition = input.positionOS;

                                        return output;
                                    }

                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                    {
                                        SurfaceDescriptionInputs output;
                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                        output.WorldSpacePosition = input.positionWS;
                                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                    #else
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                    #endif
                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                        return output;
                                    }


                                    // --------------------------------------------------
                                    // Main

                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                                    ENDHLSL
                                }
                                Pass
                                {
                                    Name "DepthNormals"
                                    Tags
                                    {
                                        "LightMode" = "DepthNormals"
                                    }

                                        // Render State
                                        Cull Back
                                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                        ZTest LEqual
                                        ZWrite On

                                        // Debug
                                        // <None>

                                        // --------------------------------------------------
                                        // Pass

                                        HLSLPROGRAM

                                        // Pragmas
                                        #pragma target 4.5
                                        #pragma exclude_renderers gles gles3 glcore
                                        #pragma multi_compile_instancing
                                        #pragma multi_compile _ DOTS_INSTANCING_ON
                                        #pragma vertex vert
                                        #pragma fragment frag

                                        // DotsInstancingOptions: <None>
                                        // HybridV1InjectedBuiltinProperties: <None>

                                        // Keywords
                                        // PassKeywords: <None>
                                        // GraphKeywords: <None>

                                        // Defines
                                        #define _SURFACE_TYPE_TRANSPARENT 1
                                        #define _NORMALMAP 1
                                        #define _NORMAL_DROPOFF_TS 1
                                        #define ATTRIBUTES_NEED_NORMAL
                                        #define ATTRIBUTES_NEED_TANGENT
                                        #define ATTRIBUTES_NEED_TEXCOORD1
                                        #define VARYINGS_NEED_POSITION_WS
                                        #define VARYINGS_NEED_NORMAL_WS
                                        #define VARYINGS_NEED_TANGENT_WS
                                        #define FEATURES_GRAPH_VERTEX
                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                        // Includes
                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                        // --------------------------------------------------
                                        // Structs and Packing

                                        struct Attributes
                                        {
                                            float3 positionOS : POSITION;
                                            float3 normalOS : NORMAL;
                                            float4 tangentOS : TANGENT;
                                            float4 uv1 : TEXCOORD1;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            uint instanceID : INSTANCEID_SEMANTIC;
                                            #endif
                                        };
                                        struct Varyings
                                        {
                                            float4 positionCS : SV_POSITION;
                                            float3 positionWS;
                                            float3 normalWS;
                                            float4 tangentWS;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                            #endif
                                        };
                                        struct SurfaceDescriptionInputs
                                        {
                                            float3 TangentSpaceNormal;
                                            float3 WorldSpacePosition;
                                            float4 ScreenPosition;
                                        };
                                        struct VertexDescriptionInputs
                                        {
                                            float3 ObjectSpaceNormal;
                                            float3 ObjectSpaceTangent;
                                            float3 ObjectSpacePosition;
                                        };
                                        struct PackedVaryings
                                        {
                                            float4 positionCS : SV_POSITION;
                                            float3 interp0 : TEXCOORD0;
                                            float3 interp1 : TEXCOORD1;
                                            float4 interp2 : TEXCOORD2;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                            #endif
                                        };

                                        PackedVaryings PackVaryings(Varyings input)
                                        {
                                            PackedVaryings output;
                                            output.positionCS = input.positionCS;
                                            output.interp0.xyz = input.positionWS;
                                            output.interp1.xyz = input.normalWS;
                                            output.interp2.xyzw = input.tangentWS;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            output.instanceID = input.instanceID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            output.cullFace = input.cullFace;
                                            #endif
                                            return output;
                                        }
                                        Varyings UnpackVaryings(PackedVaryings input)
                                        {
                                            Varyings output;
                                            output.positionCS = input.positionCS;
                                            output.positionWS = input.interp0.xyz;
                                            output.normalWS = input.interp1.xyz;
                                            output.tangentWS = input.interp2.xyzw;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            output.instanceID = input.instanceID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            output.cullFace = input.cullFace;
                                            #endif
                                            return output;
                                        }

                                        // --------------------------------------------------
                                        // Graph

                                        // Graph Properties
                                        CBUFFER_START(UnityPerMaterial)
                                        float2 _Position;
                                        float _Size;
                                        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                        float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                        CBUFFER_END

                                            // Object and Global properties

                                            // Graph Functions

                                            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                            {
                                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                            }

                                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                            {
                                                Out = A + B;
                                            }

                                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                            {
                                                Out = UV * Tiling + Offset;
                                            }

                                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                            {
                                                Out = A * B;
                                            }

                                            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                            {
                                                Out = A - B;
                                            }

                                            void Unity_Divide_float(float A, float B, out float Out)
                                            {
                                                Out = A / B;
                                            }

                                            void Unity_Multiply_float(float A, float B, out float Out)
                                            {
                                                Out = A * B;
                                            }

                                            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                            {
                                                Out = A / B;
                                            }

                                            void Unity_Length_float2(float2 In, out float Out)
                                            {
                                                Out = length(In);
                                            }

                                            void Unity_OneMinus_float(float In, out float Out)
                                            {
                                                Out = 1 - In;
                                            }

                                            void Unity_Saturate_float(float In, out float Out)
                                            {
                                                Out = saturate(In);
                                            }

                                            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                            {
                                                Out = smoothstep(Edge1, Edge2, In);
                                            }

                                            // Graph Vertex
                                            struct VertexDescription
                                            {
                                                float3 Position;
                                                float3 Normal;
                                                float3 Tangent;
                                            };

                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                            {
                                                VertexDescription description = (VertexDescription)0;
                                                description.Position = IN.ObjectSpacePosition;
                                                description.Normal = IN.ObjectSpaceNormal;
                                                description.Tangent = IN.ObjectSpaceTangent;
                                                return description;
                                            }

                                            // Graph Pixel
                                            struct SurfaceDescription
                                            {
                                                float3 NormalTS;
                                                float Alpha;
                                            };

                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                            {
                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                surface.NormalTS = IN.TangentSpaceNormal;
                                                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                return surface;
                                            }

                                            // --------------------------------------------------
                                            // Build Graph Inputs

                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                            {
                                                VertexDescriptionInputs output;
                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                output.ObjectSpaceNormal = input.normalOS;
                                                output.ObjectSpaceTangent = input.tangentOS;
                                                output.ObjectSpacePosition = input.positionOS;

                                                return output;
                                            }

                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                            {
                                                SurfaceDescriptionInputs output;
                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                                                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                                                output.WorldSpacePosition = input.positionWS;
                                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                            #else
                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                            #endif
                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                return output;
                                            }


                                            // --------------------------------------------------
                                            // Main

                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

                                            ENDHLSL
                                        }
                                        Pass
                                        {
                                            Name "Meta"
                                            Tags
                                            {
                                                "LightMode" = "Meta"
                                            }

                                                // Render State
                                                Cull Off

                                                // Debug
                                                // <None>

                                                // --------------------------------------------------
                                                // Pass

                                                HLSLPROGRAM

                                                // Pragmas
                                                #pragma target 4.5
                                                #pragma exclude_renderers gles gles3 glcore
                                                #pragma vertex vert
                                                #pragma fragment frag

                                                // DotsInstancingOptions: <None>
                                                // HybridV1InjectedBuiltinProperties: <None>

                                                // Keywords
                                                #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                                                // GraphKeywords: <None>

                                                // Defines
                                                #define _SURFACE_TYPE_TRANSPARENT 1
                                                #define _NORMALMAP 1
                                                #define _NORMAL_DROPOFF_TS 1
                                                #define ATTRIBUTES_NEED_NORMAL
                                                #define ATTRIBUTES_NEED_TANGENT
                                                #define ATTRIBUTES_NEED_TEXCOORD1
                                                #define ATTRIBUTES_NEED_TEXCOORD2
                                                #define VARYINGS_NEED_POSITION_WS
                                                #define FEATURES_GRAPH_VERTEX
                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                #define SHADERPASS SHADERPASS_META
                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                // Includes
                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

                                                // --------------------------------------------------
                                                // Structs and Packing

                                                struct Attributes
                                                {
                                                    float3 positionOS : POSITION;
                                                    float3 normalOS : NORMAL;
                                                    float4 tangentOS : TANGENT;
                                                    float4 uv1 : TEXCOORD1;
                                                    float4 uv2 : TEXCOORD2;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    uint instanceID : INSTANCEID_SEMANTIC;
                                                    #endif
                                                };
                                                struct Varyings
                                                {
                                                    float4 positionCS : SV_POSITION;
                                                    float3 positionWS;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                    #endif
                                                };
                                                struct SurfaceDescriptionInputs
                                                {
                                                    float3 WorldSpacePosition;
                                                    float4 ScreenPosition;
                                                };
                                                struct VertexDescriptionInputs
                                                {
                                                    float3 ObjectSpaceNormal;
                                                    float3 ObjectSpaceTangent;
                                                    float3 ObjectSpacePosition;
                                                };
                                                struct PackedVaryings
                                                {
                                                    float4 positionCS : SV_POSITION;
                                                    float3 interp0 : TEXCOORD0;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                    #endif
                                                };

                                                PackedVaryings PackVaryings(Varyings input)
                                                {
                                                    PackedVaryings output;
                                                    output.positionCS = input.positionCS;
                                                    output.interp0.xyz = input.positionWS;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    output.instanceID = input.instanceID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    output.cullFace = input.cullFace;
                                                    #endif
                                                    return output;
                                                }
                                                Varyings UnpackVaryings(PackedVaryings input)
                                                {
                                                    Varyings output;
                                                    output.positionCS = input.positionCS;
                                                    output.positionWS = input.interp0.xyz;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    output.instanceID = input.instanceID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    output.cullFace = input.cullFace;
                                                    #endif
                                                    return output;
                                                }

                                                // --------------------------------------------------
                                                // Graph

                                                // Graph Properties
                                                CBUFFER_START(UnityPerMaterial)
                                                float2 _Position;
                                                float _Size;
                                                float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                CBUFFER_END

                                                    // Object and Global properties

                                                    // Graph Functions

                                                    void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                    {
                                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                    }

                                                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                    {
                                                        Out = A + B;
                                                    }

                                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                    {
                                                        Out = UV * Tiling + Offset;
                                                    }

                                                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                    {
                                                        Out = A * B;
                                                    }

                                                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                    {
                                                        Out = A - B;
                                                    }

                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                    {
                                                        Out = A / B;
                                                    }

                                                    void Unity_Multiply_float(float A, float B, out float Out)
                                                    {
                                                        Out = A * B;
                                                    }

                                                    void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                    {
                                                        Out = A / B;
                                                    }

                                                    void Unity_Length_float2(float2 In, out float Out)
                                                    {
                                                        Out = length(In);
                                                    }

                                                    void Unity_OneMinus_float(float In, out float Out)
                                                    {
                                                        Out = 1 - In;
                                                    }

                                                    void Unity_Saturate_float(float In, out float Out)
                                                    {
                                                        Out = saturate(In);
                                                    }

                                                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                    {
                                                        Out = smoothstep(Edge1, Edge2, In);
                                                    }

                                                    // Graph Vertex
                                                    struct VertexDescription
                                                    {
                                                        float3 Position;
                                                        float3 Normal;
                                                        float3 Tangent;
                                                    };

                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                    {
                                                        VertexDescription description = (VertexDescription)0;
                                                        description.Position = IN.ObjectSpacePosition;
                                                        description.Normal = IN.ObjectSpaceNormal;
                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                        return description;
                                                    }

                                                    // Graph Pixel
                                                    struct SurfaceDescription
                                                    {
                                                        float3 BaseColor;
                                                        float3 Emission;
                                                        float Alpha;
                                                    };

                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                    {
                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                        float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                        float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                        float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                        float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                        Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                        float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                        Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                        float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                        Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                        float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                        Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                        float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                        Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                        float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                        float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                        Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                        float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                        Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                        float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                        float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                        Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                        float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                        Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                        float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                        Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                        float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                        Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                        float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                        Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                        float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                        float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                        Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                        float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                        Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                        surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                                                        surface.Emission = float3(0, 0, 0);
                                                        surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                        return surface;
                                                    }

                                                    // --------------------------------------------------
                                                    // Build Graph Inputs

                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                    {
                                                        VertexDescriptionInputs output;
                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                        output.ObjectSpaceNormal = input.normalOS;
                                                        output.ObjectSpaceTangent = input.tangentOS;
                                                        output.ObjectSpacePosition = input.positionOS;

                                                        return output;
                                                    }

                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                    {
                                                        SurfaceDescriptionInputs output;
                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                                        output.WorldSpacePosition = input.positionWS;
                                                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                    #else
                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                    #endif
                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                        return output;
                                                    }


                                                    // --------------------------------------------------
                                                    // Main

                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                                                    ENDHLSL
                                                }
                                                Pass
                                                {
                                                        // Name: <None>
                                                        Tags
                                                        {
                                                            "LightMode" = "Universal2D"
                                                        }

                                                        // Render State
                                                        Cull Back
                                                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                        ZTest LEqual
                                                        ZWrite Off

                                                        // Debug
                                                        // <None>

                                                        // --------------------------------------------------
                                                        // Pass

                                                        HLSLPROGRAM

                                                        // Pragmas
                                                        #pragma target 4.5
                                                        #pragma exclude_renderers gles gles3 glcore
                                                        #pragma vertex vert
                                                        #pragma fragment frag

                                                        // DotsInstancingOptions: <None>
                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                        // Keywords
                                                        // PassKeywords: <None>
                                                        // GraphKeywords: <None>

                                                        // Defines
                                                        #define _SURFACE_TYPE_TRANSPARENT 1
                                                        #define _NORMALMAP 1
                                                        #define _NORMAL_DROPOFF_TS 1
                                                        #define ATTRIBUTES_NEED_NORMAL
                                                        #define ATTRIBUTES_NEED_TANGENT
                                                        #define VARYINGS_NEED_POSITION_WS
                                                        #define FEATURES_GRAPH_VERTEX
                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                        #define SHADERPASS SHADERPASS_2D
                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                        // Includes
                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                                        // --------------------------------------------------
                                                        // Structs and Packing

                                                        struct Attributes
                                                        {
                                                            float3 positionOS : POSITION;
                                                            float3 normalOS : NORMAL;
                                                            float4 tangentOS : TANGENT;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            uint instanceID : INSTANCEID_SEMANTIC;
                                                            #endif
                                                        };
                                                        struct Varyings
                                                        {
                                                            float4 positionCS : SV_POSITION;
                                                            float3 positionWS;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                            #endif
                                                        };
                                                        struct SurfaceDescriptionInputs
                                                        {
                                                            float3 WorldSpacePosition;
                                                            float4 ScreenPosition;
                                                        };
                                                        struct VertexDescriptionInputs
                                                        {
                                                            float3 ObjectSpaceNormal;
                                                            float3 ObjectSpaceTangent;
                                                            float3 ObjectSpacePosition;
                                                        };
                                                        struct PackedVaryings
                                                        {
                                                            float4 positionCS : SV_POSITION;
                                                            float3 interp0 : TEXCOORD0;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                            #endif
                                                        };

                                                        PackedVaryings PackVaryings(Varyings input)
                                                        {
                                                            PackedVaryings output;
                                                            output.positionCS = input.positionCS;
                                                            output.interp0.xyz = input.positionWS;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            output.instanceID = input.instanceID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            output.cullFace = input.cullFace;
                                                            #endif
                                                            return output;
                                                        }
                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                        {
                                                            Varyings output;
                                                            output.positionCS = input.positionCS;
                                                            output.positionWS = input.interp0.xyz;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            output.instanceID = input.instanceID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            output.cullFace = input.cullFace;
                                                            #endif
                                                            return output;
                                                        }

                                                        // --------------------------------------------------
                                                        // Graph

                                                        // Graph Properties
                                                        CBUFFER_START(UnityPerMaterial)
                                                        float2 _Position;
                                                        float _Size;
                                                        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                        float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                        CBUFFER_END

                                                            // Object and Global properties

                                                            // Graph Functions

                                                            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                            {
                                                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                            }

                                                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                            {
                                                                Out = A + B;
                                                            }

                                                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                            {
                                                                Out = UV * Tiling + Offset;
                                                            }

                                                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                            {
                                                                Out = A * B;
                                                            }

                                                            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                            {
                                                                Out = A - B;
                                                            }

                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                            {
                                                                Out = A / B;
                                                            }

                                                            void Unity_Multiply_float(float A, float B, out float Out)
                                                            {
                                                                Out = A * B;
                                                            }

                                                            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                            {
                                                                Out = A / B;
                                                            }

                                                            void Unity_Length_float2(float2 In, out float Out)
                                                            {
                                                                Out = length(In);
                                                            }

                                                            void Unity_OneMinus_float(float In, out float Out)
                                                            {
                                                                Out = 1 - In;
                                                            }

                                                            void Unity_Saturate_float(float In, out float Out)
                                                            {
                                                                Out = saturate(In);
                                                            }

                                                            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                            {
                                                                Out = smoothstep(Edge1, Edge2, In);
                                                            }

                                                            // Graph Vertex
                                                            struct VertexDescription
                                                            {
                                                                float3 Position;
                                                                float3 Normal;
                                                                float3 Tangent;
                                                            };

                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                            {
                                                                VertexDescription description = (VertexDescription)0;
                                                                description.Position = IN.ObjectSpacePosition;
                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                return description;
                                                            }

                                                            // Graph Pixel
                                                            struct SurfaceDescription
                                                            {
                                                                float3 BaseColor;
                                                                float Alpha;
                                                            };

                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                            {
                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                                                                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                return surface;
                                                            }

                                                            // --------------------------------------------------
                                                            // Build Graph Inputs

                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                            {
                                                                VertexDescriptionInputs output;
                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                output.ObjectSpaceTangent = input.tangentOS;
                                                                output.ObjectSpacePosition = input.positionOS;

                                                                return output;
                                                            }

                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                            {
                                                                SurfaceDescriptionInputs output;
                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                                                output.WorldSpacePosition = input.positionWS;
                                                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                            #else
                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                            #endif
                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                return output;
                                                            }


                                                            // --------------------------------------------------
                                                            // Main

                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                                                            ENDHLSL
                                                        }
    }
        SubShader
                                                            {
                                                                Tags
                                                                {
                                                                    "RenderPipeline" = "UniversalPipeline"
                                                                    "RenderType" = "Transparent"
                                                                    "UniversalMaterialType" = "Lit"
                                                                    "Queue" = "Transparent"
                                                                }
                                                                Pass
                                                                {
                                                                    Name "Universal Forward"
                                                                    Tags
                                                                    {
                                                                        "LightMode" = "UniversalForward"
                                                                    }

                                                                // Render State
                                                                Cull Back
                                                                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                                ZTest LEqual
                                                                ZWrite Off

                                                                // Debug
                                                                // <None>

                                                                // --------------------------------------------------
                                                                // Pass

                                                                HLSLPROGRAM

                                                                // Pragmas
                                                                #pragma target 2.0
                                                                #pragma only_renderers gles gles3 glcore
                                                                #pragma multi_compile_instancing
                                                                #pragma multi_compile_fog
                                                                #pragma vertex vert
                                                                #pragma fragment frag

                                                                // DotsInstancingOptions: <None>
                                                                // HybridV1InjectedBuiltinProperties: <None>

                                                                // Keywords
                                                                #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
                                                                #pragma multi_compile _ LIGHTMAP_ON
                                                                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                                                                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                                                                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
                                                                #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
                                                                #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
                                                                #pragma multi_compile _ _SHADOWS_SOFT
                                                                #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
                                                                #pragma multi_compile _ SHADOWS_SHADOWMASK
                                                                // GraphKeywords: <None>

                                                                // Defines
                                                                #define _SURFACE_TYPE_TRANSPARENT 1
                                                                #define _NORMALMAP 1
                                                                #define _NORMAL_DROPOFF_TS 1
                                                                #define ATTRIBUTES_NEED_NORMAL
                                                                #define ATTRIBUTES_NEED_TANGENT
                                                                #define ATTRIBUTES_NEED_TEXCOORD1
                                                                #define VARYINGS_NEED_POSITION_WS
                                                                #define VARYINGS_NEED_NORMAL_WS
                                                                #define VARYINGS_NEED_TANGENT_WS
                                                                #define VARYINGS_NEED_VIEWDIRECTION_WS
                                                                #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                                                                #define FEATURES_GRAPH_VERTEX
                                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                #define SHADERPASS SHADERPASS_FORWARD
                                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                                // Includes
                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                                                // --------------------------------------------------
                                                                // Structs and Packing

                                                                struct Attributes
                                                                {
                                                                    float3 positionOS : POSITION;
                                                                    float3 normalOS : NORMAL;
                                                                    float4 tangentOS : TANGENT;
                                                                    float4 uv1 : TEXCOORD1;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    uint instanceID : INSTANCEID_SEMANTIC;
                                                                    #endif
                                                                };
                                                                struct Varyings
                                                                {
                                                                    float4 positionCS : SV_POSITION;
                                                                    float3 positionWS;
                                                                    float3 normalWS;
                                                                    float4 tangentWS;
                                                                    float3 viewDirectionWS;
                                                                    #if defined(LIGHTMAP_ON)
                                                                    float2 lightmapUV;
                                                                    #endif
                                                                    #if !defined(LIGHTMAP_ON)
                                                                    float3 sh;
                                                                    #endif
                                                                    float4 fogFactorAndVertexLight;
                                                                    float4 shadowCoord;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                    #endif
                                                                };
                                                                struct SurfaceDescriptionInputs
                                                                {
                                                                    float3 TangentSpaceNormal;
                                                                    float3 WorldSpacePosition;
                                                                    float4 ScreenPosition;
                                                                };
                                                                struct VertexDescriptionInputs
                                                                {
                                                                    float3 ObjectSpaceNormal;
                                                                    float3 ObjectSpaceTangent;
                                                                    float3 ObjectSpacePosition;
                                                                };
                                                                struct PackedVaryings
                                                                {
                                                                    float4 positionCS : SV_POSITION;
                                                                    float3 interp0 : TEXCOORD0;
                                                                    float3 interp1 : TEXCOORD1;
                                                                    float4 interp2 : TEXCOORD2;
                                                                    float3 interp3 : TEXCOORD3;
                                                                    #if defined(LIGHTMAP_ON)
                                                                    float2 interp4 : TEXCOORD4;
                                                                    #endif
                                                                    #if !defined(LIGHTMAP_ON)
                                                                    float3 interp5 : TEXCOORD5;
                                                                    #endif
                                                                    float4 interp6 : TEXCOORD6;
                                                                    float4 interp7 : TEXCOORD7;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                    #endif
                                                                };

                                                                PackedVaryings PackVaryings(Varyings input)
                                                                {
                                                                    PackedVaryings output;
                                                                    output.positionCS = input.positionCS;
                                                                    output.interp0.xyz = input.positionWS;
                                                                    output.interp1.xyz = input.normalWS;
                                                                    output.interp2.xyzw = input.tangentWS;
                                                                    output.interp3.xyz = input.viewDirectionWS;
                                                                    #if defined(LIGHTMAP_ON)
                                                                    output.interp4.xy = input.lightmapUV;
                                                                    #endif
                                                                    #if !defined(LIGHTMAP_ON)
                                                                    output.interp5.xyz = input.sh;
                                                                    #endif
                                                                    output.interp6.xyzw = input.fogFactorAndVertexLight;
                                                                    output.interp7.xyzw = input.shadowCoord;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    output.instanceID = input.instanceID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    output.cullFace = input.cullFace;
                                                                    #endif
                                                                    return output;
                                                                }
                                                                Varyings UnpackVaryings(PackedVaryings input)
                                                                {
                                                                    Varyings output;
                                                                    output.positionCS = input.positionCS;
                                                                    output.positionWS = input.interp0.xyz;
                                                                    output.normalWS = input.interp1.xyz;
                                                                    output.tangentWS = input.interp2.xyzw;
                                                                    output.viewDirectionWS = input.interp3.xyz;
                                                                    #if defined(LIGHTMAP_ON)
                                                                    output.lightmapUV = input.interp4.xy;
                                                                    #endif
                                                                    #if !defined(LIGHTMAP_ON)
                                                                    output.sh = input.interp5.xyz;
                                                                    #endif
                                                                    output.fogFactorAndVertexLight = input.interp6.xyzw;
                                                                    output.shadowCoord = input.interp7.xyzw;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    output.instanceID = input.instanceID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    output.cullFace = input.cullFace;
                                                                    #endif
                                                                    return output;
                                                                }

                                                                // --------------------------------------------------
                                                                // Graph

                                                                // Graph Properties
                                                                CBUFFER_START(UnityPerMaterial)
                                                                float2 _Position;
                                                                float _Size;
                                                                float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                CBUFFER_END

                                                                    // Object and Global properties

                                                                    // Graph Functions

                                                                    void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                                    {
                                                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                                    }

                                                                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                                    {
                                                                        Out = A + B;
                                                                    }

                                                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                                    {
                                                                        Out = UV * Tiling + Offset;
                                                                    }

                                                                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                                    {
                                                                        Out = A * B;
                                                                    }

                                                                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                                    {
                                                                        Out = A - B;
                                                                    }

                                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                                    {
                                                                        Out = A / B;
                                                                    }

                                                                    void Unity_Multiply_float(float A, float B, out float Out)
                                                                    {
                                                                        Out = A * B;
                                                                    }

                                                                    void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                                    {
                                                                        Out = A / B;
                                                                    }

                                                                    void Unity_Length_float2(float2 In, out float Out)
                                                                    {
                                                                        Out = length(In);
                                                                    }

                                                                    void Unity_OneMinus_float(float In, out float Out)
                                                                    {
                                                                        Out = 1 - In;
                                                                    }

                                                                    void Unity_Saturate_float(float In, out float Out)
                                                                    {
                                                                        Out = saturate(In);
                                                                    }

                                                                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                                    {
                                                                        Out = smoothstep(Edge1, Edge2, In);
                                                                    }

                                                                    // Graph Vertex
                                                                    struct VertexDescription
                                                                    {
                                                                        float3 Position;
                                                                        float3 Normal;
                                                                        float3 Tangent;
                                                                    };

                                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                    {
                                                                        VertexDescription description = (VertexDescription)0;
                                                                        description.Position = IN.ObjectSpacePosition;
                                                                        description.Normal = IN.ObjectSpaceNormal;
                                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                                        return description;
                                                                    }

                                                                    // Graph Pixel
                                                                    struct SurfaceDescription
                                                                    {
                                                                        float3 BaseColor;
                                                                        float3 NormalTS;
                                                                        float3 Emission;
                                                                        float Metallic;
                                                                        float Smoothness;
                                                                        float Occlusion;
                                                                        float Alpha;
                                                                    };

                                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                    {
                                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                                        float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                        float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                        float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                        float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                        Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                        float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                        Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                        float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                        Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                        float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                        Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                        float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                        Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                        float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                        float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                        Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                        float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                        Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                        float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                        float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                        Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                        float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                        Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                        float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                        Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                        float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                        Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                        float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                        Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                        float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                        float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                        Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                        float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                        Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                        surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                                                                        surface.NormalTS = IN.TangentSpaceNormal;
                                                                        surface.Emission = float3(0, 0, 0);
                                                                        surface.Metallic = 0;
                                                                        surface.Smoothness = 0.5;
                                                                        surface.Occlusion = 1;
                                                                        surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                        return surface;
                                                                    }

                                                                    // --------------------------------------------------
                                                                    // Build Graph Inputs

                                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                    {
                                                                        VertexDescriptionInputs output;
                                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                        output.ObjectSpaceNormal = input.normalOS;
                                                                        output.ObjectSpaceTangent = input.tangentOS;
                                                                        output.ObjectSpacePosition = input.positionOS;

                                                                        return output;
                                                                    }

                                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                    {
                                                                        SurfaceDescriptionInputs output;
                                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                                                                        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                                                                        output.WorldSpacePosition = input.positionWS;
                                                                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                    #else
                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                    #endif
                                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                        return output;
                                                                    }


                                                                    // --------------------------------------------------
                                                                    // Main

                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

                                                                    ENDHLSL
                                                                }
                                                                Pass
                                                                {
                                                                    Name "ShadowCaster"
                                                                    Tags
                                                                    {
                                                                        "LightMode" = "ShadowCaster"
                                                                    }

                                                                        // Render State
                                                                        Cull Back
                                                                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                                        ZTest LEqual
                                                                        ZWrite On
                                                                        ColorMask 0

                                                                        // Debug
                                                                        // <None>

                                                                        // --------------------------------------------------
                                                                        // Pass

                                                                        HLSLPROGRAM

                                                                        // Pragmas
                                                                        #pragma target 2.0
                                                                        #pragma only_renderers gles gles3 glcore
                                                                        #pragma multi_compile_instancing
                                                                        #pragma vertex vert
                                                                        #pragma fragment frag

                                                                        // DotsInstancingOptions: <None>
                                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                                        // Keywords
                                                                        // PassKeywords: <None>
                                                                        // GraphKeywords: <None>

                                                                        // Defines
                                                                        #define _SURFACE_TYPE_TRANSPARENT 1
                                                                        #define _NORMALMAP 1
                                                                        #define _NORMAL_DROPOFF_TS 1
                                                                        #define ATTRIBUTES_NEED_NORMAL
                                                                        #define ATTRIBUTES_NEED_TANGENT
                                                                        #define VARYINGS_NEED_POSITION_WS
                                                                        #define FEATURES_GRAPH_VERTEX
                                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                        #define SHADERPASS SHADERPASS_SHADOWCASTER
                                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                                        // Includes
                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                                                        // --------------------------------------------------
                                                                        // Structs and Packing

                                                                        struct Attributes
                                                                        {
                                                                            float3 positionOS : POSITION;
                                                                            float3 normalOS : NORMAL;
                                                                            float4 tangentOS : TANGENT;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            uint instanceID : INSTANCEID_SEMANTIC;
                                                                            #endif
                                                                        };
                                                                        struct Varyings
                                                                        {
                                                                            float4 positionCS : SV_POSITION;
                                                                            float3 positionWS;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                            #endif
                                                                        };
                                                                        struct SurfaceDescriptionInputs
                                                                        {
                                                                            float3 WorldSpacePosition;
                                                                            float4 ScreenPosition;
                                                                        };
                                                                        struct VertexDescriptionInputs
                                                                        {
                                                                            float3 ObjectSpaceNormal;
                                                                            float3 ObjectSpaceTangent;
                                                                            float3 ObjectSpacePosition;
                                                                        };
                                                                        struct PackedVaryings
                                                                        {
                                                                            float4 positionCS : SV_POSITION;
                                                                            float3 interp0 : TEXCOORD0;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                            #endif
                                                                        };

                                                                        PackedVaryings PackVaryings(Varyings input)
                                                                        {
                                                                            PackedVaryings output;
                                                                            output.positionCS = input.positionCS;
                                                                            output.interp0.xyz = input.positionWS;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            output.instanceID = input.instanceID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            output.cullFace = input.cullFace;
                                                                            #endif
                                                                            return output;
                                                                        }
                                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                                        {
                                                                            Varyings output;
                                                                            output.positionCS = input.positionCS;
                                                                            output.positionWS = input.interp0.xyz;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            output.instanceID = input.instanceID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            output.cullFace = input.cullFace;
                                                                            #endif
                                                                            return output;
                                                                        }

                                                                        // --------------------------------------------------
                                                                        // Graph

                                                                        // Graph Properties
                                                                        CBUFFER_START(UnityPerMaterial)
                                                                        float2 _Position;
                                                                        float _Size;
                                                                        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                        float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                        CBUFFER_END

                                                                            // Object and Global properties

                                                                            // Graph Functions

                                                                            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                                            {
                                                                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                                            }

                                                                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                                            {
                                                                                Out = A + B;
                                                                            }

                                                                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                                            {
                                                                                Out = UV * Tiling + Offset;
                                                                            }

                                                                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                                            {
                                                                                Out = A * B;
                                                                            }

                                                                            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                                            {
                                                                                Out = A - B;
                                                                            }

                                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                                            {
                                                                                Out = A / B;
                                                                            }

                                                                            void Unity_Multiply_float(float A, float B, out float Out)
                                                                            {
                                                                                Out = A * B;
                                                                            }

                                                                            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                                            {
                                                                                Out = A / B;
                                                                            }

                                                                            void Unity_Length_float2(float2 In, out float Out)
                                                                            {
                                                                                Out = length(In);
                                                                            }

                                                                            void Unity_OneMinus_float(float In, out float Out)
                                                                            {
                                                                                Out = 1 - In;
                                                                            }

                                                                            void Unity_Saturate_float(float In, out float Out)
                                                                            {
                                                                                Out = saturate(In);
                                                                            }

                                                                            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                                            {
                                                                                Out = smoothstep(Edge1, Edge2, In);
                                                                            }

                                                                            // Graph Vertex
                                                                            struct VertexDescription
                                                                            {
                                                                                float3 Position;
                                                                                float3 Normal;
                                                                                float3 Tangent;
                                                                            };

                                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                            {
                                                                                VertexDescription description = (VertexDescription)0;
                                                                                description.Position = IN.ObjectSpacePosition;
                                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                                return description;
                                                                            }

                                                                            // Graph Pixel
                                                                            struct SurfaceDescription
                                                                            {
                                                                                float Alpha;
                                                                            };

                                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                            {
                                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                return surface;
                                                                            }

                                                                            // --------------------------------------------------
                                                                            // Build Graph Inputs

                                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                            {
                                                                                VertexDescriptionInputs output;
                                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                                output.ObjectSpaceTangent = input.tangentOS;
                                                                                output.ObjectSpacePosition = input.positionOS;

                                                                                return output;
                                                                            }

                                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                            {
                                                                                SurfaceDescriptionInputs output;
                                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                                                                output.WorldSpacePosition = input.positionWS;
                                                                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                            #else
                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                            #endif
                                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                return output;
                                                                            }


                                                                            // --------------------------------------------------
                                                                            // Main

                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                                                                            ENDHLSL
                                                                        }
                                                                        Pass
                                                                        {
                                                                            Name "DepthOnly"
                                                                            Tags
                                                                            {
                                                                                "LightMode" = "DepthOnly"
                                                                            }

                                                                                // Render State
                                                                                Cull Back
                                                                                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                                                ZTest LEqual
                                                                                ZWrite On
                                                                                ColorMask 0

                                                                                // Debug
                                                                                // <None>

                                                                                // --------------------------------------------------
                                                                                // Pass

                                                                                HLSLPROGRAM

                                                                                // Pragmas
                                                                                #pragma target 2.0
                                                                                #pragma only_renderers gles gles3 glcore
                                                                                #pragma multi_compile_instancing
                                                                                #pragma vertex vert
                                                                                #pragma fragment frag

                                                                                // DotsInstancingOptions: <None>
                                                                                // HybridV1InjectedBuiltinProperties: <None>

                                                                                // Keywords
                                                                                // PassKeywords: <None>
                                                                                // GraphKeywords: <None>

                                                                                // Defines
                                                                                #define _SURFACE_TYPE_TRANSPARENT 1
                                                                                #define _NORMALMAP 1
                                                                                #define _NORMAL_DROPOFF_TS 1
                                                                                #define ATTRIBUTES_NEED_NORMAL
                                                                                #define ATTRIBUTES_NEED_TANGENT
                                                                                #define VARYINGS_NEED_POSITION_WS
                                                                                #define FEATURES_GRAPH_VERTEX
                                                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                #define SHADERPASS SHADERPASS_DEPTHONLY
                                                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                                                // Includes
                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                                                                // --------------------------------------------------
                                                                                // Structs and Packing

                                                                                struct Attributes
                                                                                {
                                                                                    float3 positionOS : POSITION;
                                                                                    float3 normalOS : NORMAL;
                                                                                    float4 tangentOS : TANGENT;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    uint instanceID : INSTANCEID_SEMANTIC;
                                                                                    #endif
                                                                                };
                                                                                struct Varyings
                                                                                {
                                                                                    float4 positionCS : SV_POSITION;
                                                                                    float3 positionWS;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                    #endif
                                                                                };
                                                                                struct SurfaceDescriptionInputs
                                                                                {
                                                                                    float3 WorldSpacePosition;
                                                                                    float4 ScreenPosition;
                                                                                };
                                                                                struct VertexDescriptionInputs
                                                                                {
                                                                                    float3 ObjectSpaceNormal;
                                                                                    float3 ObjectSpaceTangent;
                                                                                    float3 ObjectSpacePosition;
                                                                                };
                                                                                struct PackedVaryings
                                                                                {
                                                                                    float4 positionCS : SV_POSITION;
                                                                                    float3 interp0 : TEXCOORD0;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                    #endif
                                                                                };

                                                                                PackedVaryings PackVaryings(Varyings input)
                                                                                {
                                                                                    PackedVaryings output;
                                                                                    output.positionCS = input.positionCS;
                                                                                    output.interp0.xyz = input.positionWS;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    output.instanceID = input.instanceID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    output.cullFace = input.cullFace;
                                                                                    #endif
                                                                                    return output;
                                                                                }
                                                                                Varyings UnpackVaryings(PackedVaryings input)
                                                                                {
                                                                                    Varyings output;
                                                                                    output.positionCS = input.positionCS;
                                                                                    output.positionWS = input.interp0.xyz;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    output.instanceID = input.instanceID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    output.cullFace = input.cullFace;
                                                                                    #endif
                                                                                    return output;
                                                                                }

                                                                                // --------------------------------------------------
                                                                                // Graph

                                                                                // Graph Properties
                                                                                CBUFFER_START(UnityPerMaterial)
                                                                                float2 _Position;
                                                                                float _Size;
                                                                                float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                CBUFFER_END

                                                                                    // Object and Global properties

                                                                                    // Graph Functions

                                                                                    void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                                                    {
                                                                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                                                    }

                                                                                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                                                    {
                                                                                        Out = A + B;
                                                                                    }

                                                                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                                                    {
                                                                                        Out = UV * Tiling + Offset;
                                                                                    }

                                                                                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                                                    {
                                                                                        Out = A * B;
                                                                                    }

                                                                                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                                                    {
                                                                                        Out = A - B;
                                                                                    }

                                                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                                                    {
                                                                                        Out = A / B;
                                                                                    }

                                                                                    void Unity_Multiply_float(float A, float B, out float Out)
                                                                                    {
                                                                                        Out = A * B;
                                                                                    }

                                                                                    void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                                                    {
                                                                                        Out = A / B;
                                                                                    }

                                                                                    void Unity_Length_float2(float2 In, out float Out)
                                                                                    {
                                                                                        Out = length(In);
                                                                                    }

                                                                                    void Unity_OneMinus_float(float In, out float Out)
                                                                                    {
                                                                                        Out = 1 - In;
                                                                                    }

                                                                                    void Unity_Saturate_float(float In, out float Out)
                                                                                    {
                                                                                        Out = saturate(In);
                                                                                    }

                                                                                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                                                    {
                                                                                        Out = smoothstep(Edge1, Edge2, In);
                                                                                    }

                                                                                    // Graph Vertex
                                                                                    struct VertexDescription
                                                                                    {
                                                                                        float3 Position;
                                                                                        float3 Normal;
                                                                                        float3 Tangent;
                                                                                    };

                                                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                    {
                                                                                        VertexDescription description = (VertexDescription)0;
                                                                                        description.Position = IN.ObjectSpacePosition;
                                                                                        description.Normal = IN.ObjectSpaceNormal;
                                                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                                                        return description;
                                                                                    }

                                                                                    // Graph Pixel
                                                                                    struct SurfaceDescription
                                                                                    {
                                                                                        float Alpha;
                                                                                    };

                                                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                    {
                                                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                                                        float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                        float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                                        float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                                        float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                                        Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                                        float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                                        Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                                        float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                                        Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                                        float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                                        Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                                        float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                                        Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                                        float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                                        float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                                        Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                                        float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                                        Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                                        float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                                        float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                                        Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                                        float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                                        Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                                        float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                                        Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                                        float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                                        Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                                        float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                                        Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                                        float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                        float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                                        Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                                        float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                        Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                                        surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                        return surface;
                                                                                    }

                                                                                    // --------------------------------------------------
                                                                                    // Build Graph Inputs

                                                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                    {
                                                                                        VertexDescriptionInputs output;
                                                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                        output.ObjectSpaceNormal = input.normalOS;
                                                                                        output.ObjectSpaceTangent = input.tangentOS;
                                                                                        output.ObjectSpacePosition = input.positionOS;

                                                                                        return output;
                                                                                    }

                                                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                    {
                                                                                        SurfaceDescriptionInputs output;
                                                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                                                                        output.WorldSpacePosition = input.positionWS;
                                                                                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                    #else
                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                    #endif
                                                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                        return output;
                                                                                    }


                                                                                    // --------------------------------------------------
                                                                                    // Main

                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                                                                                    ENDHLSL
                                                                                }
                                                                                Pass
                                                                                {
                                                                                    Name "DepthNormals"
                                                                                    Tags
                                                                                    {
                                                                                        "LightMode" = "DepthNormals"
                                                                                    }

                                                                                        // Render State
                                                                                        Cull Back
                                                                                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                                                        ZTest LEqual
                                                                                        ZWrite On

                                                                                        // Debug
                                                                                        // <None>

                                                                                        // --------------------------------------------------
                                                                                        // Pass

                                                                                        HLSLPROGRAM

                                                                                        // Pragmas
                                                                                        #pragma target 2.0
                                                                                        #pragma only_renderers gles gles3 glcore
                                                                                        #pragma multi_compile_instancing
                                                                                        #pragma vertex vert
                                                                                        #pragma fragment frag

                                                                                        // DotsInstancingOptions: <None>
                                                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                                                        // Keywords
                                                                                        // PassKeywords: <None>
                                                                                        // GraphKeywords: <None>

                                                                                        // Defines
                                                                                        #define _SURFACE_TYPE_TRANSPARENT 1
                                                                                        #define _NORMALMAP 1
                                                                                        #define _NORMAL_DROPOFF_TS 1
                                                                                        #define ATTRIBUTES_NEED_NORMAL
                                                                                        #define ATTRIBUTES_NEED_TANGENT
                                                                                        #define ATTRIBUTES_NEED_TEXCOORD1
                                                                                        #define VARYINGS_NEED_POSITION_WS
                                                                                        #define VARYINGS_NEED_NORMAL_WS
                                                                                        #define VARYINGS_NEED_TANGENT_WS
                                                                                        #define FEATURES_GRAPH_VERTEX
                                                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                                                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                                                        // Includes
                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                                                                        // --------------------------------------------------
                                                                                        // Structs and Packing

                                                                                        struct Attributes
                                                                                        {
                                                                                            float3 positionOS : POSITION;
                                                                                            float3 normalOS : NORMAL;
                                                                                            float4 tangentOS : TANGENT;
                                                                                            float4 uv1 : TEXCOORD1;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            uint instanceID : INSTANCEID_SEMANTIC;
                                                                                            #endif
                                                                                        };
                                                                                        struct Varyings
                                                                                        {
                                                                                            float4 positionCS : SV_POSITION;
                                                                                            float3 positionWS;
                                                                                            float3 normalWS;
                                                                                            float4 tangentWS;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                            #endif
                                                                                        };
                                                                                        struct SurfaceDescriptionInputs
                                                                                        {
                                                                                            float3 TangentSpaceNormal;
                                                                                            float3 WorldSpacePosition;
                                                                                            float4 ScreenPosition;
                                                                                        };
                                                                                        struct VertexDescriptionInputs
                                                                                        {
                                                                                            float3 ObjectSpaceNormal;
                                                                                            float3 ObjectSpaceTangent;
                                                                                            float3 ObjectSpacePosition;
                                                                                        };
                                                                                        struct PackedVaryings
                                                                                        {
                                                                                            float4 positionCS : SV_POSITION;
                                                                                            float3 interp0 : TEXCOORD0;
                                                                                            float3 interp1 : TEXCOORD1;
                                                                                            float4 interp2 : TEXCOORD2;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                            #endif
                                                                                        };

                                                                                        PackedVaryings PackVaryings(Varyings input)
                                                                                        {
                                                                                            PackedVaryings output;
                                                                                            output.positionCS = input.positionCS;
                                                                                            output.interp0.xyz = input.positionWS;
                                                                                            output.interp1.xyz = input.normalWS;
                                                                                            output.interp2.xyzw = input.tangentWS;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            output.instanceID = input.instanceID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            output.cullFace = input.cullFace;
                                                                                            #endif
                                                                                            return output;
                                                                                        }
                                                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                                                        {
                                                                                            Varyings output;
                                                                                            output.positionCS = input.positionCS;
                                                                                            output.positionWS = input.interp0.xyz;
                                                                                            output.normalWS = input.interp1.xyz;
                                                                                            output.tangentWS = input.interp2.xyzw;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            output.instanceID = input.instanceID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            output.cullFace = input.cullFace;
                                                                                            #endif
                                                                                            return output;
                                                                                        }

                                                                                        // --------------------------------------------------
                                                                                        // Graph

                                                                                        // Graph Properties
                                                                                        CBUFFER_START(UnityPerMaterial)
                                                                                        float2 _Position;
                                                                                        float _Size;
                                                                                        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                        float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                        CBUFFER_END

                                                                                            // Object and Global properties

                                                                                            // Graph Functions

                                                                                            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                                                            {
                                                                                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                                                            }

                                                                                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                                                            {
                                                                                                Out = A + B;
                                                                                            }

                                                                                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                                                            {
                                                                                                Out = UV * Tiling + Offset;
                                                                                            }

                                                                                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                                                            {
                                                                                                Out = A * B;
                                                                                            }

                                                                                            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                                                            {
                                                                                                Out = A - B;
                                                                                            }

                                                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                                                            {
                                                                                                Out = A / B;
                                                                                            }

                                                                                            void Unity_Multiply_float(float A, float B, out float Out)
                                                                                            {
                                                                                                Out = A * B;
                                                                                            }

                                                                                            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                                                            {
                                                                                                Out = A / B;
                                                                                            }

                                                                                            void Unity_Length_float2(float2 In, out float Out)
                                                                                            {
                                                                                                Out = length(In);
                                                                                            }

                                                                                            void Unity_OneMinus_float(float In, out float Out)
                                                                                            {
                                                                                                Out = 1 - In;
                                                                                            }

                                                                                            void Unity_Saturate_float(float In, out float Out)
                                                                                            {
                                                                                                Out = saturate(In);
                                                                                            }

                                                                                            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                                                            {
                                                                                                Out = smoothstep(Edge1, Edge2, In);
                                                                                            }

                                                                                            // Graph Vertex
                                                                                            struct VertexDescription
                                                                                            {
                                                                                                float3 Position;
                                                                                                float3 Normal;
                                                                                                float3 Tangent;
                                                                                            };

                                                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                            {
                                                                                                VertexDescription description = (VertexDescription)0;
                                                                                                description.Position = IN.ObjectSpacePosition;
                                                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                                                return description;
                                                                                            }

                                                                                            // Graph Pixel
                                                                                            struct SurfaceDescription
                                                                                            {
                                                                                                float3 NormalTS;
                                                                                                float Alpha;
                                                                                            };

                                                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                            {
                                                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                                                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                                                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                                                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                                                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                                                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                                                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                                                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                                                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                                                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                                                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                                                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                                                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                                                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                                                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                                                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                                                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                                                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                                                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                                                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                                                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                                                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                                                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                                                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                                                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                                                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                                                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                                                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                                                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                                                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                                                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                                                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                                                surface.NormalTS = IN.TangentSpaceNormal;
                                                                                                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                                return surface;
                                                                                            }

                                                                                            // --------------------------------------------------
                                                                                            // Build Graph Inputs

                                                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                            {
                                                                                                VertexDescriptionInputs output;
                                                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                                                output.ObjectSpaceTangent = input.tangentOS;
                                                                                                output.ObjectSpacePosition = input.positionOS;

                                                                                                return output;
                                                                                            }

                                                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                            {
                                                                                                SurfaceDescriptionInputs output;
                                                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                                                                                                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                                                                                                output.WorldSpacePosition = input.positionWS;
                                                                                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                            #else
                                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                            #endif
                                                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                                return output;
                                                                                            }


                                                                                            // --------------------------------------------------
                                                                                            // Main

                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

                                                                                            ENDHLSL
                                                                                        }
                                                                                        Pass
                                                                                        {
                                                                                            Name "Meta"
                                                                                            Tags
                                                                                            {
                                                                                                "LightMode" = "Meta"
                                                                                            }

                                                                                                // Render State
                                                                                                Cull Off

                                                                                                // Debug
                                                                                                // <None>

                                                                                                // --------------------------------------------------
                                                                                                // Pass

                                                                                                HLSLPROGRAM

                                                                                                // Pragmas
                                                                                                #pragma target 2.0
                                                                                                #pragma only_renderers gles gles3 glcore
                                                                                                #pragma vertex vert
                                                                                                #pragma fragment frag

                                                                                                // DotsInstancingOptions: <None>
                                                                                                // HybridV1InjectedBuiltinProperties: <None>

                                                                                                // Keywords
                                                                                                #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                                                                                                // GraphKeywords: <None>

                                                                                                // Defines
                                                                                                #define _SURFACE_TYPE_TRANSPARENT 1
                                                                                                #define _NORMALMAP 1
                                                                                                #define _NORMAL_DROPOFF_TS 1
                                                                                                #define ATTRIBUTES_NEED_NORMAL
                                                                                                #define ATTRIBUTES_NEED_TANGENT
                                                                                                #define ATTRIBUTES_NEED_TEXCOORD1
                                                                                                #define ATTRIBUTES_NEED_TEXCOORD2
                                                                                                #define VARYINGS_NEED_POSITION_WS
                                                                                                #define FEATURES_GRAPH_VERTEX
                                                                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                                #define SHADERPASS SHADERPASS_META
                                                                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                                                                // Includes
                                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

                                                                                                // --------------------------------------------------
                                                                                                // Structs and Packing

                                                                                                struct Attributes
                                                                                                {
                                                                                                    float3 positionOS : POSITION;
                                                                                                    float3 normalOS : NORMAL;
                                                                                                    float4 tangentOS : TANGENT;
                                                                                                    float4 uv1 : TEXCOORD1;
                                                                                                    float4 uv2 : TEXCOORD2;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    uint instanceID : INSTANCEID_SEMANTIC;
                                                                                                    #endif
                                                                                                };
                                                                                                struct Varyings
                                                                                                {
                                                                                                    float4 positionCS : SV_POSITION;
                                                                                                    float3 positionWS;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                                    #endif
                                                                                                };
                                                                                                struct SurfaceDescriptionInputs
                                                                                                {
                                                                                                    float3 WorldSpacePosition;
                                                                                                    float4 ScreenPosition;
                                                                                                };
                                                                                                struct VertexDescriptionInputs
                                                                                                {
                                                                                                    float3 ObjectSpaceNormal;
                                                                                                    float3 ObjectSpaceTangent;
                                                                                                    float3 ObjectSpacePosition;
                                                                                                };
                                                                                                struct PackedVaryings
                                                                                                {
                                                                                                    float4 positionCS : SV_POSITION;
                                                                                                    float3 interp0 : TEXCOORD0;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                                    #endif
                                                                                                };

                                                                                                PackedVaryings PackVaryings(Varyings input)
                                                                                                {
                                                                                                    PackedVaryings output;
                                                                                                    output.positionCS = input.positionCS;
                                                                                                    output.interp0.xyz = input.positionWS;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    output.instanceID = input.instanceID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    output.cullFace = input.cullFace;
                                                                                                    #endif
                                                                                                    return output;
                                                                                                }
                                                                                                Varyings UnpackVaryings(PackedVaryings input)
                                                                                                {
                                                                                                    Varyings output;
                                                                                                    output.positionCS = input.positionCS;
                                                                                                    output.positionWS = input.interp0.xyz;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    output.instanceID = input.instanceID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    output.cullFace = input.cullFace;
                                                                                                    #endif
                                                                                                    return output;
                                                                                                }

                                                                                                // --------------------------------------------------
                                                                                                // Graph

                                                                                                // Graph Properties
                                                                                                CBUFFER_START(UnityPerMaterial)
                                                                                                float2 _Position;
                                                                                                float _Size;
                                                                                                float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                                float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                                CBUFFER_END

                                                                                                    // Object and Global properties

                                                                                                    // Graph Functions

                                                                                                    void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                                                                    {
                                                                                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                                                                    }

                                                                                                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                                                                    {
                                                                                                        Out = A + B;
                                                                                                    }

                                                                                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                                                                    {
                                                                                                        Out = UV * Tiling + Offset;
                                                                                                    }

                                                                                                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                                                                    {
                                                                                                        Out = A * B;
                                                                                                    }

                                                                                                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                                                                    {
                                                                                                        Out = A - B;
                                                                                                    }

                                                                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                                                                    {
                                                                                                        Out = A / B;
                                                                                                    }

                                                                                                    void Unity_Multiply_float(float A, float B, out float Out)
                                                                                                    {
                                                                                                        Out = A * B;
                                                                                                    }

                                                                                                    void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                                                                    {
                                                                                                        Out = A / B;
                                                                                                    }

                                                                                                    void Unity_Length_float2(float2 In, out float Out)
                                                                                                    {
                                                                                                        Out = length(In);
                                                                                                    }

                                                                                                    void Unity_OneMinus_float(float In, out float Out)
                                                                                                    {
                                                                                                        Out = 1 - In;
                                                                                                    }

                                                                                                    void Unity_Saturate_float(float In, out float Out)
                                                                                                    {
                                                                                                        Out = saturate(In);
                                                                                                    }

                                                                                                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                                                                    {
                                                                                                        Out = smoothstep(Edge1, Edge2, In);
                                                                                                    }

                                                                                                    // Graph Vertex
                                                                                                    struct VertexDescription
                                                                                                    {
                                                                                                        float3 Position;
                                                                                                        float3 Normal;
                                                                                                        float3 Tangent;
                                                                                                    };

                                                                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                                    {
                                                                                                        VertexDescription description = (VertexDescription)0;
                                                                                                        description.Position = IN.ObjectSpacePosition;
                                                                                                        description.Normal = IN.ObjectSpaceNormal;
                                                                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                                                                        return description;
                                                                                                    }

                                                                                                    // Graph Pixel
                                                                                                    struct SurfaceDescription
                                                                                                    {
                                                                                                        float3 BaseColor;
                                                                                                        float3 Emission;
                                                                                                        float Alpha;
                                                                                                    };

                                                                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                                    {
                                                                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                                                                        float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                                        float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                                                        float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                                                        float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                                                        Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                                                        float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                                                        Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                                                        float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                                                        Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                                                        float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                                                        Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                                                        float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                                                        Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                                                        float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                                                        float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                                                        Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                                                        float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                                                        Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                                                        float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                                                        float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                                                        Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                                                        float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                                                        Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                                                        float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                                                        Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                                                        float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                                                        Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                                                        float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                                                        Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                                                        float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                                        float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                                                        Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                                                        float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                                        Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                                                        surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                                                                                                        surface.Emission = float3(0, 0, 0);
                                                                                                        surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                                        return surface;
                                                                                                    }

                                                                                                    // --------------------------------------------------
                                                                                                    // Build Graph Inputs

                                                                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                                    {
                                                                                                        VertexDescriptionInputs output;
                                                                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                                        output.ObjectSpaceNormal = input.normalOS;
                                                                                                        output.ObjectSpaceTangent = input.tangentOS;
                                                                                                        output.ObjectSpacePosition = input.positionOS;

                                                                                                        return output;
                                                                                                    }

                                                                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                                    {
                                                                                                        SurfaceDescriptionInputs output;
                                                                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                                                                                        output.WorldSpacePosition = input.positionWS;
                                                                                                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                                    #else
                                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                                    #endif
                                                                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                                        return output;
                                                                                                    }


                                                                                                    // --------------------------------------------------
                                                                                                    // Main

                                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                                                                                                    ENDHLSL
                                                                                                }
                                                                                                Pass
                                                                                                {
                                                                                                        // Name: <None>
                                                                                                        Tags
                                                                                                        {
                                                                                                            "LightMode" = "Universal2D"
                                                                                                        }

                                                                                                        // Render State
                                                                                                        Cull Back
                                                                                                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                                                                        ZTest LEqual
                                                                                                        ZWrite Off

                                                                                                        // Debug
                                                                                                        // <None>

                                                                                                        // --------------------------------------------------
                                                                                                        // Pass

                                                                                                        HLSLPROGRAM

                                                                                                        // Pragmas
                                                                                                        #pragma target 2.0
                                                                                                        #pragma only_renderers gles gles3 glcore
                                                                                                        #pragma multi_compile_instancing
                                                                                                        #pragma vertex vert
                                                                                                        #pragma fragment frag

                                                                                                        // DotsInstancingOptions: <None>
                                                                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                                                                        // Keywords
                                                                                                        // PassKeywords: <None>
                                                                                                        // GraphKeywords: <None>

                                                                                                        // Defines
                                                                                                        #define _SURFACE_TYPE_TRANSPARENT 1
                                                                                                        #define _NORMALMAP 1
                                                                                                        #define _NORMAL_DROPOFF_TS 1
                                                                                                        #define ATTRIBUTES_NEED_NORMAL
                                                                                                        #define ATTRIBUTES_NEED_TANGENT
                                                                                                        #define VARYINGS_NEED_POSITION_WS
                                                                                                        #define FEATURES_GRAPH_VERTEX
                                                                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                                        #define SHADERPASS SHADERPASS_2D
                                                                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                                                                                        // Includes
                                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                                                                                        // --------------------------------------------------
                                                                                                        // Structs and Packing

                                                                                                        struct Attributes
                                                                                                        {
                                                                                                            float3 positionOS : POSITION;
                                                                                                            float3 normalOS : NORMAL;
                                                                                                            float4 tangentOS : TANGENT;
                                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                            uint instanceID : INSTANCEID_SEMANTIC;
                                                                                                            #endif
                                                                                                        };
                                                                                                        struct Varyings
                                                                                                        {
                                                                                                            float4 positionCS : SV_POSITION;
                                                                                                            float3 positionWS;
                                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                                            #endif
                                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                                            #endif
                                                                                                        };
                                                                                                        struct SurfaceDescriptionInputs
                                                                                                        {
                                                                                                            float3 WorldSpacePosition;
                                                                                                            float4 ScreenPosition;
                                                                                                        };
                                                                                                        struct VertexDescriptionInputs
                                                                                                        {
                                                                                                            float3 ObjectSpaceNormal;
                                                                                                            float3 ObjectSpaceTangent;
                                                                                                            float3 ObjectSpacePosition;
                                                                                                        };
                                                                                                        struct PackedVaryings
                                                                                                        {
                                                                                                            float4 positionCS : SV_POSITION;
                                                                                                            float3 interp0 : TEXCOORD0;
                                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                            uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                                            #endif
                                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                                            #endif
                                                                                                        };

                                                                                                        PackedVaryings PackVaryings(Varyings input)
                                                                                                        {
                                                                                                            PackedVaryings output;
                                                                                                            output.positionCS = input.positionCS;
                                                                                                            output.interp0.xyz = input.positionWS;
                                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                            output.instanceID = input.instanceID;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                                            #endif
                                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                            output.cullFace = input.cullFace;
                                                                                                            #endif
                                                                                                            return output;
                                                                                                        }
                                                                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                                                                        {
                                                                                                            Varyings output;
                                                                                                            output.positionCS = input.positionCS;
                                                                                                            output.positionWS = input.interp0.xyz;
                                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                            output.instanceID = input.instanceID;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                                            #endif
                                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                                            #endif
                                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                            output.cullFace = input.cullFace;
                                                                                                            #endif
                                                                                                            return output;
                                                                                                        }

                                                                                                        // --------------------------------------------------
                                                                                                        // Graph

                                                                                                        // Graph Properties
                                                                                                        CBUFFER_START(UnityPerMaterial)
                                                                                                        float2 _Position;
                                                                                                        float _Size;
                                                                                                        float Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                                        float Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                                        CBUFFER_END

                                                                                                            // Object and Global properties

                                                                                                            // Graph Functions

                                                                                                            void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
                                                                                                            {
                                                                                                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                                                                                            }

                                                                                                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                                                                                                            {
                                                                                                                Out = A + B;
                                                                                                            }

                                                                                                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                                                                                            {
                                                                                                                Out = UV * Tiling + Offset;
                                                                                                            }

                                                                                                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                                                                                                            {
                                                                                                                Out = A * B;
                                                                                                            }

                                                                                                            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                                                                                                            {
                                                                                                                Out = A - B;
                                                                                                            }

                                                                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                                                                            {
                                                                                                                Out = A / B;
                                                                                                            }

                                                                                                            void Unity_Multiply_float(float A, float B, out float Out)
                                                                                                            {
                                                                                                                Out = A * B;
                                                                                                            }

                                                                                                            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
                                                                                                            {
                                                                                                                Out = A / B;
                                                                                                            }

                                                                                                            void Unity_Length_float2(float2 In, out float Out)
                                                                                                            {
                                                                                                                Out = length(In);
                                                                                                            }

                                                                                                            void Unity_OneMinus_float(float In, out float Out)
                                                                                                            {
                                                                                                                Out = 1 - In;
                                                                                                            }

                                                                                                            void Unity_Saturate_float(float In, out float Out)
                                                                                                            {
                                                                                                                Out = saturate(In);
                                                                                                            }

                                                                                                            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                                                                                            {
                                                                                                                Out = smoothstep(Edge1, Edge2, In);
                                                                                                            }

                                                                                                            // Graph Vertex
                                                                                                            struct VertexDescription
                                                                                                            {
                                                                                                                float3 Position;
                                                                                                                float3 Normal;
                                                                                                                float3 Tangent;
                                                                                                            };

                                                                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                                            {
                                                                                                                VertexDescription description = (VertexDescription)0;
                                                                                                                description.Position = IN.ObjectSpacePosition;
                                                                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                                                                return description;
                                                                                                            }

                                                                                                            // Graph Pixel
                                                                                                            struct SurfaceDescription
                                                                                                            {
                                                                                                                float3 BaseColor;
                                                                                                                float Alpha;
                                                                                                            };

                                                                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                                            {
                                                                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                                                                float _Property_f01714a44b50410ea05733ea73758322_Out_0 = Vector1_4c8346c3e1d04caaa3e99436a21c6463;
                                                                                                                float4 _ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                                                                                                float2 _Property_12ea34893ea540b881ea66061d3f9586_Out_0 = _Position;
                                                                                                                float2 _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3;
                                                                                                                Unity_Remap_float2(_Property_12ea34893ea540b881ea66061d3f9586_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3);
                                                                                                                float2 _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2;
                                                                                                                Unity_Add_float2((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), _Remap_9cad6624493c4a78aff2a9ace21a59c7_Out_3, _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2);
                                                                                                                float2 _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3;
                                                                                                                Unity_TilingAndOffset_float((_ScreenPosition_321c8186ada14b0abc2df569f5183ba0_Out_0.xy), float2 (1, 1), _Add_d0de7731e1fd45d487446de0e6ac43ba_Out_2, _TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3);
                                                                                                                float2 _Multiply_e666c0b97200494985b0d7835d831541_Out_2;
                                                                                                                Unity_Multiply_float(_TilingAndOffset_18f7807c08bc4599a2233e9b9aec19ab_Out_3, float2(2, 2), _Multiply_e666c0b97200494985b0d7835d831541_Out_2);
                                                                                                                float2 _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2;
                                                                                                                Unity_Subtract_float2(_Multiply_e666c0b97200494985b0d7835d831541_Out_2, float2(1, 1), _Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2);
                                                                                                                float _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0 = _Size;
                                                                                                                float _Divide_8816d7dce5264539a956e53452d2263e_Out_2;
                                                                                                                Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_8816d7dce5264539a956e53452d2263e_Out_2);
                                                                                                                float _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2;
                                                                                                                Unity_Multiply_float(_Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0, _Divide_8816d7dce5264539a956e53452d2263e_Out_2, _Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2);
                                                                                                                float2 _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0 = float2(_Multiply_51ff5959a9fb45d78607387d63f1c49a_Out_2, _Property_4d08cbfa7c1d4136a3a205db63a6d926_Out_0);
                                                                                                                float2 _Divide_461b161746194952a28baa2f19779fdc_Out_2;
                                                                                                                Unity_Divide_float2(_Subtract_6a2032f1b11249a8aea5ee82aa92e658_Out_2, _Vector2_1e114214c30f4d5d9d8e15c2165110a2_Out_0, _Divide_461b161746194952a28baa2f19779fdc_Out_2);
                                                                                                                float _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1;
                                                                                                                Unity_Length_float2(_Divide_461b161746194952a28baa2f19779fdc_Out_2, _Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1);
                                                                                                                float _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1;
                                                                                                                Unity_OneMinus_float(_Length_7c71a3ca2ced4d7ea01cba7544e208b1_Out_1, _OneMinus_02287b4636394df692fe5c6d153c4362_Out_1);
                                                                                                                float _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1;
                                                                                                                Unity_Saturate_float(_OneMinus_02287b4636394df692fe5c6d153c4362_Out_1, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1);
                                                                                                                float _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3;
                                                                                                                Unity_Smoothstep_float(0, _Property_f01714a44b50410ea05733ea73758322_Out_0, _Saturate_3bbf69af232f405fa2d95f8ac809f293_Out_1, _Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3);
                                                                                                                float _Property_3229bb0450b644c2884895119c58638c_Out_0 = Vector1_aeb586a2664d4d429e0412ff5b266119;
                                                                                                                float _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2;
                                                                                                                Unity_Multiply_float(_Smoothstep_b4994894e52a43a0be9e586cda46cda1_Out_3, _Property_3229bb0450b644c2884895119c58638c_Out_0, _Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2);
                                                                                                                float _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                                                Unity_OneMinus_float(_Multiply_4d82a87fa34c42279b405cae4ec4b66e_Out_2, _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1);
                                                                                                                surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
                                                                                                                surface.Alpha = _OneMinus_67d08c27f9e54b99a3353ecd75bc843d_Out_1;
                                                                                                                return surface;
                                                                                                            }

                                                                                                            // --------------------------------------------------
                                                                                                            // Build Graph Inputs

                                                                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                                            {
                                                                                                                VertexDescriptionInputs output;
                                                                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                                                                output.ObjectSpaceTangent = input.tangentOS;
                                                                                                                output.ObjectSpacePosition = input.positionOS;

                                                                                                                return output;
                                                                                                            }

                                                                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                                            {
                                                                                                                SurfaceDescriptionInputs output;
                                                                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                                                                                                output.WorldSpacePosition = input.positionWS;
                                                                                                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                                            #else
                                                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                                            #endif
                                                                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                                                return output;
                                                                                                            }


                                                                                                            // --------------------------------------------------
                                                                                                            // Main

                                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                                                                                                            ENDHLSL
                                                                                                        }
                                                            }
                                                                CustomEditor "ShaderGraph.PBRMasterGUI"
                                                                                                                FallBack "Hidden/Shader Graph/FallbackError"
}

