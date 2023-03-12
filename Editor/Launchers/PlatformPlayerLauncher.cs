using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace PhantasmicGames.PlayerLauncherEditor
{
    public abstract class PlatformPlayerLauncher : ScriptableObject
    {
        public abstract void Launch(BuildTarget buildTarget);

        public abstract bool CanLaunch();
    }
}