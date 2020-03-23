using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshDeform.Flag
{
    enum TypeCalculation
    {
        CPU,
        GPU,
    }
    public class Flag : MonoBehaviour
    {
        [SerializeField] 
        private Vector2Int _planeSize = new Vector2Int(10, 10);
        
        [SerializeField] 
        private MeshFilter _meshFilter;
      
        [SerializeField] 
        private MeshRenderer _meshRenderer;
        
        [SerializeField] 
        private float _waveSpeed;
        
        [SerializeField] 
        private float _waveStrength;

        [SerializeField] 
        private TypeCalculation _calc;

        [SerializeField]
        private Material _cpuMat;
        
        [SerializeField]
        private Material _gpuMat;

        
        private MeshData _meshData;
        private JobHandle _jobHandle;
        private UnityEngine.Mesh _mesh;

        private TypeCalculation _lastCalc;
        
        private void Awake()
        {
            var vertexCount = (_planeSize.x+1) *( _planeSize.y+1);
            var triangleCount = _planeSize.x  * _planeSize.y  *6;

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
            _mesh.SetTriangles(_meshData.Triangles.ToList(),0);
            _mesh.SetUVs(0,_meshData.Uv);
            _mesh.RecalculateNormals();
            _meshFilter.sharedMesh = _mesh;
        }

        private void Update()
        {
            SwitchCalc();

            if (_calc == TypeCalculation.CPU)
            {
                var job = new WavePlaneJob
                {
                    UVs = _meshData.Uv,
                    Time = Time.timeSinceLevelLoad,
                    WaveSpeed = _waveSpeed,
                    WaveStrength = _waveStrength,
                    Vertices = _meshData.Vertices
                };

                _jobHandle = job.Schedule(_meshData.Vertices.Length, 128,_jobHandle);
                _jobHandle.Complete();
            }

            _mesh.SetVertices(_meshData.Vertices);
            _mesh.RecalculateNormals();
            _meshFilter.sharedMesh = _mesh;
           
        }


        private void SwitchCalc()
        {
            if (_lastCalc == _calc) return ;
                
            if (_calc == TypeCalculation.CPU)
            {
                _meshRenderer.material = _cpuMat;
                
                var job = new RefreshJob()
                {
                    Vertices = _meshData.Vertices
                };
                    
                    
                _jobHandle = job.Schedule(_meshData.Vertices.Length, 128,_jobHandle);

                _jobHandle.Complete();
            }

            if (_calc == TypeCalculation.GPU)
            {
                _meshRenderer.material = _gpuMat;
            }
        }

        private void OnDisable()
        {
            _meshData.Dispose();
        }
    }
}