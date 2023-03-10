using UnityEditor;

namespace PhantasmicGames.PlayerLauncherEditor
{
    public abstract class PlatformPlayerLauncher
    {
        public abstract bool Launch();
    }

    [PlayerLauncher(BuildTargetGroup.Standalone)]
    public class StandalonePlayerLauncher : PlatformPlayerLauncher
    {
        public override bool Launch()
        {
            return false;
        }
    }
}