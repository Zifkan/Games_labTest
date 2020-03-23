Shader "Custom/Flag"
{
 
Properties
{   
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
    _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
    _MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}   
  
    _Cutoff ("Shadow Alpha cutoff", Range(0.25,0.9)) = 1.0 
   
    _WaveSpeed ("Wave Speed", Range(0.0, 300.0)) = 50.0
    _WaveStrength ("Wave Strength", Range(0.0, 5.0)) = 1.0
}
 
 
SubShader
{
    Tags {
    "Queue"="Geometry"
    "IgnoreProjector"="True"
    "RenderType"="Transparent"}
 
    LOD 300 
  
    Pass
    {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
           
            Fog {Mode Off}
            ZWrite On ZTest Less Cull Off
            Offset 1, 1
 
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #include "Wave_cginc.cginc"
           
           
            v2f vert( appdata_full v )
            {
                v2f o;
                computeWave(v, o);
                TRANSFER_SHADOW_CASTER(o)
   
              return o;
            }          
        
                   
            float4 frag( v2f i ) : COLOR
            {
                fixed4 texcol = tex2D( _MainTex, i.uv );
                clip( texcol.a - _Cutoff );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG 
    
    }
    
CGPROGRAM
        #pragma surface surf BlinnPhong alpha vertex:vert fullforwardshadows approxview
        #include "Wave_cginc.cginc"
 
 
        half _Shininess;
 
        sampler2D _BumpMap;
        float _Parallax;
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };
 
        v2f vert (inout appdata_full v) {
            v2f o;
            computeWave(v, o);
            return o;
        }
 
        void surf (Input IN, inout SurfaceOutput o) {
            
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = tex.rgb * _Color.rgb;
            o.Gloss = tex.a;
            o.Alpha = tex.a * _Color.a;
            o.Specular = _Shininess;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        }
ENDCG
}
   
Fallback "Transparent/VertexLit"
}