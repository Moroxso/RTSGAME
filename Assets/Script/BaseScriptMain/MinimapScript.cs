using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player; // Ссылка на игрока или главный объект
    public float height = 35f; // Высота камеры мини-карты

    private Camera minimapCamera;

    void Start()
    {
        minimapCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // Следим за игроком (или другим объектом)
        if (player != null)
        {
            Vector3 newPosition = player.position;
            newPosition.y = height; // Устанавливаем высоту камеры
            transform.position = newPosition;

            // Поворачиваем мини-карту, если это необходимо
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}
