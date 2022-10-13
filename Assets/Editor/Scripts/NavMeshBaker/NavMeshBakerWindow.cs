using UnityEditor;
using UnityEngine;

namespace Editor.Scripts.NavMeshBaker
{
    public class NavMeshBakerWindow : EditorWindow
    {
        private string _rootFolderPath = "Assets/Resources/Content/Level";

        [MenuItem("App/Bake all NavMeshes in a folder")]
        public static void ShowWindow() => GetWindow(typeof(NavMeshBakerWindow));

        private void OnGUI()
        {
            _rootFolderPath = EditorGUILayout.TextField("Root folder path", _rootFolderPath);

            if (GUILayout.Button("Bake NavMeshes")) {
                NavMeshBaker.BakeAllNavMeshesInFolder(_rootFolderPath);
            }
        }
    }
}