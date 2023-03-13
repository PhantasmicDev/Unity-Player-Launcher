using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [PlayerLauncher(BuildTargetGroup.WebGL)]
    public class WebGLPlayerLauncher : PlatformPlayerLauncher
    {
        private static readonly MethodInfo s_HttpServerEditorWrapper_CreateIfNeededMethod = typeof(UnityEditor.WebGL.UserBuildSettings).Assembly
            .GetType("UnityEditor.WebGL.HttpServerEditorWrapper").GetMethod("CreateIfNeeded");
        
        public override void Launch(BuildTarget buildTarget, string playerPath)
        {
            var args = new object[] { playerPath, 0 };
            s_HttpServerEditorWrapper_CreateIfNeededMethod.Invoke(null, args);
            Application.OpenURL($"http://localhost:{args[1]}/");
        }

        public override bool CanLaunch(string playerPath)
        {
            return Directory.Exists(playerPath);
        }
    }
}
