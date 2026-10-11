using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField, Min(0.01f)]
    private float smoothTime = 0.15f;

    private Vector3 offset;
    private Vector3 velocity;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError(
                "CameraFollow: не назначен Target!",
                this
            );

            enabled = false;
            return;
        }

        // Сохраняем начальное положение камеры
        // относительно игрока.
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Желаемое положение камеры.
        Vector3 targetPosition = target.position + offset;

        // Плавно перемещаем камеру за игроком.
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}