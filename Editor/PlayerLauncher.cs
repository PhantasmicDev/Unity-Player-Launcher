using UnityEditor;

namespace PhantasmicGames.PlayerLauncherEditor
{
    public static class PlayerLauncher
    {
        private static string playerPath => EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);

        [MenuItem("File/Launch Player %#&p", false, priority = 221)]
        private static void Launch()
        {
            var launcher = PlayerLauncherSettings.instance.launcherEntries[EditorUserBuildSettings.selectedBuildTargetGroup];
            launcher.Launch(EditorUserBuildSettings.activeBuildTarget, playerPath);
        }
        
        [MenuItem("File/Launch Player %#&p", true)]
        private static bool ValidateLaunch()
        {
            if (string.IsNullOrEmpty(playerPath))
                return false;

            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            return PlayerLauncherSettings.instance.launcherEntries.TryGetLauncher(buildTargetGroup, out var launcher) &&
                   launcher.CanLaunch(playerPath);
        }
    }
}
