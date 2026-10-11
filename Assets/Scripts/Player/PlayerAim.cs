using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera aimCamera;
    [SerializeField] private Transform aimPivot;
    [SerializeField] private LayerMask groundLayers;

    [Header("Rotation")]
    [SerializeField] private float turnSpeed = 720f;

    private void Awake()
    {
        if (aimCamera == null)
            aimCamera = Camera.main;

        if (aimPivot == null)
            aimPivot = transform;
    }

    private void Update()
    {
        if (aimCamera == null || Mouse.current == null)
            return;

        // Получаем положение курсора на экране.
        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        // Проводим луч от камеры через курсор.
        Ray ray = aimCamera.ScreenPointToRay(mousePosition);

        // Ищем поверхность, на которую можно навестись.
        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                500f,
                groundLayers,
                QueryTriggerInteraction.Ignore))
        {
            return;
        }

        // Направление от персонажа к точке на земле.
        Vector3 direction = hit.point - aimPivot.position;

        // Поворачиваем только вокруг вертикальной оси Y.
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        // Плавно поворачиваем модель.
        aimPivot.rotation = Quaternion.RotateTowards(
            aimPivot.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }
}