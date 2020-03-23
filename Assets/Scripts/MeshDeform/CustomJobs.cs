using Mesh.GeometryTest;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshDeform
{
    [BurstCompile]
    public struct PerlinNoiseJob : IJobParallelFor
    {
        public NativeArray<float3> Vertices;

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

    [BurstCompile]
    public struct CreatePlaneJob : IJob
    {
        [ReadOnly] 
        public Vector2Int PlaneSize;
        
        public MeshData MeshData;

        public void Execute()
        {
            float width  = PlaneSize.x;
            float length = PlaneSize.y;
            
            float xOffset = width / PlaneSize.x;
            float yOffset = length / PlaneSize.y;
            
           
            for (int i = 0; i < MeshData.Vertices.Length ; i++)
            {
                var vert =  new Vector3(-width/2 + (xOffset * (i % (PlaneSize.x+1))),0, -length/2 + (yOffset * (i / (PlaneSize.x+1))));
                MeshData.Vertices[i] = vert;
                MeshData.BaseVertices[i] = vert;

                MeshData.UVs[i] = new Vector2((vert.x + width/2)/width, (vert.z + length/2)/length);
            }

            for (int ti = 0, vi = 0, y = 0; y < PlaneSize.y; y++, vi++) 
            {
                for (int x = 0; x < PlaneSize.x; x++, ti += 6, vi++) 
                {
                     MeshData.Triangles[ti ] = vi;
                     MeshData.Triangles[ti + 1] = vi + PlaneSize.x+ 1;
                     MeshData.Triangles[ti+ 2] = vi  + 1;
                     
                     MeshData.Triangles[ti + 3] = vi  + 1;
                     MeshData.Triangles[ti + 4] = vi + PlaneSize.x+ 1;
                     MeshData.Triangles[ti + 5] = vi + PlaneSize.x + 2;
                }
            }
        }
    }
    
    [BurstCompile]
    public struct WavePlaneJob : IJobParallelFor
    {
        public MeshData MeshData;

        [ReadOnly] 
        public float WaveStrength;

        [ReadOnly] 
        public float WaveSpeed;

        [ReadOnly] 
        public float Time;
            
        public void Execute(int index)
        {
            var vertex = MeshData.BaseVertices[index];
            var uv = MeshData.UVs[index];
            
            float sinOff=(vertex.x+vertex.y+vertex.z) * WaveStrength;
            float t=-Time * WaveSpeed;
                
            float fx = uv.x;
            float fy = uv.x * uv.y;
     
            vertex.x += math.sin(t*1.45f + sinOff) * fx * 0.5f;
            vertex.y =  math.sin(t*3.12f + sinOff) * fx * 0.5f-fy * 0.9f;
            vertex.z -= math.sin(t*2.2f  + sinOff) * fx * 0.2f;
            
            MeshData.Vertices[index] = vertex;
        }
    }
    
    
    [BurstCompile]
    public struct RefreshJob : IJobParallelFor
    {
        [ReadOnly] 
        public Vector2Int PlaneSize;
        
        public NativeArray<float3> Vertices;
        
        public void Execute(int i)
        {
            var width  = (PlaneSize.x * PlaneSize.x);
            var length = (PlaneSize.y * PlaneSize.y);
            
            var xOffset = width / PlaneSize.x;
            var yOffset = length / PlaneSize.y;
            
            
            var vert =  new Vector3(-width/2 + (xOffset * (i % (PlaneSize.x+1))),0, length/2 + (-yOffset * (i / (PlaneSize.x+1))));
            Vertices[i] = vert;
        }
    }
}
