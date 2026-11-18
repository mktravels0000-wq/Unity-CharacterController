using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Demo transformation prototype: swaps normal and divine character visuals.
    /// Assign both visual roots in the Inspector.
    /// </summary>
    public class MuniTransformation : MonoBehaviour
    {
        [SerializeField] private GameObject normalVisual;
        [SerializeField] private GameObject divineVisual;

        public bool IsDivineForm { get; private set; }

        private void Awake()
        {
            ApplyVisualState(false);
        }

        /// <summary>
        /// Toggles between normal and divine visual objects.
        /// Can be called by a keyboard binding, UI Button, or UnityEvent.
        /// </summary>
        public void ToggleTransformation()
        {
            ApplyVisualState(!IsDivineForm);
        }

        public void EnterDivineForm()
        {
            ApplyVisualState(true);
        }

        public void ExitDivineForm()
        {
            ApplyVisualState(false);
        }

        private void ApplyVisualState(bool divine)
        {
            IsDivineForm = divine;

            if (normalVisual != null)
                normalVisual.SetActive(!divine);

            if (divineVisual != null)
                divineVisual.SetActive(divine);
        }
    }
}
