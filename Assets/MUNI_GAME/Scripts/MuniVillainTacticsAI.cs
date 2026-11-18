using UnityEngine;
using UnityEngine.Events;

namespace MuniGame
{
    public enum MuniVillainStyle
    {
        AggressiveFighter,
        HitAndRun,
        SpellCaster
    }

    public enum MuniVillainState
    {
        Idle,
        Chase,
        Fight,
        Flee,
        Hide
    }

    /// <summary>
    /// Additive prototype AI: approaches Muni, attacks without reducing Muni's health,
    /// retreats when threatened, hides at a designer-placed point, and can cast the
    /// 10-second green-rope binding spell.
    /// </summary>
    [RequireComponent(typeof(MuniHealth))]
    public class MuniVillainTacticsAI : MonoBehaviour
    {
        [Header("Villain Style")]
        [SerializeField] private MuniVillainStyle style = MuniVillainStyle.AggressiveFighter;

        [Header("Muni Target")]
        [SerializeField] private Transform muniTarget;
        [SerializeField, Min(1f)] private float detectionRange = 12f;
        [SerializeField, Min(0.5f)] private float meleeRange = 2f;
        [SerializeField, Min(0f)] private float turnSpeed = 420f;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float chaseSpeed = 2.8f;
        [SerializeField, Min(0f)] private float fleeSpeed = 4.2f;
        [SerializeField, Min(0.1f)] private float hideDuration = 4f;
        [SerializeField] private Transform[] hidePoints;

        [Header("Combat (Muni is not damaged)")]
        [SerializeField, Min(0.1f)] private float attackCooldown = 1.8f;
        [SerializeField] private UnityEvent onAttack;
        [SerializeField, Range(0f, 1f)] private float bindingChance = 0.15f;
        [SerializeField, Min(0.5f)] private float bindingRange = 3f;
        [SerializeField, Min(0.1f)] private float bindingCooldown = 12f;
        [SerializeField] private UnityEvent onBindingCast;

        [Header("Retreat")]
        [SerializeField, Range(0.05f, 0.95f)] private float fleeAtHealthFraction = 0.35f;
        [SerializeField, Min(0.1f)] private float fallbackFleeSeconds = 2.5f;

        [Header("Runtime (read only)")]
        [SerializeField] private MuniVillainState currentState = MuniVillainState.Idle;

        private MuniHealth ownHealth;
        private MuniMagicBinding targetBinding;
        private float nextAttackTime;
        private float nextBindingTime;
        private float hideUntil;
        private float fallbackFleeUntil;
        private int currentHidePoint = -1;

        public MuniVillainState CurrentState => currentState;

        private void Awake()
        {
            ownHealth = GetComponent<MuniHealth>();
            CacheTargetComponents();
        }

        private void Update()
        {
            if (ownHealth == null || ownHealth.IsDefeated)
                return;

            if (muniTarget == null)
            {
                currentState = MuniVillainState.Idle;
                return;
            }

            if (targetBinding == null)
                CacheTargetComponents();

            float distance = Vector3.Distance(FlatPosition(transform.position), FlatPosition(muniTarget.position));

            if (currentState == MuniVillainState.Hide)
            {
                if (Time.time >= hideUntil)
                    currentState = MuniVillainState.Chase;
                else
                    return;
            }

            if (ShouldRetreat())
            {
                BeginFlee();
            }

            if (currentState == MuniVillainState.Flee)
            {
                MoveToHideOrAway();
                return;
            }

            if (distance > detectionRange)
            {
                currentState = MuniVillainState.Idle;
                return;
            }

            FaceTarget();

            if (distance > meleeRange)
            {
                currentState = MuniVillainState.Chase;
                MoveTowards(muniTarget.position, chaseSpeed);
                return;
            }

            currentState = MuniVillainState.Fight;
            TryAttack(distance);
        }

        private void TryAttack(float distance)
        {
            if (Time.time < nextAttackTime)
                return;

            nextAttackTime = Time.time + attackCooldown;
            onAttack?.Invoke();

            // Muni receives no ordinary melee damage. Only the explicit magic-bind
            // status effect can temporarily immobilize him.
            bool canCastBinding = style == MuniVillainStyle.SpellCaster ||
                                  style == MuniVillainStyle.HitAndRun;

            if (canCastBinding && targetBinding != null &&
                !targetBinding.IsBound && distance <= bindingRange &&
                Time.time >= nextBindingTime && Random.value <= bindingChance)
            {
                nextBindingTime = Time.time + bindingCooldown;
                onBindingCast?.Invoke();
                targetBinding.ApplyBinding();
            }

            if (style == MuniVillainStyle.HitAndRun)
            {
                currentState = MuniVillainState.Flee;
                fallbackFleeUntil = Time.time + fallbackFleeSeconds;
            }
        }

        private bool ShouldRetreat()
        {
            if (ownHealth == null || ownHealth.MaxHealth <= 0)
                return false;

            float healthFraction = (float)ownHealth.CurrentHealth / ownHealth.MaxHealth;
            return healthFraction <= fleeAtHealthFraction &&
                   currentState != MuniVillainState.Flee;
        }

        private void BeginFlee()
        {
            currentState = MuniVillainState.Flee;
            currentHidePoint = FindNearestHidePoint();
            fallbackFleeUntil = Time.time + fallbackFleeSeconds;
        }

        private void MoveToHideOrAway()
        {
            if (currentHidePoint >= 0 && hidePoints != null &&
                currentHidePoint < hidePoints.Length && hidePoints[currentHidePoint] != null)
            {
                Transform point = hidePoints[currentHidePoint];
                MoveTowards(point.position, fleeSpeed);

                if (Vector3.Distance(FlatPosition(transform.position), FlatPosition(point.position)) <= 0.5f)
                {
                    currentState = MuniVillainState.Hide;
                    hideUntil = Time.time + hideDuration;
                }
                return;
            }

            Vector3 away = FlatPosition(transform.position) - FlatPosition(muniTarget.position);
            if (away.sqrMagnitude > 0.001f)
            {
                Vector3 destination = transform.position + away.normalized * 5f;
                MoveTowards(destination, fleeSpeed);
            }

            if (Time.time >= fallbackFleeUntil)
                currentState = MuniVillainState.Chase;
        }

        private int FindNearestHidePoint()
        {
            if (hidePoints == null || hidePoints.Length == 0)
                return -1;

            int nearest = -1;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < hidePoints.Length; i++)
            {
                if (hidePoints[i] == null) continue;

                float distance = (FlatPosition(hidePoints[i].position) -
                                  FlatPosition(transform.position)).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = i;
                }
            }

            return nearest;
        }

        private void MoveTowards(Vector3 destination, float speed)
        {
            Vector3 direction = FlatPosition(destination) - FlatPosition(transform.position);
            if (direction.sqrMagnitude <= 0.01f) return;

            direction.Normalize();
            Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);

            transform.position += direction * speed * Time.deltaTime;
        }

        private void FaceTarget()
        {
            Vector3 direction = FlatPosition(muniTarget.position) - FlatPosition(transform.position);
            if (direction.sqrMagnitude <= 0.001f) return;

            Quaternion desiredRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
        }

        private void CacheTargetComponents()
        {
            if (muniTarget == null) return;
            targetBinding = muniTarget.GetComponentInParent<MuniMagicBinding>();
        }

        private static Vector3 FlatPosition(Vector3 position)
        {
            position.y = 0f;
            return position;
        }

        public void SetMuniTarget(Transform target)
        {
            muniTarget = target;
            targetBinding = null;
            CacheTargetComponents();
        }

        public void ForceFlee()
        {
            BeginFlee();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeRange);

            if (hidePoints == null) return;
            Gizmos.color = Color.green;
            foreach (Transform point in hidePoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(point.position, 0.35f);
            }
        }
    }
}
