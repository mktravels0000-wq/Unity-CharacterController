using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Prototype spear attack. Call Attack() from a UI Button or UnityEvent.
    /// </summary>
    public class MuniSpearCombat : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField, Min(0)] private int damage = 25;
        [SerializeField, Min(0.1f)] private float attackRange = 1.6f;
        [SerializeField, Min(0.1f)] private float hitRadius = 0.65f;
        [SerializeField, Min(0)] private float attackCooldown = 0.7f;
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask targetLayers = ~0;

        private float nextAttackTime;

        /// <summary>
        /// Performs one sphere-based hit check in front of the player.
        /// </summary>
        public void Attack()
        {
            if (Time.time < nextAttackTime) return;
            nextAttackTime = Time.time + attackCooldown;

            Vector3 origin = attackOrigin != null
                ? attackOrigin.position
                : transform.position + Vector3.up;

            Vector3 center = origin + transform.forward * attackRange;
            Collider[] hits = Physics.OverlapSphere(center, hitRadius, targetLayers, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hits.Length; i++)
            {
                MuniEnemyHealth enemy = hits[i].GetComponentInParent<MuniEnemyHealth>();
                if (enemy != null)
                    enemy.ReceiveDamage(damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 origin = attackOrigin != null
                ? attackOrigin.position
                : transform.position + Vector3.up;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin + transform.forward * attackRange, hitRadius);
        }
    }
}
