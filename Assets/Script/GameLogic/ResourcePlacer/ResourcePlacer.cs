using UnityEngine;

public class ResourcePlacer : MonoBehaviour
{
    public GameObject resourcePrefab; // Префаб ресурса
    public int gridSize = 50; // Размер сетки
    public float minDistanceBetweenResources = 5f; // Минимальное расстояние между ресурсами
    public float noiseScale = 0.1f; // Масштаб шума Перлина
    public float spawnThreshold = 0.6f; // Порог для спавна ресурса
    public Transform playerBase; // База игрока
    public float playerBaseRadius = 10f; // Радиус вокруг базы, где ресурсы не должны спавниться
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

                    // Проверяем, не находится ли позиция слишком близко к базе игрока
                    if (Vector3.Distance(spawnPosition, playerBase.position) > playerBaseRadius)
                    {
                        // Проверяем, нет ли других ресурсов слишком близко
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