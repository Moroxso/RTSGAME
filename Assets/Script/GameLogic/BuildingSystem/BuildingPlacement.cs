using System.Resources;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public GameObject buildingPrefab; // Префаб здания, которое будет строиться
    public Material greenMaterial; // Материал для разрешенной области
    public Material redMaterial; // Материал для запрещенной области
    private GameObject previewBuilding; // Превью строящегося здания
    private bool canPlaceBuilding = false; // Флаг для проверки возможности постройки

    private bool buttonif = false;
    // Ссылки на компоненты для ресурса
    // Стоимость постройки

    public void ButtonDo()
    {
        buttonif = true;
    }



    void Update()
    {
        if (buttonif == true)
        {
            // Перемещаем превью здания в положение курсора
            MovePreviewBuilding();

            // Если нажата кнопка, пытаемся построить
            if (Input.GetMouseButtonDown(0))
            {
                TryPlaceBuilding();
            }
            if (Input.GetMouseButtonDown(1))
            {
                CancelBuildingPlacement();
            }
        }
    }

    // Метод для движения превью здания
    private void MovePreviewBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Луч от камеры к позиции курсора
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 placementPosition = hit.point; // Точка, куда попадет луч


            if (previewBuilding == null)
            {
                StartBuildingPlacement();
            }

            previewBuilding.transform.position = new Vector3(placementPosition.x, 0.2f, placementPosition.z); // Размещаем превью на земле

            // Проверяем, можем ли мы поставить здание
            CheckPlacementValidity(placementPosition);
        }
    }

    // Метод для проверки возможности постройки
    private void CheckPlacementValidity(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 5f); // Проверка на пересечение в радиусе 1 метр
        canPlaceBuilding = true;

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Structure") || collider.gameObject.CompareTag("Unit")) // Если пересекается с другим зданием
            {
                canPlaceBuilding = false;
                break;
            }
        }

        // Меняем материал на зелёный, если можно поставить, или на красный, если нельзя
        previewBuilding.GetComponent<Renderer>().material = canPlaceBuilding ? greenMaterial : redMaterial;
    }

    // Метод для размещения здания
    private void TryPlaceBuilding()
    {
        // Проверяем, можно ли построить (есть ли ресурсы и возможно ли поставить)
        if (canPlaceBuilding)
        {
            // Уменьшаем ресурсы


            // Строим здание
            Instantiate(buildingPrefab, previewBuilding.transform.position, Quaternion.identity);
            Destroy(previewBuilding);
            buttonif = false;
        }
    }

    // Вызывается при начале строительства, чтобы создать превью
    public void StartBuildingPlacement()
    {
        previewBuilding = Instantiate(buildingPrefab); // Создаем превью
        previewBuilding.GetComponent<Renderer>().material = greenMaterial; // Начальный цвет - зеленый
        previewBuilding.GetComponent<Collider>().enabled = false; // Отключаем коллайдер для превью
    }

    // Метод для остановки строительства (если, например, игрок отменяет строительство)
    public void CancelBuildingPlacement()
    {
        Destroy(previewBuilding); // Удаляем превью
        buttonif = false;
    }
}
