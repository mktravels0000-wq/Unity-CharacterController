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
        [SerializeField] private MuniClueDetector clueDetector;

        [Header("PC Controls")]
        [SerializeField] private Key transformationKey = Key.Q;
        [SerializeField] private Key clueScanKey = Key.E;

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard[transformationKey].wasPressedThisFrame)
                    ToggleTransformation();

                if (keyboard[clueScanKey].wasPressedThisFrame)
                    ScanForClues();
            }

            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
                Attack();
        }

        /// <summary>
        /// PC default: Left Mouse. Can also be called by a mobile UI Button.
        /// </summary>
        public void Attack()
        {
            if (spearCombat != null)
                spearCombat.Attack();
        }

        /// <summary>
        /// PC default: Q. Can also be called by a mobile UI Button.
        /// </summary>
        public void ToggleTransformation()
        {
            if (transformation != null)
                transformation.ToggleTransformation();
        }

        /// <summary>
        /// PC default: E. Can also be called by a mobile UI Button or spear-ground impact event.
        /// </summary>
        public void ScanForClues()
        {
            if (clueDetector != null)
                clueDetector.ScanForClues();
        }
    }
}
