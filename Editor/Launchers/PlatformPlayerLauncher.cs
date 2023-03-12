using UnityEditor;
using UnityEngine;

namespace PhantasmicGames.PlayerLauncherEditor
{
    public abstract class PlatformPlayerLauncher : ScriptableObject
    {
        public abstract void Launch(BuildTarget buildTarget, string playerPath);

        public abstract bool CanLaunch(string playerPath);
    }
}