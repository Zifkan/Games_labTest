Shader "Custom/FlagShader"
{
    Properties
     {
      _Color ("Main Color", Color) = (1,1,1,1)
     _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
     _MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
     _BumpMap ("Normalmap", 2D) = "bump" {}  
     
     _WaveSpeed ("Wave Speed", Range(0.0, 300.0)) = 50.0
     _WaveStrength ("Wave Strength", Range(0.0, 5.0)) = 1.0
     
     _ScrollXSpeed("X", Range(0,10)) = 2
     _ScrollYSpeed("Y", Range(0,10)) = 3
     _time("_time", Range(0,10)) = 3
    }   
   
    SubShader 
    {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      
      #include "Wave_cginc.cginc"
      
    
      struct Input 
      {
          float2 uv_MainTex;
      };
      
    
      
      v2f vert (inout appdata_full v) 
      {
            v2f o;
            computeWave(v, o);
            return o;
       }
       
       
       fixed _ScrollXSpeed;
       fixed _ScrollYSpeed;
      
      
    
      
      void surf(Input IN, inout SurfaceOutput o) {
         fixed2 scrolledUV = IN.uv_MainTex;
 
         fixed xScrollValue = _ScrollXSpeed * _time;
         fixed yScrollValue = _ScrollYSpeed * _time;
 
         scrolledUV += fixed2(xScrollValue, yScrollValue);
         
         o.Albedo = tex2D(_MainTex, scrolledUV).rgb;
 
       
     }
      ENDCG
    } 
    Fallback "Diffuse"
  }