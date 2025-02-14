using UnityEngine;
using UnityEngine.AI; // Не забудьте добавить это пространство имен для работы с NavMeshAgent

public class UnitMovement : MonoBehaviour
{
    public float stopDistance = 0.5f; // Расстояние, на котором юнит остановится перед целью

    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // Получаем компонент NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Настраиваем остановочное расстояние
        agent.stoppingDistance = stopDistance;
    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;

        // Устанавливаем цель для NavMeshAgent
        agent.SetDestination(targetPosition);
    }

    void Update()
    {
        if (isMoving)
        {
            // Проверка достижения цели
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoving = false;
            }
        }
    }
}