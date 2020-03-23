using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MeshDeform
{
    public struct MeshData
    {
        public NativeArray<float3> Vertices;
        public NativeArray<int> Triangles;
        public NativeArray<float2> Uv;
        
        public void Dispose()
        {
            Vertices.Dispose();
            Triangles.Dispose();
            Uv.Dispose();
        }
    }
}