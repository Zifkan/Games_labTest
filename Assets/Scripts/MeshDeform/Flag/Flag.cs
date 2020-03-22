using System;
using System.Linq;
using Mesh.GeometryTest;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshDeform.Flag
{
    public class Flag : MonoBehaviour
    {
        [SerializeField] 
        private Vector2Int _planeSize = new Vector2Int(10, 10);
        
        [SerializeField] 
        private MeshFilter _meshFilter;
      
        [SerializeField] 
        private float _waveSpeed;
        
        [SerializeField] 
        private float _waveStrength;
        
        private MeshData _meshData;
        private JobHandle _jobHandle;
        private UnityEngine.Mesh _mesh;

        private void Awake()
        {
            var vertexCount = (_planeSize.x+1) *( _planeSize.y+1);
            var triangleCount = _planeSize.x  * _planeSize.y  *6;

            _meshData.Vertices = new NativeArray<float3>(vertexCount, Allocator.Persistent);
            _meshData.Triangles = new NativeArray<int>(triangleCount, Allocator.Persistent);
            _meshData.Uv = new NativeArray<Vector2>(vertexCount, Allocator.Persistent);

            var job = new CreatePlaneJob
            {
                MeshData = _meshData,
                PlaneSize = _planeSize,
                VertCount = vertexCount,
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
            _mesh.SetTriangles(_meshData.Triangles.ToList(),0);
            _mesh.SetUVs(0,_meshData.Uv);
            _mesh.RecalculateNormals();
            _meshFilter.sharedMesh = _mesh;
        }

        private void Update()
        {
            // var job = new WavePlaneJob
            // {
            //   Time = Time.deltaTime,
            //   WaveSpeed = _waveSpeed,
            //   WaveStrength = _waveStrength,
            //   Vertices =  _meshData.Vertices
            // };

            var job = new PerlinNoiseJob
            {
                Octave = new Octave{Speed = _waveSpeed, Height = 1, Scale = 1},
                Time = Time.deltaTime,
                Vertices =  _meshData.Vertices,
            };
            
            _jobHandle =  job.Schedule(_meshData.Vertices.Length, 32);
            
            _jobHandle.Complete();
            
            _mesh.SetVertices(_meshData.Vertices);
        }

        private void OnDisable()
        {
            _meshData.Dispose();
        }
    }
}