using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour
{
    [SerializeField] private float _speed = 7.5f;
    [SerializeField] private GameObject SelectionManager;
    private SelectionManager selectionManager;
    private int _index = 0;
    private Vector3 _position;
    public float mouseSensitivity = 100f; // Чувствительность мыши
    private float xRotation = 44f; // Текущий угол поворота по оси X
    private float yRotation = 0f;
    private float speedX = 5f;
    private float speedZ = 5f;

    [SerializeField] private float edgeThreshold = 10f; // Расстояние от края экрана, при котором камера начинает двигаться

    void Start()
    {
        selectionManager = SelectionManager.GetComponent<SelectionManager>();
    }

    void Update()
    {
        MovementCameraWithMouse();
        SpeedPlus();
        MoveCameraWithMouseAtEdge();
    }

    private void RotationXZ()
    {
            speedX = (xRotation / 9);
            speedZ = 10f - speedX;
    }


    private void SpeedPlus()
    {

            if (Input.GetMouseButtonDown(1))
            {
                _index = 1;
                _speed = 25f;
            Cursor.lockState = CursorLockMode.Confined;
        }
            if (Input.GetMouseButtonUp(1))
            {
                _index = 0;
                _speed = 7.5f;
                Cursor.lockState = CursorLockMode.None;
            }
        
    }

    private void MovementCameraWithMouse()
    {
        if (_index == 1)
        {
            // Получаем ввод мыши
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 10f;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 10f;

            // Поворачиваем камеру по вертикали (вверх/вниз)
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, 0f, 90f); // Ограничиваем угол поворота, чтобы камера не переворачивалась

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    private void MoveCameraWithMouseAtEdge()
    {
        // Получаем позицию курсора на экране
        Vector3 mousePosition = Input.mousePosition;
        float scrollDelta = Input.mouseScrollDelta.y;
        RotationXZ();

        // Проверяем, находится ли курсор близко к краям экрана
        if (mousePosition.x <= edgeThreshold)
        {
            // Двигаем камеру влево
            transform.Translate(Vector3.left * _speed * 10f * Time.deltaTime);
        }
        else if (mousePosition.x >= Screen.width - edgeThreshold)
        {
            // Двигаем камеру вправо
            transform.Translate(Vector3.right * _speed * 10f * Time.deltaTime);
        }
        if (mousePosition.y <= edgeThreshold)
        {
            // Двигаем камеру вниз
            transform.Translate(Vector3.down * _speed * speedX * Time.deltaTime);
            transform.Translate(Vector3.back * _speed * speedZ * Time.deltaTime);
        }
        else if (mousePosition.y >= Screen.height - edgeThreshold)
        {
            // Двигаем камеру вверх
            transform.Translate(Vector3.up * _speed * speedX * Time.deltaTime);
            transform.Translate(Vector3.forward * _speed * speedZ * Time.deltaTime);
        }

        if (scrollDelta != 0) {
            if (scrollDelta < 0)
            {
                // Двигаем камеру вниз
                transform.Translate(Vector3.back * _speed * 50f * Time.deltaTime);
            }
            else if (scrollDelta > 0)
            {
                // Двигаем камеру вверх
                transform.Translate(Vector3.forward * _speed * 50f * Time.deltaTime);
            }
        }
    }
}