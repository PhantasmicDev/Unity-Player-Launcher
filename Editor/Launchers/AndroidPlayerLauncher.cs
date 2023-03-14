using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [PlayerLauncher(BuildTargetGroup.Android)]
    public class AndroidPlayerLauncher : PlatformPlayerLauncher
    {
        public override void Launch(BuildTarget buildTarget, string playerPath)
        {
            var adbPath = Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb");
            var arguments = $"-d shell am start -n {Application.identifier}/com.unity3d.player.UnityPlayerActivity -a android.intent.action.MAIN -c android.intent.category.LAUNCHER";

            Process.Start(adbPath, arguments);
        }

        public override bool CanLaunch(string playerPath)
        {
            return false;
        }
    }
}
