using Unity.Collections;
using Unity.Mathematics;

namespace MeshDeform
{
    public struct MeshData
    {
        public NativeArray<float3> BaseVertices;
        public NativeArray<float3> Vertices;
        public NativeArray<int> Triangles;
        public NativeArray<float2> UVs;
        
        public void Dispose()
        {
            Vertices.Dispose();
            Triangles.Dispose();
            UVs.Dispose();
            BaseVertices.Dispose();
        }
    }
}