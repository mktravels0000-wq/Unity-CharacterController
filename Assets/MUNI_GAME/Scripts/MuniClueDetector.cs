using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Reveals disguised wizards within range when the player performs a clue scan.
    /// Call ScanForClues from a keyboard input, UI button, or spear-ground impact event.
    /// </summary>
    public class MuniClueDetector : MonoBehaviour
    {
        [SerializeField, Min(0.5f)] private float scanRadius = 7f;
        [SerializeField] private LayerMask clueLayers = ~0;

        public void ScanForClues()
        {
            Collider[] nearby = Physics.OverlapSphere(
                transform.position, scanRadius, clueLayers, QueryTriggerInteraction.Collide);

            for (int i = 0; i < nearby.Length; i++)
            {
                MuniWizardReveal wizard = nearby[i].GetComponentInParent<MuniWizardReveal>();
                if (wizard != null && !wizard.IsRevealed)
                    wizard.Reveal();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, scanRadius);
        }
    }
}
