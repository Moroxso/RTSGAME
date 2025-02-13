using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player; // ������ �� ������ ��� ������� ������
    public float height = 35f; // ������ ������ ����-�����

    private Camera minimapCamera;

    void Start()
    {
        minimapCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // ������ �� ������� (��� ������ ��������)
        if (player != null)
        {
            Vector3 newPosition = player.position;
            newPosition.y = height; // ������������� ������ ������
            transform.position = newPosition;

            // ������������ ����-�����, ���� ��� ����������
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}
