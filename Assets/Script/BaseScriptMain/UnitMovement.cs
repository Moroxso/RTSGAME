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
            // ��������� �� � ������������� ����������
            UnitAI unitAI = GetComponent<UnitAI>();
            if (unitAI != null)
            {
                unitAI.ResetTarget();
            }

            // ������������� ���� ������� ����������
            unit.isManualControl = true;

            // �������� � ��������� �����
            if (NavMesh.SamplePosition(position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }
}