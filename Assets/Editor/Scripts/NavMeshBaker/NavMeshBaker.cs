using System.Collections.Generic;
using System.IO;
using System.Linq;
using SuperMaxim.Core.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Editor.Scripts.NavMeshBaker
{
    public class NavMeshBaker
    {
        private const string PREFAB_ASSETS_FILTER = "t:Prefab";
        private const string NAV_MESH_DATA_PROPERTY_NAME = "m_NavMeshData";

        public static void BakeAllNavMeshesInFolder(string folderPath)
        {
            GetAllAssetPathInFolder(folderPath, PREFAB_ASSETS_FILTER).ForEach(BakeNavMeshesInPrefab);
        }

        private static IEnumerable<string> GetAllAssetPathInFolder(string folderPath, string filter)
        {
            return AssetDatabase.FindAssets(filter, new[] {folderPath}).Select(AssetDatabase.GUIDToAssetPath);
        }

        private static void BakeNavMeshesInPrefab(string prefabAssetPath)
        {
            var prefab = PrefabUtility.LoadPrefabContents(prefabAssetPath);
            var prefabSurfaces = prefab.GetComponentsInChildren<NavMeshSurface>();

            if (prefabSurfaces.IsNullOrEmpty()) {
                PrefabUtility.UnloadPrefabContents(prefab);
                return;
            }
            BakeSurfaces(prefab, prefabSurfaces, prefabAssetPath);
        }

        private static void BakeSurfaces(GameObject prefab, NavMeshSurface[] prefabSurfaces, string prefabAssetPath)
        {
            var bakedNavMeshCount = 0;
            for (int index = 0; index < prefabSurfaces.Length; index++) {
                var surface = prefabSurfaces[index];
                var navMeshData = InitializeBakeData(surface);
                var updateNavMeshAsync = surface.UpdateNavMesh(navMeshData);
                
                void OnNavMeshAsyncUpdated(AsyncOperation operation)
                {
                    updateNavMeshAsync.completed -= OnNavMeshAsyncUpdated;
                    UpdateNavMeshData(surface, navMeshData);
                    bakedNavMeshCount++;
                    if (bakedNavMeshCount == prefabSurfaces.Length) {
                        SavePrefab(prefab, prefabAssetPath);
                    }
                }
                
                updateNavMeshAsync.completed += OnNavMeshAsyncUpdated;

            }
        }

        private static void UpdateNavMeshData(NavMeshSurface surface, NavMeshData navMeshData)
        {
            DeleteOldNavMeshData(surface);
            SetNewNavMeshData(surface, navMeshData);
            CreateNavMeshDataAsset(surface);
        }

        private static void SavePrefab(GameObject prefab, string prefabAssetPath)
        {
            PrefabUtility.SaveAsPrefabAsset(prefab, prefabAssetPath);
            PrefabUtility.UnloadPrefabContents(prefab);
        }

        private static void DeleteOldNavMeshData(NavMeshSurface surface)
        {
            var navMeshData = surface.navMeshData;
            if (navMeshData == null) {
                return;
            }
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(navMeshData));
        }

        private static void CreateNavMeshDataAsset(NavMeshSurface surface)
        {
            var surfacePrefabPath = Path.GetDirectoryName(surface.gameObject.scene.path);

            var navMeshDataFolderPath = Path.Combine(surfacePrefabPath, "NavMesh");

            if (!Directory.Exists(navMeshDataFolderPath)) {
                AssetDatabase.CreateFolder(surfacePrefabPath, "NavMesh");
            }

            var navMeshDataPath = Path.Combine(navMeshDataFolderPath, "NavMesh-" + surface.name + ".asset");

            navMeshDataPath = AssetDatabase.GenerateUniqueAssetPath(navMeshDataPath);
            AssetDatabase.CreateAsset(surface.navMeshData, navMeshDataPath);
        }

        private static void SetNewNavMeshData(NavMeshSurface surface, NavMeshData navMeshData)
        {
            var serializedSurface = new SerializedObject(surface);
            var navMeshDataProperty = serializedSurface.FindProperty(NAV_MESH_DATA_PROPERTY_NAME);
            navMeshDataProperty.objectReferenceValue = navMeshData;
            serializedSurface.ApplyModifiedPropertiesWithoutUndo();
        }

        private static NavMeshData InitializeBakeData(NavMeshSurface surface)
        {
            var emptySources = new List<NavMeshBuildSource>();
            var emptyBounds = new Bounds();
            return NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds, surface.transform.position,
                                                   surface.transform.rotation);
        }
    }
}