using System;
using System.Collections.Generic;
using System.Linq;
using MeshDeform;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Mesh.GeometryTest
{
    [Serializable]
    public struct Octave 
    {
        public float Scale;
        public float Speed;
        public float Height;
    }
    
    public class PlaneBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Octave[] _octaves;
        
        [SerializeField] 
        private Vector2Int _planeSize = new Vector2Int(10, 10);
        
        [SerializeField] 
        private MeshFilter _meshFilter;
        
        private MeshData _meshData;
        private JobHandle _jobHandle;
        private UnityEngine.Mesh _mesh;

        private void Awake()
        {
            var quadCount = (_planeSize.x - 1) * (_planeSize.y - 1);
            var vertexCount = quadCount * 4;
            var triangleCount = vertexCount * 6;

            _meshData.Vertices = new NativeArray<float3>(vertexCount, Allocator.Persistent);
            _meshData.Triangles = new NativeArray<int>(triangleCount, Allocator.Persistent);
            _meshData.Uv = new NativeArray<float2>(vertexCount, Allocator.Persistent);

            var job = new CreatePlaneJob
            {
                MeshData = _meshData,
                PlaneSize = _planeSize,
            };

            _jobHandle = job.Schedule();
        }

        private void Start()
        {
            _mesh = new UnityEngine.Mesh();
            _mesh.MarkDynamic();
            _mesh.name = "Plane Mesh";
            
            _jobHandle.Complete();
            BuildMesh();
        }

        private void BuildMesh()
        {
            _mesh.SetVertices(_meshData.Vertices);
            _mesh.SetTriangles(_meshData.Triangles.ToArray(),0);
            _mesh.SetUVs(0,_meshData.Uv);
            _mesh.RecalculateNormals();
            _meshFilter.sharedMesh = _mesh;
        }

        // private void Update()
        // {
        //     var jobHandles = new List<JobHandle>();
        //     
        //     var vertCount = _meshData.Vertices.Length;
        //     
        //     /*for (var i = 0; i <vertCount; i++)
        //     {
        //         _meshData.Vertices[i] = new Vector3(_meshData.Vertices[i].x,0,_meshData.Vertices[i].z);
        //     }*/
        //     
        //     for (int i = 0; i < _octaves.Length; i++)
        //     {
        //         var noiseJob = new PerlinNoiseJob
        //         {
        //             Vertices = _meshData.Vertices,
        //             Octave = _octaves[i],
        //             Time = Time.timeSinceLevelLoad
        //         };
        //         jobHandles.Add(noiseJob.Schedule(vertCount, 256, i == 0 ? new JobHandle() : jobHandles[i - 1]));
        //     }
        //     
        //     jobHandles.Last().Complete();
        //     _mesh.SetVertices(_meshData.Vertices);
        //     BuildMesh();
        // }


        private void OnDestroy()
        {
            if (!_jobHandle.IsCompleted)
            {
                _jobHandle.Complete();
            }
 
            _meshData.Dispose();
        }
    }
}
