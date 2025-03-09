using UnityEngine;

public class ResourcePlacer : MonoBehaviour
{
    public GameObject resourcePrefab; // ������ �������
    public int gridSize = 50; // ������ �����
    public float minDistanceBetweenResources = 5f; // ����������� ���������� ����� ���������
    public float noiseScale = 0.1f; // ������� ���� �������
    public float spawnThreshold = 0.6f; // ����� ��� ������ �������
    public Transform playerBase; // ���� ������
    public float playerBaseRadius = 10f; // ������ ������ ����, ��� ������� �� ������ ����������
    public Transform miscParent;

    void Start()
    {
        PlaceResources();
    }

    void PlaceResources()
    {
        for (int x = gridSize * -1; x < gridSize; x++)
        {
            for (int z = gridSize * -1; z < gridSize; z++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, z * noiseScale);

                if (noiseValue > spawnThreshold)
                {
                    Vector3 spawnPosition = new Vector3(x, 0.95f, z);

                    // ���������, �� ��������� �� ������� ������� ������ � ���� ������
                    if (Vector3.Distance(spawnPosition, playerBase.position) > playerBaseRadius)
                    {
                        // ���������, ��� �� ������ �������� ������� ������
                        if (!IsTooCloseToOtherResources(spawnPosition))
                        {

                            GameObject newObject = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity, miscParent);
                            newObject.name = resourcePrefab.name;
                        }
                    }
                }
            }
        }
    }

    bool IsTooCloseToOtherResources(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, minDistanceBetweenResources);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Tree"))
            {
                return true;
            }
        }
        return false;
    }
}