using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stopDistance = 0.5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            // Поворот к цели
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Движение
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Проверка достижения цели
            if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
            {
                isMoving = false;
            }
        }
    }
}