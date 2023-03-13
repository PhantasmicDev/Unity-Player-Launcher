using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [PlayerLauncher(BuildTargetGroup.Standalone)]
    public class StandalonePlayerLauncher : PlatformPlayerLauncher
    {
        [SerializeField] private string arguments;
        
        public override void Launch(BuildTarget buildTarget, string playerPath)
        {
            Process.Start(playerPath, arguments);
        }

        public override bool CanLaunch(string playerPath)
        {
            return Directory.Exists(playerPath);
        }
    }
}