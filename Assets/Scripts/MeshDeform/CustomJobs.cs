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

    public struct CreatePlaneJob : IJob
    {
        [ReadOnly] 
        public Vector2Int PlaneSize;
        
        [ReadOnly] 
        public int VertCount;

        [WriteOnly] 
        public MeshData MeshData;

        public void Execute()
        {
            var width  = (PlaneSize.x * PlaneSize.x) * .5f;
            var length = (PlaneSize.y * PlaneSize.y) * .5f;
            
            var xOffset = width / PlaneSize.x;
            var yOffset = length / PlaneSize.y;
            
           
            // for (int i = 0; i < VertCount ; i++)
            // {
            //     // var x = i % PlaneSize.x;
            //     // var z = i / PlaneSize.x;
            //     
            //   //  MeshData.Vertices[i] = new Vector3(width/2 + (xOffset * (i % PlaneSize.x)),0, length/2 + (-yOffset * (i / PlaneSize.y)));
            //   MeshData.Vertices[i] =  new Vector3(x,0, y);
            // }
            
            for (int i = 0, y = 0; y <= PlaneSize.y; y++)
            {
                for (int x = 0; x <= PlaneSize.x; x++, i++) 
                {
                    MeshData.Vertices[i] = new Vector3(x,0, y);
                }
            }
            
            
            for (int ti = 0, vi = 0, y = 0; y < PlaneSize.y; y++, vi++) 
            {
                for (int x = 0; x < PlaneSize.x; x++, ti += 6, vi++) 
                {
                     MeshData.Triangles[ti] = vi;
                     MeshData.Triangles[ti + 3] =  MeshData.Triangles[ti + 2] = vi + 1;
                     MeshData.Triangles[ti + 4] =  MeshData.Triangles[ti + 1] = vi + PlaneSize.x + 1;
                     MeshData.Triangles[ti + 5] = vi + PlaneSize.x + 2;
                }
            }
        }
    }
    
    [BurstCompile]
    public struct WavePlaneJob : IJobParallelFor
    {
        public NativeArray<float3> Vertices;

        [ReadOnly] 
        public float WaveStrength;

        [ReadOnly] 
        public float WaveSpeed;

        [ReadOnly] 
        public float Time;
            
        public void Execute(int index)
        {
            var vertex = Vertices[index];
            float sinOff=(vertex.x+vertex.y+vertex.z) * WaveStrength;
            float t=Time * WaveSpeed;
                
            float fx = vertex.x;
            float fy = vertex.x* vertex.y;
            
          //  vertex.y =  math.cos((vertex.x + t)) *  vertex.x * WaveStrength;
            
            // vertex.x += math.sin(t*1.45f+sinOff) *0.5f;
            // vertex.y =  math.sin(t*3.12f+sinOff) *0.5f;
            // vertex.z -= math.sin(t*2.2f +sinOff) *0.2f;

            
            
          
            float3 dir = new float3(1,0,0) * vertex;
            vertex.y += Mathf.Sin(t+ dir.x + dir.y + dir.z) ;
            Vertices[index] = vertex;
            
            
            
            Vertices[index] = vertex;
        }
    }
   
}
