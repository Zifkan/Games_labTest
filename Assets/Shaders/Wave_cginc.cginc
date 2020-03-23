// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
#ifndef YAKOV_FLAG_VAWE
#define YAKOV_FLAG_VAWE

#include "UnityCG.cginc"
 
float4 _Color;
sampler2D _MainTex;
fixed _Cutoff;
float _WaveSpeed;
float _WaveStrength;
float _time;
 
struct v2f {
   V2F_SHADOW_CASTER;
    float2 uv : TEXCOORD1;
};
 
 
void computeWave (inout appdata_full v, inout v2f o)
{
    float sinOff=(v.vertex.x+v.vertex.y+v.vertex.z) * _WaveStrength;
    float t=-_time*_WaveSpeed;
    float fx=v.texcoord.x;
    float fy=v.texcoord.x*v.texcoord.y;
 
    v.vertex.x+=sin(t*1.45+sinOff)*fx*0.5;
    v.vertex.y=(sin(t*3.12+sinOff)*fx*0.5-fy*0.9);
    v.vertex.z-=(sin(t*2.2+sinOff)*fx*0.2);
    o.pos = UnityObjectToClipPos( v.vertex );
    o.uv = v.texcoord;
}

#endif 