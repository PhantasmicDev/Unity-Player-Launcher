using UnityEngine;

namespace PhantasmicDev.UnityPackageTemplate
{
    /// <summary>
    /// Provides 2D character like movement to an object with collision information.
    /// This is an example class to show where scripts are place within the project folder. Scripts in this folder are compiled for use at runtime.
    /// Documentation on this script is purely to show off what generated documentation will look like.
    /// </summary>
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private int _maxJumpCount = 2;

        /// <summary>
        /// The maximum speed the character can move, in meters per second.
        /// </summary>
        public float MaxSpeed => _maxSpeed;

        /// <summary>
        /// The maximum jumps the character can perform before landing on the ground again.
        /// </summary>
        public int MaxJumpCount => _maxJumpCount;

        /// <summary>
        /// True if the character has contact with the ground.
        /// </summary>
        public bool IsGrounded { get; private set; }

        /// <summary>
        /// Make the player jump in the next available physics step.
        /// </summary>
        /// <param name="speed">The initial speed to take off from the ground.</param>
        public void Jump(float speed)
        {
            // Jump code
        }

        /// <summary>
        /// Returns true if the character is descending vertically and is not grounded.
        /// </summary>
        public bool IsFalling()
        {
            return /* velocity.y < 0 && */ IsGrounded == false;
        }
    }
}