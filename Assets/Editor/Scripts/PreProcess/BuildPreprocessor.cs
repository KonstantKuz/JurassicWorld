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
        
        public static void SetDefines(bool platformBuild, bool debugConsoleEnabled)
        {
            SetDefine(PLATFORM_BUILD_DEFINE, platformBuild);
            SetDefine(DEBUG_CONSOLE_DEFINE, debugConsoleEnabled);
        }

        private static void SetDefine(string define, bool value)
        {
            if (value) {
                DefineSymbolsUtil.Add(define);
            } else {
                DefineSymbolsUtil.Remove(define);
            }
        }

        public static void BuildLoggerConfig(string loggerLevel)
        {
            ConfigPreprocessor.BuildLoggerConfig(loggerLevel);
        }

        public static void DownloadConfigs(string configsUrl)
        {
            ConfigDownloaderWindow.Download(configsUrl);
        }
    }
}