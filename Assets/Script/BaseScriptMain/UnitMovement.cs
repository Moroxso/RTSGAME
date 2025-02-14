using UnityEngine;
using UnityEngine.AI; // �� �������� �������� ��� ������������ ���� ��� ������ � NavMeshAgent

public class UnitMovement : MonoBehaviour
{
    public float stopDistance = 0.5f; // ����������, �� ������� ���� ����������� ����� �����

    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // �������� ��������� NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // ����������� ������������ ����������
        agent.stoppingDistance = stopDistance;
    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;

        // ������������� ���� ��� NavMeshAgent
        agent.SetDestination(targetPosition);
    }

    void Update()
    {
        if (isMoving)
        {
            // �������� ���������� ����
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoving = false;
            }
        }
    }
}