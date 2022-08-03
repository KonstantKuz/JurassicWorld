using System.Linq;
using Dino.Config;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts.Config
{
    public class ConfigDownloaderWindow : EditorWindow
    {
        private string _mainUrl = "https://docs.google.com/spreadsheets/d/1Z--m3yV_njdcRhjxrCQ_-IxC6v6oj5IFlSFxCq3Xzc4";

        private const int MAIN_SHEET_ID_LIST = 515831250; //id of sheet that contains list of all other sheets
        private const string MAIN_CONFIG_PATH = "Resources/Configs";

        private const int CONFIGS_URL_PARTS_COUNT = 6;
        private const string CONFIGS_URL_PATTERN = "https://docs.google.com/spreadsheets/d";

        [MenuItem("App/Download Configs")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ConfigDownloaderWindow));
        }
        
        private void OnGUI () {
            GUILayout.Label("URL of google sheet with main configs", EditorStyles.boldLabel);
            GUILayout.Label("Should have format: https://docs.google.com/spreadsheets/d/KEY", EditorStyles.label);
            GUILayout.Label("without last / and anything after it", EditorStyles.label);
            _mainUrl = EditorGUILayout.TextField ("Url", _mainUrl);

            if (GUILayout.Button("Download all"))
            {
                DownloadAll(_mainUrl);
            }
            if (GUILayout.Button("Download localization only"))
            {
                DownloadLocalization();
            }
        }

        public static void Download(string url)
        {
            if (!url.Contains(CONFIGS_URL_PATTERN))
            {
                Debug.LogWarning("Invalid configs url.");
                return;
            }
            
            var urlParts = url.Split('/').Take(CONFIGS_URL_PARTS_COUNT);
            DownloadAll(string.Join("/", urlParts));
        }

        private static void DownloadAll(string url)
        {
            new ConfigDownloader(url, MAIN_SHEET_ID_LIST).Download(MAIN_CONFIG_PATH);
        }
        
        private void DownloadLocalization()
        {
            new ConfigDownloader(_mainUrl, MAIN_SHEET_ID_LIST).Download(MAIN_CONFIG_PATH,new[] { Configs.LOCALIZATION });
        }
    }
}