using UnityEngine.AI;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private UnitAI unitAI;
    private SelectionManager selectionManager;
    [SerializeField] private GameObject SelectObject;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitAI = GetComponent<UnitAI>();
        selectionManager = SelectObject.GetComponent<SelectionManager>();
    }

    public void MoveTo(Vector3 position)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {

            // Движение к указанной точке
            if (NavMesh.SamplePosition(position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
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

    public bool HasReachedDestination()
    {
        if (!agent.hasPath || agent.pathPending)
            return false;

        // Оставшееся расстояние <= stoppingDistance + небольшая погрешность
        return agent.remainingDistance <= agent.stoppingDistance + 0.1f;
    }

    private void Update()
    {
        // Перемещение юнитов (ПКМ)
        if (Input.GetMouseButtonDown(1) && selectionManager.selectedUnits.Count > 0)
        {
            unitAI.isManualControl = true;
        }

        if (HasReachedDestination())
        {
            unitAI.isManualControl = false;
        }
    }
}
