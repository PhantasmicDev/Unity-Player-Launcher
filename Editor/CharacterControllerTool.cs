using UnityEditor;
using UnityEngine;
using PhantasmicDev.UnityPackageTemplate;

namespace PhantasmicDev.UnityPackageTemplateEditor
{
    /// <summary>
    /// A utility class that adds Editor functionality when working with <see cref="CharacterController2D"/> instances in a scene.
    /// This is an example class to show where Editor scripts are place within the project folder. Other scripts in this folder do NOT get included in the final build.
    /// </summary>
    public static class CharacterControllerTool
    {
        /// <summary>
        /// Selects the first <see cref="CharacterController2D"/> in the opened scene.
        /// </summary>
        [MenuItem("Tools/Select First Character")]
        public static void SelectFirstCharacter()
        {
            var character = Object.FindObjectOfType<CharacterController2D>();

            if (character)
                Selection.activeGameObject = character.gameObject;
        }
    }
}
