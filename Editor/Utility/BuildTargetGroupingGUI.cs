using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Pool;

namespace PhantasmicGames.PlayerLauncherEditor
{
    public static class BuildTargetGroupingGUI
    {
        private static readonly Assembly s_buildAssembly = typeof(NamedBuildTarget).Assembly;
        private static readonly Type s_buildPlatformType = s_buildAssembly.GetType("UnityEditor.Build.BuildPlatform");
        private static readonly FieldInfo s_buildPlatform_namedBuildTargetFieldInfo = s_buildPlatformType.GetField("namedBuildTarget");
        
        private class BuildPlatformsWrapper
        {
            private static readonly Type s_Type = s_buildAssembly.GetType("UnityEditor.Build.BuildPlatforms");
            
            private static readonly object _instance = s_Type.GetProperty("instance")?.GetValue(null);
            private readonly MethodInfo _getValidPlatforms = s_Type.GetMethod("GetValidPlatforms", Type.EmptyTypes);

            public Array platforms { get; } = (Array)s_Type.GetField("buildPlatforms").GetValue(_instance);
            public Array validPlatforms { get; }

            public BuildPlatformsWrapper()
            {
                var list = (IList)_getValidPlatforms.Invoke(_instance, null);
                var array = Array.CreateInstance(s_buildPlatformType, list.Count);
                list.CopyTo(array, 0);
                validPlatforms = array;
            }
        }
        
        private class EditorGUILayoutWrapper
        {
            private static readonly Type s_Type = typeof(EditorGUILayout);
            private static readonly Type s_buildPlatformArrayType = s_buildPlatformType.MakeArrayType();

            private readonly MethodInfo _beginPlatformGrouping = s_Type.GetMethod("BeginPlatformGrouping",
                BindingFlags.Static | BindingFlags.NonPublic, null,
                new Type[] { s_buildPlatformArrayType, typeof(GUIContent) }, null);

            public int BeginPlatformGrouping(Array platforms)
            {
                return (int)_beginPlatformGrouping.Invoke(null, new object[] { platforms, null });
            }
        }

        private static readonly EditorGUILayoutWrapper s_editorGUILayoutWrapper = new EditorGUILayoutWrapper();
        private static readonly BuildPlatformsWrapper s_buildPlatforms = new BuildPlatformsWrapper();

        public static NamedBuildTarget BeginGrouping(bool allPlatforms, params NamedBuildTarget[] buildTargetsToExclude)
        {
            var platforms = allPlatforms ? s_buildPlatforms.platforms : s_buildPlatforms.validPlatforms;

            var indicesToRemove = ListPool<int>.Get();

            for (int i = 0; i < platforms.Length; i++)
            {
                var namedBuildTarget = (NamedBuildTarget)s_buildPlatform_namedBuildTargetFieldInfo.GetValue(platforms.GetValue(i));
                if (buildTargetsToExclude.Contains(namedBuildTarget))
                    indicesToRemove.Add(i);
            }

            var filteredPlatforms = Array.CreateInstance(s_buildPlatformType, platforms.Length - indicesToRemove.Count);

            for (int i = 0, j = 0; i < platforms.Length; i++)
            {
                if (indicesToRemove.Contains(i)) 
                    continue;
                filteredPlatforms.SetValue(platforms.GetValue(i), j);
                j++;
            }

            var index = s_editorGUILayoutWrapper.BeginPlatformGrouping(filteredPlatforms);
            var selectedPlatform = filteredPlatforms.GetValue(index);
            
            ListPool<int>.Release(indicesToRemove);
            
            return (NamedBuildTarget)s_buildPlatform_namedBuildTargetFieldInfo.GetValue(selectedPlatform);
        }
        
        public static NamedBuildTarget BeginGrouping(bool allPlatforms = false)
        {
            var platforms = allPlatforms ? s_buildPlatforms.platforms : s_buildPlatforms.validPlatforms;
            var index = s_editorGUILayoutWrapper.BeginPlatformGrouping(platforms);
            var selectedPlatform = platforms.GetValue(index);
            return (NamedBuildTarget)s_buildPlatform_namedBuildTargetFieldInfo.GetValue(selectedPlatform);
        }

        public static void EndGrouping()
        {
            EditorGUILayout.EndBuildTargetSelectionGrouping();
        }
    }
}
