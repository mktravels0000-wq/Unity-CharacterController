using UnityEngine;
using UnityEngine.Events;

namespace MuniGame
{
    /// <summary>
    /// Boss health wrapper with configurable phase thresholds.
    /// Attach alongside MuniHealth and configure phase events in the Inspector.
    /// </summary>
    [RequireComponent(typeof(MuniHealth))]
    public class MuniBossHealth : MonoBehaviour
    {
        [System.Serializable]
        public class BossPhaseEvent : UnityEvent<int> { }

        [Header("Boss Identity")]
        [SerializeField] private string bossName = "Wizard";
        [Header("Phase Thresholds (health percentage)")]
        [SerializeField, Range(1f, 99f)] private float secondPhaseAt = 60f;
        [SerializeField, Range(1f, 99f)] private float finalPhaseAt = 25f;
        [Header("Events")]
        [SerializeField] private BossPhaseEvent onPhaseChanged = new BossPhaseEvent();
        [SerializeField] private UnityEvent onDefeated = new UnityEvent();

        private MuniHealth health;
        private int currentPhase = 1;
        private bool defeatEventSent;

        public string BossName => bossName;
        public int CurrentPhase => currentPhase;
        public float HealthPercent => health == null || health.MaxHealth <= 0
            ? 0f
            : 100f * health.CurrentHealth / health.MaxHealth;

        private void Awake()
        {
            health = GetComponent<MuniHealth>();
            if (finalPhaseAt >= secondPhaseAt)
                finalPhaseAt = Mathf.Max(1f, secondPhaseAt - 1f);
        }

        private void Update()
        {
            if (health == null) return;

            if (health.IsDefeated)
            {
                if (!defeatEventSent)
                {
                    defeatEventSent = true;
                    onDefeated.Invoke();
                }
                return;
            }

            int nextPhase = HealthPercent <= finalPhaseAt ? 3
                : HealthPercent <= secondPhaseAt ? 2 : 1;

            if (nextPhase != currentPhase)
            {
                currentPhase = nextPhase;
                onPhaseChanged.Invoke(currentPhase);
            }
        }

        public void TakeDamage(int amount)
        {
            if (health != null)
                health.TakeDamage(amount);
        }
    }
}
