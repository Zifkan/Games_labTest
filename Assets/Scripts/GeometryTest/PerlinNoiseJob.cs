using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GeometryTest
{
    public struct PerlinNoiseJob : IJobParallelFor
    {
        public NativeArray<Vector3> Vertices;

        [DeallocateOnJobCompletion] [ReadOnly]
        public Octave Octave;

        public float Time;

        public void Execute(int i)
        {            
            var vertex = Vertices[i];
            var x = vertex.x * Octave.Scale + Time * Octave.Speed;
            var z = vertex.z * Octave.Scale + Time * Octave.Speed;
            vertex.y += (Mathf.PerlinNoise(x, z) - 0.5f) * Octave.Height;
            Vertices[i] = vertex;
        }
    }

    public struct CreatePlaneJob : IJob
    {
        [ReadOnly] 
        public Vector2Int PlaneSize;
        
        [ReadOnly] 
        public int QuadCount;

        [WriteOnly] 
        public MeshData MeshData;

        [WriteOnly] 
        public Color MeshColor;
        
        public void Execute()
        {
            var halfWidth  = PlaneSize.x /10; //(PlaneSize.x * PlaneSize.x) * .5f;
            var halfLength = PlaneSize.y /10; //(PlaneSize.y * PlaneSize.y) * .5f;

            for (int i = 0; i < QuadCount-1; i++)
            {
                var x = i % PlaneSize.x;
                var z = i / PlaneSize.x;
 
                var left = (x * PlaneSize.x) - halfWidth;
                var right = (left + PlaneSize.x);
 
                var bottom = (z * PlaneSize.y) - halfLength;
                var top = (bottom + PlaneSize.y);
 
                var v = i * 4;

                MeshData.Vertices[v + 0] = new Vector3(left, 0, bottom);
                MeshData.Vertices[v + 1] = new Vector3(left, 0, top);
                MeshData.Vertices[v + 2] = new Vector3(right, 0, top);
                MeshData.Vertices[v + 3] = new Vector3(right, 0, bottom);

                var t = i * 6;

                MeshData.Triangles[t + 0] = v + 0;
                MeshData.Triangles[t + 1] = v + 1;
                MeshData.Triangles[t + 2] = v + 2;
                MeshData.Triangles[t + 3] = v + 2;
                MeshData.Triangles[t + 4] = v + 3;
                MeshData.Triangles[t + 5] = v + 0;

                MeshData.Normals[v + 0] = Vector3.up;
                MeshData.Normals[v + 1] = Vector3.up;
                MeshData.Normals[v + 2] = Vector3.up;
                MeshData.Normals[v + 3] = Vector3.up;

                MeshData.Uv[v + 0] = new Vector2(0, 0);
                MeshData.Uv[v + 1] = new Vector2(0, 1);
                MeshData.Uv[v + 2] = new Vector2(1, 1);
                MeshData.Uv[v + 3] = new Vector2(1, 0);

                MeshData.Colors[v + 0] = MeshColor;
                MeshData.Colors[v + 1] = MeshColor;
                MeshData.Colors[v + 2] = MeshColor;
                MeshData.Colors[v + 3] = MeshColor;
            }
        }
    }
   
}
