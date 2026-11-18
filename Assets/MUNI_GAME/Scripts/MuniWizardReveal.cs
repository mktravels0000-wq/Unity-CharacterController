using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Controls a disguised wizard. Assign the disguise and revealed visuals in the Inspector.
    /// </summary>
    public class MuniWizardReveal : MonoBehaviour
    {
        [SerializeField] private GameObject disguiseVisual;
        [SerializeField] private GameObject revealedVisual;
        [SerializeField] private bool startDisguised = true;

        public bool IsRevealed { get; private set; }

        private void Awake()
        {
            ApplyState(!startDisguised);
        }

        public void Reveal()
        {
            ApplyState(true);
        }

        public void HideAsDisguise()
        {
            ApplyState(false);
        }

        private void ApplyState(bool revealed)
        {
            IsRevealed = revealed;

            if (disguiseVisual != null)
                disguiseVisual.SetActive(!revealed);

            if (revealedVisual != null)
                revealedVisual.SetActive(revealed);
        }
    }
}
