using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        // �������� ��������� NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // ���������, ��� NavMeshAgent ������� � ��������� �� NavMesh
        if (agent == null || !agent.isActiveAndEnabled)
        {
            Debug.LogWarning("NavMeshAgent is missing or not active on unit.");
        }
    }

    public void MoveTo(Vector3 position)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            // ���������, �������� �� ������� ������� �� NavMesh
            if (NavMesh.SamplePosition(position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                // ������������� ������� ������� ��� NavMeshAgent
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