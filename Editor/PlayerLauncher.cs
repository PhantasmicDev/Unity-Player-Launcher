using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [InitializeOnLoad]
    public class PlayerLauncher : IPreprocessBuildWithReport
    {
        private static Dictionary<BuildTargetGroup, List<Type>> s_launchers = new Dictionary<BuildTargetGroup, List<Type>>();

        private static BuildReport s_buildReport;
        
        public int callbackOrder => 0;


        static PlayerLauncher()
        {
            //LaunchPlayerAttribute attr = null;
            //ar group = attr.TargetGroup;
        }
        
        
        public void OnPreprocessBuild(BuildReport report)
        {
            s_buildReport = report;
        }
        
        [PostProcessBuild]
        private static void SaveBuildInfo(BuildTarget target, string pathToBuiltProject)
        {
            //BuildPipeline.BuildPlayer()
            //TypeCache.GetMethodsWithAttribute
        }

        [MenuItem("File/Launch Player %#&p", false, priority = 221)]
        private static void Launch()
        {
#if UNITY_ANDROID
            
            string androidSdkRoot = EditorPrefs.GetString("AndroidSdkRoot");
            string platformToolsPath = Path.Combine(androidSdkRoot, "platform-tools");
            string adbPath = Path.Combine(platformToolsPath, "adb");
            
            var command = $"-d shell am start -n {Application.identifier}/com.unity3d.player.UnityPlayerActivity -a android.intent.action.MAIN -c android.intent.category.LAUNCHER";

            Process process = new Process();

            var startInfo = new ProcessStartInfo
            {
                FileName = adbPath,
                Arguments = command,
                CreateNoWindow = true
            };


            process.StartInfo = startInfo;
            process.Start();
#endif
        }
    }
}
