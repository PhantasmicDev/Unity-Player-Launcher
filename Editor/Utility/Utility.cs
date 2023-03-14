using System;

namespace PhantasmicGames.PlayerLauncherEditor
{
    internal static class Utility
    {
        public static int GetInheritanceDepth(Type baseType, Type derivedType)
        {
            int depth = 0;
            var currentType = derivedType;
            while (currentType.BaseType != baseType && currentType.BaseType != null)
            {
                currentType = currentType.BaseType;
                depth++;
            }
            return currentType.BaseType == null ? -1 : depth + 1;
        }
        
        public static Type GetTypeClosestToParent(Type type1, Type type2, Type parent)
        {
            var type1Depth = GetInheritanceDepth(parent, type1);
            var type2Depth = GetInheritanceDepth(parent, type2);

            return type2Depth < type1Depth ? type2 : type1;
        }
    }
}