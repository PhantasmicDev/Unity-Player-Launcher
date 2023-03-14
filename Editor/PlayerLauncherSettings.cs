using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PhantasmicGames.PlayerLauncherEditor
{
    [FilePath("ProjectSettings/PlayerLauncherSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class PlayerLauncherSettings : ScriptableSingleton<PlayerLauncherSettings>
    {
        [SerializeField] private PlatformLauncherEntries _launchers = new PlatformLauncherEntries();
        [NonSerialized] private readonly PlatformLauncherEntries _fallbackLaunchers = new PlatformLauncherEntries();

        private void OnEnable()
        {
            GatherFallbackLaunchers();
        }

        private void GatherFallbackLaunchers()
        {
            var launcherBaseType = typeof(PlatformPlayerLauncher);
            foreach (var launcherType in TypeCache.GetTypesWithAttribute<PlayerLauncherAttribute>())
            {
                if(!launcherType.IsSubclassOf(launcherBaseType))
                    continue;

                var buildTargetGroup = launcherType.GetCustomAttribute<PlayerLauncherAttribute>().BuildTargetGroup;
                var hasFallbackLauncher = _fallbackLaunchers.TryGetLauncher(buildTargetGroup, out var launcher);

                if (hasFallbackLauncher &&
                    Utility.GetTypeClosestToParent(launcher.GetType(), launcherType, launcherBaseType) == launcherType || !hasFallbackLauncher)
                {
                    if (hasFallbackLauncher)
                        DestroyImmediate(launcher);
                    
                    launcher = (PlatformPlayerLauncher)CreateInstance(launcherType);
                    _fallbackLaunchers[buildTargetGroup] = launcher;
                }
            }
        }
        
        public static PlatformPlayerLauncher GetLauncher(BuildTargetGroup buildTargetGroup) => instance._launchers[buildTargetGroup];

        public static void SetLauncher(BuildTargetGroup buildTargetGroup, PlatformPlayerLauncher launcher)
        {
            if (instance._launchers.TryGetLauncher(buildTargetGroup, out var currentLauncher))
            {
                if (launcher != null && !AssetDatabase.Contains(launcher))
                    DestroyImmediate(currentLauncher);
            }

            if (launcher == null)
                instance._launchers.RemoveLauncher(buildTargetGroup);
            else
                instance._launchers[buildTargetGroup] = launcher;
            
            instance.Save(true);
        }

        public static bool TryGetLauncher(BuildTargetGroup buildTargetGroup, out PlatformPlayerLauncher launcher, bool includeFallback = false)
        {
            var hasLauncher = instance._launchers.TryGetLauncher(buildTargetGroup, out launcher);
            if (!hasLauncher && includeFallback)
                hasLauncher = instance._fallbackLaunchers.TryGetLauncher(buildTargetGroup, out launcher);

            return hasLauncher;
        }

        public static bool TryGetFallbackLauncher(BuildTargetGroup buildTargetGroup, out PlatformPlayerLauncher launcher)
        {
            return instance._fallbackLaunchers.TryGetLauncher(buildTargetGroup, out launcher);
        }
        
        public static Type GetLauncherBaseType(BuildTargetGroup buildTargetGroup)
        {
            instance._fallbackLaunchers.TryGetLauncher(buildTargetGroup, out var launcher);
            return launcher != null ? launcher.GetType() : typeof(PlatformPlayerLauncher);
        }
    }

    [Serializable]
    internal class PlatformLauncherEntries : ISerializationCallbackReceiver
    {
        [SerializeField] private List<BuildTargetGroup> _buildTargetGroups = new List<BuildTargetGroup>();
        [SerializeField] private List<PlatformPlayerLauncher> _playerLaunchers = new List<PlatformPlayerLauncher>();

        private Dictionary<BuildTargetGroup, PlatformPlayerLauncher> _dict =
            new Dictionary<BuildTargetGroup, PlatformPlayerLauncher>();

        public void OnBeforeSerialize()
        {
            _buildTargetGroups.Clear();
            _playerLaunchers.Clear();

            foreach (var (buildTargetGroup, playerLauncher) in _dict)
            {
                if (playerLauncher == null)
                    continue;
                
                _buildTargetGroups.Add(buildTargetGroup);
                _playerLaunchers.Add(playerLauncher);
            }
        }

        public void OnAfterDeserialize()
        {
            _dict.Clear();

            for (int i = 0; i < Mathf.Min(_buildTargetGroups.Count, _playerLaunchers.Count); i++)
            {
                _dict.Add(_buildTargetGroups[i], _playerLaunchers[i]);
            }
        }

        public PlatformPlayerLauncher this[BuildTargetGroup buildTargetGroup]
        {
            get => _dict[buildTargetGroup];
            set => _dict[buildTargetGroup] = value;
        }

        public bool TryGetLauncher(BuildTargetGroup buildTargetGroup, out PlatformPlayerLauncher launcher)
        {
            return _dict.TryGetValue(buildTargetGroup, out launcher);
        }

        public bool ContainsLauncher(BuildTargetGroup buildTargetGroup)
        {
            return _dict.ContainsKey(buildTargetGroup);
        }

        public bool RemoveLauncher(BuildTargetGroup buildTargetGroup)
        {
            return _dict.Remove(buildTargetGroup);
        }
    }
}
