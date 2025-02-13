using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour
{
    [SerializeField] private float _speed = 7.5f;
    private int _index = 0;
    private Vector3 _position;
    public float mouseSensitivity = 100f; // ���������������� ����
    private float xRotation = 44f; // ������� ���� �������� �� ��� X
    private float yRotation = 0f;

    void Start()
    {

    }

    void Update()
    {
        MovementCameraWithMouse();
        SpeedPlus();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(_speed * movement * Time.deltaTime);
    }


    private void SpeedPlus()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _index = 1;
            _speed = 25f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
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

            // �������� ���� ����
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 10f;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 10f;

            // ������������ ������ �� ��������� (�����/����)
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ������������ ���� ��������, ����� ������ �� ����������������

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        }

    }



}
