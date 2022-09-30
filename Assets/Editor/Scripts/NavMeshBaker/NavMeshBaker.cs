using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Editor.Scripts.NavMeshBaker
{
    public class NavMeshBaker : EditorWindow
    {
        private string _prefab = "Assets/Resources/Content/Level/1-2/scene.prefab";

        [MenuItem("App/Build NavMeshes")]
        public static void ShowWindow()
        {
            GetWindow(typeof(NavMeshBaker));
        }

        private void OnGUI()
        {
            _prefab = EditorGUILayout.TextField("Prefab", _prefab);

            if (GUILayout.Button("Build NavMeshes")) {
                BakeAllNavmeshInFolder(_prefab);
            }
        }

        public void BakeAllNavmeshInFolder(string assetPath)
        {
            GameObject prefab = PrefabUtility.LoadPrefabContents(assetPath);
            NavMeshSurface[] surfaces = prefab.GetComponentsInChildren<NavMeshSurface>();

            var completedNavMeshCount = 0;
            for (int index = 0; index < surfaces.Length; index++) {
          
                var navMeshSurface = surfaces[index];
                var data = InitializeBakeData(navMeshSurface);
                var asyncOperation = navMeshSurface.UpdateNavMesh(data);
                asyncOperation.completed += (operation) => {
                    SetNavMeshData(navMeshSurface, data);
                    CreateNavMeshAsset(navMeshSurface);
                    PrefabUtility.SaveAsPrefabAsset(prefab, assetPath);
                    completedNavMeshCount++;
                    if (completedNavMeshCount == surfaces.Length) {
                        PrefabUtility.UnloadPrefabContents(prefab); 
                    }
                };
            }
        }
        private static void CreateNavMeshAsset(NavMeshSurface surface)
        {
            var targetPath = GetAndEnsureTargetPath(surface);
            
      
            if (!Directory.Exists(Path.Combine(targetPath, "NavMesh")))
            {
                AssetDatabase.CreateFolder(targetPath, "NavMesh");
            }
            var combinedAssetPath = Path.Combine(targetPath, "NavMesh", "NavMesh-" + surface.name + ".asset");
            combinedAssetPath = AssetDatabase.GenerateUniqueAssetPath(combinedAssetPath);
            AssetDatabase.CreateAsset(surface.navMeshData, combinedAssetPath);
        }

        static void SetNavMeshData(NavMeshSurface navSurface, NavMeshData navMeshData)
        {
            var so = new SerializedObject(navSurface);
            var navMeshDataProperty = so.FindProperty("m_NavMeshData");
            navMeshDataProperty.objectReferenceValue = navMeshData;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static NavMeshData InitializeBakeData(NavMeshSurface surface)
        {
            var emptySources = new List<NavMeshBuildSource>();
            var emptyBounds = new Bounds();
            return UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds, surface.transform.position,
                                                                  surface.transform.rotation);
        }

        static string GetAndEnsureTargetPath(NavMeshSurface surface)
        {
            var activeScenePath = surface.gameObject.scene.path;
            return Path.Combine(Path.GetDirectoryName(activeScenePath));
        }
    }
}