Shader "Custom/SpecificThreadColorChanger"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color1 ("First Dye Color", Color) = (1,0,0,1)
        _Color1SatMult ("First Color Saturation Multiplier", Range(0.5, 2)) = 1
        _Color1ValMult ("First Color Value Multiplier", Range(0.5, 2)) = 1
        _Color2 ("Second Dye Color", Color) = (1,1,1,1)
        _Color2SatMult ("Second Color Saturation Multiplier", Range(0.5, 2)) = 1
        _Color2ValMult ("Second Color Value Multiplier", Range(0.5, 2)) = 1
        _UseSecondColor ("Use Second Color", Float) = 0
        _DefaultThreadColor ("Default Thread Color", Color) = (0.95, 0.93, 0.88, 1)
        _GlobalSatMult ("Global Saturation Multiplier", Range(0.5, 2)) = 1
        _GlobalValMult ("Global Value Multiplier", Range(0.5, 2)) = 1
        _Alpha ("Alpha", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _Color1;
            float4 _Color2;
            float _UseSecondColor;
            float4 _DefaultThreadColor;
            float _Alpha;

            // RGB to HSV 변환
            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            // HSV to RGB 변환
            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            bool IsRedColor(float3 color)
            {
                return color.r > 0.19 &&
                       color.r > color.g * 2.0 &&    
                       color.r > color.b * 1.5 &&
                       abs(color.g - color.b) < 0.05;
            }

            float3 MixColors(float3 color1, float3 color2)
            {
                float3 hsv1 = rgb2hsv(color1);
                float3 hsv2 = rgb2hsv(color2);
                float3 mixed = float3(
                    lerp(hsv1.x, hsv2.x, 0.5), // Hue
                    lerp(hsv1.y, hsv2.y, 0.5), // Saturation
                    lerp(hsv1.z, hsv2.z, 0.5)  // Value
                );
                return hsv2rgb(mixed);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _Color1SatMult;
            float _Color1ValMult;
            float _Color2SatMult;
            float _Color2ValMult;
            float _GlobalSatMult;
            float _GlobalValMult;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
        
                if(IsRedColor(col.rgb))
                {
                    float3 originalHSV = rgb2hsv(col.rgb);
                    float3 targetHSV = rgb2hsv(_DefaultThreadColor.rgb);
                    float satMult = _GlobalSatMult;
                    float valMult = _GlobalValMult;
            
                    if (_Color1.a > 0)
                    {
                        float3 dyeHSV;
                        if (_UseSecondColor > 0.5 && _Color2.a > 0)
                        {
                            float3 color1HSV = rgb2hsv(_Color1.rgb);
                            float3 color2HSV = rgb2hsv(_Color2.rgb);
                            dyeHSV = float3(
                                lerp(color1HSV.x, color2HSV.x, 0.5),
                                lerp(color1HSV.y * _Color1SatMult, color2HSV.y * _Color2SatMult, 0.5),
                                originalHSV.z
                            );
                            satMult = lerp(_Color1SatMult, _Color2SatMult, 0.5);
                            valMult = lerp(_Color1ValMult, _Color2ValMult, 0.5);
                        }
                        else
                        {
                            float3 dyeColorHSV = rgb2hsv(_Color1.rgb);
                            dyeHSV = float3(
                                dyeColorHSV.x,
                                dyeColorHSV.y * _Color1SatMult,
                                originalHSV.z
                            );
                            satMult = _Color1SatMult;
                            valMult = _Color1ValMult;
                        }
                
                        targetHSV = dyeHSV;
                    }
            
                    float3 finalHSV = float3(
                        targetHSV.x,
                        min(targetHSV.y * satMult * _GlobalSatMult, 1.0),
                        min(originalHSV.z * valMult * _GlobalValMult, 1.0)
                    );
            
                    col.rgb = hsv2rgb(finalHSV);
                }
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}