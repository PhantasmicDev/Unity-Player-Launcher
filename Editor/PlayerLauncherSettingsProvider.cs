using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;

namespace PhantasmicGames.PlayerLauncherEditor
{
    public class PlayerLauncherSettingsProvider : SettingsProvider
    {
        private static readonly GUIContent s_launcherLabel = new GUIContent("Player Launcher");
        
        private Editor _selectedPlatformLauncherEditor;

        private PlayerLauncherSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new PlayerLauncherSettingsProvider("Project/Player Launcher", SettingsScope.Project);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if (PlayerLauncherSettings.TryGetLauncher(EditorUserBuildSettings.selectedBuildTargetGroup, out var launcher, true))
                _selectedPlatformLauncherEditor = Editor.CreateEditor(launcher);
        }

        public override void OnGUI(string searchContext)
        {
            GUILayout.Space(16f);

            var buildTargetGroup = BuildTargetGroupingGUI.BeginGrouping(false, NamedBuildTarget.Server).ToBuildTargetGroup();

            var hasFallbackLauncher = PlayerLauncherSettings.TryGetFallbackLauncher(buildTargetGroup, out var fallbackLauncher);
            var hasLauncher = PlayerLauncherSettings.TryGetLauncher(buildTargetGroup, out var launcher);
            
            if (!hasLauncher && !hasFallbackLauncher)
            {
                GUILayout.Label("No Supported Launcher");
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                
                var launcherBaseType = PlayerLauncherSettings.GetLauncherBaseType(buildTargetGroup);
                launcher = (PlatformPlayerLauncher)EditorGUILayout.ObjectField(s_launcherLabel, launcher, launcherBaseType, false);

                if (EditorGUI.EndChangeCheck())
                {
                    PlayerLauncherSettings.SetLauncher(buildTargetGroup, launcher);
                    _selectedPlatformLauncherEditor = Editor.CreateEditor(launcher != null ? launcher : fallbackLauncher);
                }

                if (_selectedPlatformLauncherEditor != null)
                {
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(hasLauncher == false))
                    {
                        _selectedPlatformLauncherEditor.OnInspectorGUI();
                    }
                    EditorGUI.indentLevel--;
                }
            }
            BuildTargetGroupingGUI.EndGrouping();
        }
    }
}