using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GeometryTest
{
    public struct MeshData
    {
        public NativeArray<Vector3> Vertices;
        public NativeArray<int> Triangles;
        public NativeArray<Vector3> Normals;
        public NativeArray<Vector2> Uv;
        public NativeArray<Color> Colors;
        
        public void Dispose()
        {
           Vertices.Dispose();
           Triangles.Dispose();
           Normals.Dispose();
           Uv.Dispose();
           Colors.Dispose();
        }
    }
    
    [Serializable]
    public struct Octave 
    {
        public float Scale;
        public float Speed;
        public float Height;
    }
    
    public class FlagBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Octave[] _octaves;
        
        [SerializeField] 
        private Vector2Int _planeSize = new Vector2Int(10, 10);
        
        [SerializeField] 
        private MeshFilter _meshFilter;

        [SerializeField] 
        private Color _meshColor = Color.white;
        
        private MeshData _meshData;
        private JobHandle _jobHandle;
        private Mesh _mesh;

        private void Awake()
        {
            var quadCount = (_planeSize.x - 1) * (_planeSize.y - 1);
            var vertexCount = quadCount * 4;
            var triangleCount = vertexCount * 6;

            _meshData.Vertices = new NativeArray<Vector3>(vertexCount, Allocator.Persistent);
            _meshData.Triangles = new NativeArray<int>(triangleCount, Allocator.Persistent);
            _meshData.Normals = new NativeArray<Vector3>(vertexCount, Allocator.Persistent);
            _meshData.Uv = new NativeArray<Vector2>(vertexCount, Allocator.Persistent);
            _meshData.Colors = new NativeArray<Color>(vertexCount, Allocator.Persistent);

            var job = new CreatePlaneJob
            {
                MeshData = _meshData,
                PlaneSize = _planeSize,
                QuadCount = quadCount,
                MeshColor = _meshColor
            };

            _jobHandle = job.Schedule();
        }

        private void Start()
        {
            _mesh = new Mesh();
            _mesh.MarkDynamic();
            _mesh.name = "Plane Mesh";
            
            _jobHandle.Complete();
            BuildMesh();
        }

        private void BuildMesh()
        {
            _mesh.SetVertices(_meshData.Vertices);
            _mesh.SetTriangles(_meshData.Triangles.ToArray(),0);
            _mesh.SetNormals(_meshData.Normals);
            _mesh.SetUVs(0,_meshData.Uv);
            _mesh.SetColors(_meshData.Colors);
            _mesh.RecalculateNormals();
            // _mesh.vertices = _meshData.Vertices.ToArray();
            // _mesh.triangles = _meshData.Triangles.ToArray();
            // _mesh.normals = _meshData.Normals.ToArray();
            // _mesh.uv = _meshData.Uv.ToArray();
            // _mesh.colors = _meshData.Colors.ToArray();
            _meshFilter.sharedMesh = _mesh;
        }

        private void Update()
        {
            var jobHandles = new List<JobHandle>();
            
            var vertCount = _meshData.Vertices.Length;
            
            for (var i = 0; i <vertCount; i++)
            {
                _meshData.Vertices[i] = new Vector3(_meshData.Vertices[i].x,0,_meshData.Vertices[i].z);
            }
            
            for (int i = 0; i < _octaves.Length; i++)
            {
                var noiseJob = new PerlinNoiseJob
                {
                    Vertices = _meshData.Vertices,
                    Octave = _octaves[i],
                    Time = Time.timeSinceLevelLoad
                };
                 jobHandles.Add(noiseJob.Schedule(vertCount, 256, i == 0 ? new JobHandle() : jobHandles[i - 1]));
            }
            
            jobHandles.Last().Complete();
        
            BuildMesh();
        }


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
