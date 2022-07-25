using Editor.Scripts.Config;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Editor.Scripts.PreProcess
{
    public static class BuildPreprocessor
    {
        private const string PLATFORM_BUILD_DEFINE = "PLATFORM_BUILD";
        private const string DEBUG_CONSOLE_DEFINE = "DEBUG_CONSOLE_ENABLED";
        
        public static void Prepare(bool platformBuild, bool debugConsoleEnabled, [CanBeNull] string loggerLevel)
        {
            SetDefine(PLATFORM_BUILD_DEFINE, platformBuild);
            SetDefine(DEBUG_CONSOLE_DEFINE, debugConsoleEnabled);
            if (!string.IsNullOrEmpty(loggerLevel)) {
                BuildLoggerConfig(loggerLevel);
            }
        }

        private static void SetDefine(string define, bool value)
        {
            if (value) {
                DefineSymbolsUtil.Add(define);
            } else {
                DefineSymbolsUtil.Remove(define);
            }
        }

        private static void BuildLoggerConfig(string loggerLevel)
        {
            ConfigPreprocessor.BuildLoggerConfig(loggerLevel);
        }

        public static void DownloadConfigs([CanBeNull] string configsUrl)
        {
            if (configsUrl.IsNullOrEmpty())
            {
                Debug.LogWarning("Configs url is empty. Build will use configs already placed in project.");
                return;
            }
                        
            ConfigDownloaderWindow.Download(configsUrl);
        }
    }
}