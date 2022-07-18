using System.Collections.Generic;
using Feofun.Extension;
using UnityEngine;

namespace Dino.Units.Component
{
    public class RaycastFovRenderer : MonoBehaviour
    {
        [Header("Vision")]
        public float _angle = 30f;
        public float _farRadius = 5f;
        public float _nearRadius = 3f;
        public LayerMask _obstacleMask = ~(0);
        public bool _showTwoLevels = false;

        [Header("Material")]
        public Material _nearConeMaterial;
        public Material _farConeMaterial;
        public int _sortOrder = 1;

        [Header("Optimization")]
        public int _precision = 60;
        public float _refreshRate = 0f;

        private MeshRenderer _nearRenderer;
        private MeshFilter _nearMeshFilter;
        private MeshRenderer _farRenderer;
        private MeshFilter _farMeshFilter;
        private float _timer = 0f;

        private void Awake()
        {
            InitConeRenderer(gameObject, _nearConeMaterial, out _nearRenderer, out _nearMeshFilter);

            if (_showTwoLevels)
            {
                var farCone = new GameObject("FarCone");
                farCone.transform.SetParent(gameObject.transform);
                farCone.transform.ResetLocalTransform();
                
                InitConeRenderer(farCone, _farConeMaterial, out _farRenderer, out _farMeshFilter);
            }
        }

        private void InitConeRenderer(GameObject obj, Material material, out MeshRenderer renderer, out MeshFilter meshFilter)
        {
            renderer = obj.AddComponent<MeshRenderer>();
            meshFilter = obj.AddComponent<MeshFilter>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            renderer.allowOcclusionWhenDynamic = false;
            renderer.sortingOrder = _sortOrder;
        }
        
        private void Start()
        {
            InitMesh(_nearMeshFilter, false);

            if (_showTwoLevels)
                InitMesh(_farMeshFilter, true);
        }

        private void InitMesh(MeshFilter mesh, bool far)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();

            if (!far)
            {
                vertices.Add(new Vector3(0f, 0f, 0f));
                normals.Add(Vector3.up);
                uv.Add(Vector2.zero);
            }

            var minmax = Mathf.RoundToInt(_angle / 2f);

            var triIndex = 0;
            var stepJump = Mathf.Clamp(_angle / _precision, 0.01f, minmax);

            for (float i = -minmax; i <= minmax; i += stepJump)
            {
                var angle = (float)(i + 90f) * Mathf.Deg2Rad;
                var dir = new Vector3(Mathf.Cos(angle) * _farRadius, 0f, Mathf.Sin(angle) * _farRadius);

                vertices.Add(dir);
                normals.Add(Vector2.up);
                uv.Add(Vector2.zero);

                if (far)
                {
                    vertices.Add(dir);
                    normals.Add(Vector2.up);
                    uv.Add(Vector2.zero);
                }

                if (triIndex > 0)
                {
                    if (far)
                    {
                        triangles.Add(triIndex);
                        triangles.Add(triIndex+1);
                        triangles.Add(triIndex-2);

                        triangles.Add(triIndex - 2);
                        triangles.Add(triIndex + 1);
                        triangles.Add(triIndex - 1);
                    }
                    else
                    {
                        triangles.Add(0);
                        triangles.Add(triIndex + 1);
                        triangles.Add(triIndex);
                    }
                }
                triIndex += far ? 2 : 1;
            }

            mesh.mesh.vertices = vertices.ToArray();
            mesh.mesh.triangles = triangles.ToArray();
            mesh.mesh.normals = normals.ToArray();
            mesh.mesh.uv = uv.ToArray();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > _refreshRate)
            {
                _timer = 0f;

                float range = _farRadius;
                if (_showTwoLevels)
                    range = _nearRadius;

                UpdateMainLevel(_nearMeshFilter, range);

                if (_showTwoLevels)
                    UpdateFarLevel(_farMeshFilter, _nearRadius, _farRadius - _nearRadius);
            }
        }

        private void UpdateMainLevel(MeshFilter mesh, float range)
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(new Vector3(0f, 0f, 0f));

            int minmax = Mathf.RoundToInt(_angle / 2f);
            float step_jump = Mathf.Clamp(_angle / _precision, 0.01f, minmax);
            for (float i = -minmax; i <= minmax; i += step_jump)
            {
                float angle = (float)(i + 90f) * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(angle) * range, 0f, Mathf.Sin(angle) * range);

                RaycastHit hit;
                Vector3 pos_world = transform.TransformPoint(Vector3.zero);
                Vector3 dir_world = transform.TransformDirection(dir.normalized);
                bool ishit = Physics.Raycast(new Ray(pos_world, dir_world), out hit, range, _obstacleMask.value);
                if (ishit)
                    dir = dir.normalized * hit.distance;
                Debug.DrawRay(pos_world, dir_world * (ishit ? hit.distance : range));

                vertices.Add(dir);
            }

            mesh.mesh.vertices = vertices.ToArray();
            mesh.mesh.RecalculateBounds();
        }

        private void UpdateFarLevel(MeshFilter mesh, float offset, float range)
        {
            List<Vector3> vertices = new List<Vector3>();

            int minmax = Mathf.RoundToInt(_angle / 2f);
            float step_jump = Mathf.Clamp(_angle / _precision, 0.01f, minmax);
            for (float i = -minmax; i <= minmax; i += step_jump)
            {
                float angle = (float)(i + 90f) * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(angle) * offset, 0f, Mathf.Sin(angle) * offset);

                RaycastHit hit;
                Vector3 pos_world = transform.TransformPoint(Vector3.zero);
                Vector3 dir_world = transform.TransformDirection(dir.normalized);
                bool ishit = Physics.Raycast(new Ray(pos_world, dir_world), out hit, range + offset, _obstacleMask.value);

                float tot_dist = ishit ? hit.distance : range + offset;
                Vector3 dir1 = dir.normalized * offset;
                Vector3 dir2 = dir.normalized * Mathf.Max(tot_dist, offset);

                Debug.DrawRay(pos_world + dir_world * offset, dir_world * Mathf.Max(tot_dist - offset, 0f), Color.blue);

                vertices.Add(dir1);
                vertices.Add(dir2);
            }

            mesh.mesh.vertices = vertices.ToArray();
            mesh.mesh.RecalculateBounds();
        }
    }
}