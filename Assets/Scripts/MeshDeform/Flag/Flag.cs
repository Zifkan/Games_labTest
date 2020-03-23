using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshDeform.Flag
{
    enum TypeCalculation
    {
        None,
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
        
        [SerializeField] [Range(0,300f)]
        private float _waveSpeed;
        
        [SerializeField] [Range(0,5f)]
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

        private TypeCalculation _lastCalc = TypeCalculation.None;
        private  MaterialPropertyBlock _mpb;

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
            var vertexCount = (_planeSize.x+1) *( _planeSize.y+1);
            var triangleCount = _planeSize.x  * _planeSize.y  *6;

            _meshData.Vertices = new NativeArray<float3>(vertexCount, Allocator.Persistent);
            _meshData.BaseVertices = new NativeArray<float3>(vertexCount, Allocator.Persistent);
            _meshData.Triangles = new NativeArray<int>(triangleCount, Allocator.Persistent);
            _meshData.UVs = new NativeArray<float2>(vertexCount, Allocator.Persistent);;
            
            
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
            _mesh.SetUVs(0,_meshData.UVs);
            _mesh.RecalculateNormals();
            _meshFilter.sharedMesh = _mesh;
            _meshRenderer.GetPropertyBlock(_mpb);
        }

        private void Update()
        {
            SwitchCalc();

            if (_calc == TypeCalculation.CPU)
            {
                _jobHandle = new WavePlaneJob
                {
                    Time = Time.timeSinceLevelLoad,
                    WaveSpeed = _waveSpeed,
                    WaveStrength = _waveStrength,
                    MeshData = _meshData
                }.Schedule(_meshData.Vertices.Length, 128,_jobHandle);
               
               
                _jobHandle.Complete();
            }

            if (_calc == TypeCalculation.GPU)
            {
                _meshRenderer.GetPropertyBlock(_mpb);
                _mpb.SetFloat("_WaveSpeed",_waveSpeed);
                _mpb.SetFloat("_WaveStrength",_waveStrength);
                _meshRenderer.SetPropertyBlock(_mpb);
            }

            _mesh.SetVertices(_meshData.Vertices);
            _mesh.RecalculateNormals();
        }


        private void SwitchCalc()
        {
            if (_lastCalc == _calc) return ;
                
            if (_calc == TypeCalculation.CPU)
            {
                _meshRenderer.material = _cpuMat;
            }

            if (_calc == TypeCalculation.GPU)
            {
                _meshRenderer.material = _gpuMat;
            }

            _lastCalc = _calc;
        }

        private void OnDestroy()
        {
            _meshData.Dispose();
        }
    }
}