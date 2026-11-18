using System.Collections;
using UnityEngine;
using NaughtyCharacter;

namespace MuniGame
{
    /// <summary>
    /// Temporarily binds Muni with a green-rope visual and freezes the player
    /// for the configured duration. A villain spell can call ApplyBinding().
    /// </summary>
    public class MuniMagicBinding : MonoBehaviour
    {
        [Header("Binding")]
        [SerializeField, Min(0.1f)] private float bindingDuration = 10f;
        [SerializeField] private GameObject greenRopeVisualPrefab;
        [SerializeField] private Transform ropeVisualAnchor;
        [SerializeField] private bool hideRopeWhenReleased = true;

        [Header("Player Freeze")]
        [SerializeField] private Character muniCharacter;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private MonoBehaviour[] additionalControlsToDisable;

        [Header("Optional Animation")]
        [SerializeField] private Animator muniAnimator;
        [SerializeField] private string boundBoolParameter = "IsBound";

        private GameObject activeRopeVisual;
        private Coroutine bindingRoutine;
        private bool characterWasEnabled;
        private bool controllerWasEnabled;
        private bool[] additionalControlsWereEnabled;
        private bool isBound;

        public bool IsBound => isBound;
        public float BindingDuration => bindingDuration;

        private void Awake()
        {
            if (muniCharacter == null)
                muniCharacter = GetComponent<Character>();

            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (muniAnimator == null)
                muniAnimator = GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// Call this from the villain's magic-attack UnityEvent.
        /// Recasting while bound restarts the full 10-second timer.
        /// </summary>
        public void ApplyBinding()
        {
            if (bindingRoutine != null)
                StopCoroutine(bindingRoutine);

            if (!isBound)
                FreezeMuni();

            ShowRopeVisual();
            SetBoundAnimation(true);
            bindingRoutine = StartCoroutine(BindingTimer());
        }

        private void FreezeMuni()
        {
            isBound = true;

            if (muniCharacter != null)
            {
                characterWasEnabled = muniCharacter.enabled;
                muniCharacter.enabled = false;
            }

            if (characterController != null)
            {
                controllerWasEnabled = characterController.enabled;
                characterController.enabled = false;
            }

            if (additionalControlsToDisable != null)
            {
                additionalControlsWereEnabled = new bool[additionalControlsToDisable.Length];
                for (int i = 0; i < additionalControlsToDisable.Length; i++)
                {
                    MonoBehaviour control = additionalControlsToDisable[i];
                    if (control == null) continue;

                    additionalControlsWereEnabled[i] = control.enabled;
                    control.enabled = false;
                }
            }
        }

        private IEnumerator BindingTimer()
        {
            yield return new WaitForSeconds(bindingDuration);
            ReleaseBinding();
        }

        /// <summary>
        /// Can also be called by a rescue event or gameplay trigger.
        /// </summary>
        public void ReleaseBinding()
        {
            if (!isBound) return;

            if (muniCharacter != null)
                muniCharacter.enabled = characterWasEnabled;

            if (characterController != null)
                characterController.enabled = controllerWasEnabled;

            if (additionalControlsToDisable != null && additionalControlsWereEnabled != null)
            {
                for (int i = 0; i < additionalControlsToDisable.Length; i++)
                {
                    if (additionalControlsToDisable[i] != null)
                        additionalControlsToDisable[i].enabled = additionalControlsWereEnabled[i];
                }
            }

            if (hideRopeWhenReleased && activeRopeVisual != null)
                Destroy(activeRopeVisual);

            SetBoundAnimation(false);
            isBound = false;
            bindingRoutine = null;
        }

        private void ShowRopeVisual()
        {
            if (greenRopeVisualPrefab == null) return;

            if (activeRopeVisual != null)
                Destroy(activeRopeVisual);

            Transform anchor = ropeVisualAnchor != null ? ropeVisualAnchor : transform;
            activeRopeVisual = Instantiate(greenRopeVisualPrefab, anchor);
            activeRopeVisual.transform.localPosition = Vector3.zero;
            activeRopeVisual.transform.localRotation = Quaternion.identity;
        }

        private void SetBoundAnimation(bool value)
        {
            if (muniAnimator == null || string.IsNullOrWhiteSpace(boundBoolParameter))
                return;

            foreach (AnimatorControllerParameter parameter in muniAnimator.parameters)
            {
                if (parameter.name == boundBoolParameter &&
                    parameter.type == AnimatorControllerParameterType.Bool)
                {
                    muniAnimator.SetBool(boundBoolParameter, value);
                    break;
                }
            }
        }

        private void OnDisable()
        {
            if (bindingRoutine != null)
            {
                StopCoroutine(bindingRoutine);
                bindingRoutine = null;
            }

            if (isBound)
                ReleaseBinding();
        }
    }
}
