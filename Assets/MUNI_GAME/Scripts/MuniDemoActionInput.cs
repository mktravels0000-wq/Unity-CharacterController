using UnityEngine;
using UnityEngine.InputSystem;

namespace MuniGame
{
    /// <summary>
    /// Optional keyboard/mouse action bridge for the PC Demo 01 prototype.
    /// Movement and jump remain handled by the existing NaughtyCharacter input setup.
    /// Mobile UI buttons can call the public methods directly.
    /// </summary>
    public class MuniDemoActionInput : MonoBehaviour
    {
        [Header("MUNI Actions")]
        [SerializeField] private MuniSpearCombat spearCombat;
        [SerializeField] private MuniTransformation transformation;

        [Header("PC Controls")]
        [SerializeField] private Key transformationKey = Key.Q;

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard[transformationKey].wasPressedThisFrame)
            {
                ToggleTransformation();
            }

            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                Attack();
            }
        }

        /// <summary>
        /// Can be called by a mobile UI Button or UnityEvent.
        /// </summary>
        public void Attack()
        {
            if (spearCombat != null)
                spearCombat.Attack();
        }

        /// <summary>
        /// PC default: Q. Can also be called by a mobile UI Button or UnityEvent.
        /// </summary>
        public void ToggleTransformation()
        {
            if (transformation != null)
                transformation.ToggleTransformation();
        }
    }
}
