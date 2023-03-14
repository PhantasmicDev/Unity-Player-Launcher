using System;
using UnityEditor;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlayerLauncherAttribute : Attribute
    {
        internal BuildTargetGroup BuildTargetGroup { get; }
        
        public PlayerLauncherAttribute(BuildTargetGroup buildTargetGroup)
        {
            BuildTargetGroup = buildTargetGroup;
        }
    }
}
