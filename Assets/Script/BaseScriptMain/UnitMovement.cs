using UnityEngine.AI;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private UnitDo unit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<UnitDo>();
    }

    public void MoveTo(Vector3 position)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            // Отключаем ИИ и перехватываем управление
            UnitAI unitAI = GetComponent<UnitAI>();
            if (unitAI != null)
            {
                unitAI.ResetTarget();
            }

            // Устанавливаем флаг ручного управления
            unit.isManualControl = true;

            // Движение к указанной точке
            if (NavMesh.SamplePosition(position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }
}