using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Simple health component for the MUNI GAME prototype.
    /// </summary>
    public class MuniHealth : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maxHealth = 100;
        [SerializeField] private bool destroyOnZero;
        [SerializeField] private bool damageImmune;

        public int MaxHealth => maxHealth;
        public int CurrentHealth { get; private set; }
        public bool IsDefeated => CurrentHealth <= 0;

        private void Awake()
        {
            maxHealth = Mathf.Max(1, maxHealth);
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (damageImmune || amount <= 0 || IsDefeated) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

            if (IsDefeated && destroyOnZero)
            {
                Destroy(gameObject);
            }
        }

        public void RestoreHealth(int amount)
        {
            if (amount <= 0 || IsDefeated) return;
            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
        }
    }
}
