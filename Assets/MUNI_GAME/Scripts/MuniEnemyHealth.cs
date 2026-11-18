using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Basic enemy health wrapper for Demo 01.
    /// </summary>
    [RequireComponent(typeof(MuniHealth))]
    public class MuniEnemyHealth : MonoBehaviour
    {
        [SerializeField] private MuniHealth health;
        [SerializeField] private GameObject defeatVisual;
        [SerializeField] private bool disableEnemyOnDefeat = true;

        private bool defeatHandled;

        private void Awake()
        {
            if (health == null) health = GetComponent<MuniHealth>();
        }

        private void Update()
        {
            if (defeatHandled || health == null || !health.IsDefeated) return;

            defeatHandled = true;

            if (defeatVisual != null)
                defeatVisual.SetActive(true);

            if (disableEnemyOnDefeat)
                gameObject.SetActive(false);
        }

        public void ReceiveDamage(int damage)
        {
            if (health != null && !defeatHandled)
                health.TakeDamage(damage);
        }
    }
}
