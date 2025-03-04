using System.Resources;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public GameObject buildingPrefab; // ������ ������, ������� ����� ���������
    public Material greenMaterial; // �������� ��� ����������� �������
    public Material redMaterial; // �������� ��� ����������� �������
    private GameObject previewBuilding; // ������ ����������� ������
    private bool canPlaceBuilding = false; // ���� ��� �������� ����������� ���������

    private bool buttonif = false;
    // ������ �� ���������� ��� �������
    // ��������� ���������

    public void ButtonDo()
    {
        buttonif = true;
    }



    void Update()
    {
        if (buttonif == true)
        {
            // ���������� ������ ������ � ��������� �������
            MovePreviewBuilding();

            // ���� ������ ������, �������� ���������
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

    // ����� ��� �������� ������ ������
    private void MovePreviewBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ��� �� ������ � ������� �������
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 placementPosition = hit.point; // �����, ���� ������� ���


            if (previewBuilding == null)
            {
                StartBuildingPlacement();
            }

            previewBuilding.transform.position = new Vector3(placementPosition.x, 0.2f, placementPosition.z); // ��������� ������ �� �����

            // ���������, ����� �� �� ��������� ������
            CheckPlacementValidity(placementPosition);
        }
    }

    // ����� ��� �������� ����������� ���������
    private void CheckPlacementValidity(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 5f); // �������� �� ����������� � ������� 1 ����
        canPlaceBuilding = true;

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Structure") || collider.gameObject.CompareTag("Unit")) // ���� ������������ � ������ �������
            {
                canPlaceBuilding = false;
                break;
            }
        }

        // ������ �������� �� ������, ���� ����� ���������, ��� �� �������, ���� ������
        previewBuilding.GetComponent<Renderer>().material = canPlaceBuilding ? greenMaterial : redMaterial;
    }

    // ����� ��� ���������� ������
    private void TryPlaceBuilding()
    {
        // ���������, ����� �� ��������� (���� �� ������� � �������� �� ���������)
        if (canPlaceBuilding)
        {
            // ��������� �������


            // ������ ������
            Instantiate(buildingPrefab, previewBuilding.transform.position, Quaternion.identity);
            Destroy(previewBuilding);
            buttonif = false;
        }
    }

    // ���������� ��� ������ �������������, ����� ������� ������
    public void StartBuildingPlacement()
    {
        previewBuilding = Instantiate(buildingPrefab); // ������� ������
        previewBuilding.GetComponent<Renderer>().material = greenMaterial; // ��������� ���� - �������
        previewBuilding.GetComponent<Collider>().enabled = false; // ��������� ��������� ��� ������
    }

    // ����� ��� ��������� ������������� (����, ��������, ����� �������� �������������)
    public void CancelBuildingPlacement()
    {
        Destroy(previewBuilding); // ������� ������
        buttonif = false;
    }
}
