using UnityEngine.AI;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private UnitAI unitAI;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitAI = GetComponent<UnitAI>();
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
}
