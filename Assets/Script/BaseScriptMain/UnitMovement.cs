using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        // Получаем компонент NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Проверяем, что NavMeshAgent активен и находится на NavMesh
        if (agent == null || !agent.isActiveAndEnabled)
        {
            Debug.LogWarning("NavMeshAgent is missing or not active on unit.");
        }
    }

    public void MoveTo(Vector3 position)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            // Проверяем, доступна ли целевая позиция на NavMesh
            if (NavMesh.SamplePosition(position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                // Устанавливаем целевую позицию для NavMeshAgent
                agent.SetDestination(navHit.position);
            }
            else
            {
                Debug.LogWarning("Target position is not reachable on NavMesh.");
            }
        }
        else
        {
            Debug.LogWarning("NavMeshAgent component is missing or not active on unit.");
        }
    }
}