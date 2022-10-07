using Feofun.Extension;
using UnityEngine;

namespace Dino.Units.Component
{
    // source https://forum.unity.com/threads/how-to-make-a-mesh-for-fov.152317/ 

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class StaticConeFovRenderer : MonoBehaviour, IFieldOfViewRenderer
    {
        private static readonly Vector2 TOP_LEFT_CORNER = new Vector2(0,1);
        private static readonly Vector2 TOP_RIGHT_CORNER = new Vector2(1,1);
        
        private const int MIN_SEGMENTS_COUNT = 2;
        [SerializeField] private Material _material;
        [SerializeField] private float _degreesPerSegment = 3;

        protected int _segmentsCount;
        protected float _angle;
        protected float _radius;
        protected Mesh _mesh;
        private Renderer _renderer;

        public Material Material
        {
            get => _material;
            set
            {
                _material = value;
                _renderer.material = value;
            }
        }
        
        protected virtual void Awake()
        {
            InitMesh();
        }

        private void InitMesh()
        {
            _renderer = gameObject.RequireComponent<MeshRenderer>();
            _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            _renderer.receiveShadows = false;
            _renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            _renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            _renderer.allowOcclusionWhenDynamic = false;
            _renderer.material = _material;

            var meshFilter = gameObject.RequireComponent<MeshFilter>();
            _mesh = meshFilter.mesh;
        }

        public void Init(float angle, float radius)
        {
            _segmentsCount = CalculateSegments(angle);
            _angle = angle;
            _radius = radius;

            _mesh.Clear();
            _mesh.vertices = BuildVertices(_segmentsCount, angle, radius);
            _mesh.triangles = BuildTriangles(_segmentsCount);
            _mesh.normals = BuildNormals(_segmentsCount);
            _mesh.uv = BuildUvs(_mesh.vertices);
        }

        private int CalculateSegments(float angle)
        {
            return (int) Mathf.Max(MIN_SEGMENTS_COUNT, angle / _degreesPerSegment);
        }

        protected Vector3[] BuildVertices(int segmentsCount, float angle, float radius)
        {
            var coneEdgeVertices = new Vector3[segmentsCount + 1];
            var segmentAngle = angle / segmentsCount;
            var currentAngle = -angle / 2;
            for (int i = coneEdgeVertices.Length - 1; i >= 0; i -= 1)
            {
                coneEdgeVertices[i] = GetVertPositionAtAngle(currentAngle, radius);
                currentAngle += segmentAngle;
            }

            var vertices = new Vector3[segmentsCount * 3];
            for (int i = vertices.Length; i >= 1; i -= 3)
            {
                vertices[i - 1] = coneEdgeVertices[i / 3]; 
                vertices[i - 2] = coneEdgeVertices[(i / 3) - 1]; 
            }
            return vertices;
        }
        
        protected virtual Vector3 GetVertPositionAtAngle(float angle, float radius)
        {
            var x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            var z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            return new Vector3(x, 0, z);
        }

        private int[] BuildTriangles(int segmentsCount)
        {
            var triangles = new int[segmentsCount * 3];
            for (int i = 0; i < triangles.Length; i += 3)
            {
                triangles[i] = 0;
                triangles[i + 1] = i + 2;
                triangles[i + 2] = i + 1;
            }
            return triangles;
        }

        private Vector3[] BuildNormals(int segmentsCount)
        {
            var normals = new Vector3[segmentsCount * 3];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.up;
            }
            return normals;
        }

        private Vector2[] BuildUvs(Vector3[] vertices)
        {
            var uvs = new Vector2[vertices.Length];
            uvs[1] = TOP_LEFT_CORNER;
            for (int i = 2; i < uvs.Length - 1; i++)
            {
                uvs[i] = new Vector2((float) i / uvs.Length, 1);
            }
            uvs[vertices.Length - 1] = TOP_RIGHT_CORNER;
            return uvs;
        }
    }
}