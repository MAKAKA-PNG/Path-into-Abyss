using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Вызывается Input Action с именем Move.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Вызывается Input Action с именем Jump.
    public void OnJump(InputValue value)
    {
        if (!value.isPressed || !controller.isGrounded)
            return;

        verticalVelocity =
            Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void Update()
    {
        // Направления движения относительно камеры.
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        if (cameraTransform != null)
        {
            forward = cameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            right = cameraTransform.right;
            right.y = 0f;
            right.Normalize();
        }

        // WASD преобразуются в направления игрового мира.
        Vector3 movement =
            right * moveInput.x +
            forward * moveInput.y;

        // Не даём диагональному движению быть быстрее.
        movement = Vector3.ClampMagnitude(movement, 1f);
        movement *= moveSpeed;

        // Гравитация и прыжок.
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }
}