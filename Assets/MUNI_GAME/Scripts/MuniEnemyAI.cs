using UnityEngine;

namespace MuniGame
{
    /// <summary>
    /// Lightweight prototype enemy AI. Uses direct movement and does not require a baked NavMesh.
    /// Place on an enemy root with a Collider and MuniHealth.
    /// </summary>
    [RequireComponent(typeof(MuniHealth))]
    public class MuniEnemyAI : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField, Min(0.5f)] private float detectionRange = 8f;
        [SerializeField, Min(0.1f)] private float attackRange = 1.5f;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 2.2f;
        [SerializeField, Min(0f)] private float turnSpeed = 360f;

        [Header("Attack")]
        [SerializeField, Min(0)] private int damage = 10;
        [SerializeField, Min(0.1f)] private float attackCooldown = 1.5f;

        private MuniHealth targetHealth;
        private MuniHealth ownHealth;
        private float nextAttackTime;

        private void Awake()
        {
            ownHealth = GetComponent<MuniHealth>();
            if (target != null)
                targetHealth = target.GetComponentInParent<MuniHealth>();
        }

        private void Update()
        {
            if (ownHealth == null || ownHealth.IsDefeated)
                return;

            if (target == null || targetHealth == null)
                return;

            if (targetHealth.IsDefeated)
                return;

            Vector3 offset = target.position - transform.position;
            offset.y = 0f;
            float distance = offset.magnitude;

            if (distance > detectionRange)
                return;

            if (offset.sqrMagnitude > 0.001f)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(offset.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
            }

            if (distance > attackRange)
            {
                Vector3 step = offset.normalized * moveSpeed * Time.deltaTime;
                if (distance - step.magnitude < attackRange)
                    step = offset.normalized * (distance - attackRange);

                transform.position += step;
                return;
            }

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                targetHealth.TakeDamage(damage);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            targetHealth = target != null
                ? target.GetComponentInParent<MuniHealth>()
                : null;
        }
    }
}
