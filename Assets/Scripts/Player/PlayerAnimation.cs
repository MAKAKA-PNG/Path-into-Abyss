using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] private float movementThreshold = 0.1f;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (animator == null || characterController == null)
        {
            Debug.LogError(
                "PlayerAnimation: проверь Animator и CharacterController.",
                this
            );

            enabled = false;
        }
    }

    private void Update()
    {
        Vector3 velocity = characterController.velocity;
        velocity.y = 0f;

        float speed = velocity.magnitude;

        animator.SetFloat(
            SpeedHash,
            speed,
            0.1f,
            Time.deltaTime
        );
    }

    // PlayerInput вызывает этот метод при действии Attack.
    public void OnAttack(InputValue value)
    {
        if (!value.isPressed)
            return;

        if (!characterController.isGrounded)
            return;

        animator.SetTrigger(AttackHash);
    }
}