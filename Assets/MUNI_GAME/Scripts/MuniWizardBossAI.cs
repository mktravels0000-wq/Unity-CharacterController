using System.Collections.Generic;
using UnityEngine;

namespace MuniGame
{
    public enum MuniBossAttackPattern
    {
        Melee,
        Dash,
        Pulse
    }

    /// <summary>
    /// Reusable prototype AI for the three wizard bosses.
    /// Configure a different attack pattern and values for each wizard in the Inspector.
    /// </summary>
    [RequireComponent(typeof(MuniHealth))]
    public class MuniWizardBossAI : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField, Min(1f)] private float detectionRange = 12f;
        [SerializeField, Min(0.5f)] private float preferredAttackRange = 2f;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 2f;
        [SerializeField, Min(0f)] private float turnSpeed = 360f;

        [Header("Boss Attack")]
        [SerializeField] private MuniBossAttackPattern attackPattern = MuniBossAttackPattern.Melee;
        [SerializeField, Min(0)] private int baseDamage = 12;
        [SerializeField, Min(0.1f)] private float attackCooldown = 2f;
        [SerializeField, Min(0.1f)] private float attackRadius = 1.5f;
        [SerializeField, Min(0.1f)] private float dashDistance = 3f;
        [SerializeField] private LayerMask targetLayers = ~0;

        private MuniHealth ownHealth;
        private MuniHealth targetHealth;
        private MuniBossHealth bossHealth;
        private float nextAttackTime;

        private void Awake()
        {
            ownHealth = GetComponent<MuniHealth>();
            bossHealth = GetComponent<MuniBossHealth>();
            CacheTargetHealth();
        }

        private void Update()
        {
            if (ownHealth == null || ownHealth.IsDefeated || target == null)
                return;

            if (targetHealth == null)
                CacheTargetHealth();

            if (targetHealth == null || targetHealth.IsDefeated)
                return;

            Vector3 offset = target.position - transform.position;
            offset.y = 0f;
            float distance = offset.magnitude;

            if (distance > detectionRange)
                return;

            if (offset.sqrMagnitude > 0.001f)
            {
                Quaternion desired = Quaternion.LookRotation(offset.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, desired, turnSpeed * Time.deltaTime);
            }

            if (distance > preferredAttackRange)
            {
                float speed = moveSpeed;
                if (attackPattern == MuniBossAttackPattern.Dash && distance > preferredAttackRange * 1.5f)
                    speed *= 2.5f;

                Vector3 step = offset.normalized * speed * Time.deltaTime;
                if (distance - step.magnitude < preferredAttackRange)
                    step = offset.normalized * (distance - preferredAttackRange);

                transform.position += step;
                return;
            }

            if (Time.time >= nextAttackTime)
                PerformAttack();
        }

        private void PerformAttack()
        {
            nextAttackTime = Time.time + attackCooldown;

            Vector3 center = transform.position + transform.forward * attackRadius + Vector3.up;
            float radius = attackRadius;

            if (attackPattern == MuniBossAttackPattern.Pulse)
                radius *= 2f;
            else if (attackPattern == MuniBossAttackPattern.Dash)
                transform.position += transform.forward * dashDistance;

            Collider[] hits = Physics.OverlapSphere(
                center, radius, targetLayers, QueryTriggerInteraction.Ignore);

            int phaseMultiplier = bossHealth != null ? bossHealth.CurrentPhase : 1;
            int damage = baseDamage * phaseMultiplier;

            HashSet<MuniHealth> damagedTargets = new HashSet<MuniHealth>();
            for (int i = 0; i < hits.Length; i++)
            {
                MuniHealth hitHealth = hits[i].GetComponentInParent<MuniHealth>();
                if (hitHealth != null && hitHealth != ownHealth && damagedTargets.Add(hitHealth))
                    hitHealth.TakeDamage(damage);
            }
        }

        private void CacheTargetHealth()
        {
            if (target != null)
                targetHealth = target.GetComponentInParent<MuniHealth>();
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            targetHealth = null;
            CacheTargetHealth();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(
                transform.position + transform.forward * attackRadius + Vector3.up,
                attackPattern == MuniBossAttackPattern.Pulse ? attackRadius * 2f : attackRadius);
        }
    }
}
