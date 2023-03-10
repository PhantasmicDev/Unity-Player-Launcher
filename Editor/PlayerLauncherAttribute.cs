using System;
using UnityEditor;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [AttributeUsage(AttributeTargets.Class)]

    public class PlayerLauncherAttribute : CallbackOrderAttribute
    {
        public BuildTargetGroup TargetGroup { get; }

        public PlayerLauncherAttribute(BuildTargetGroup targetGroup, int order = 0)
        {
            TargetGroup = targetGroup;
            m_CallbackOrder = 0;
        }
    }
}