using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    private List<GameObject> selectedUnits = new List<GameObject>();
    private bool isSelecting = false;
    private Vector3 mouseStartPosition;

    void Update()
    {
        // Начало выделения области
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mouseStartPosition = Input.mousePosition;

            // Если не зажата клавиша Ctrl или Shift, снимаем выделение со всех объектов
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAllObjects();
            }
        }

        // Завершение выделения области
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            SelectUnitsInRectangle();
        }

        // Выделение одиночного объекта при клике
        if (Input.GetMouseButtonDown(0) && !isSelecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Если объект уже выделен, снимаем выделение
                if (selectedUnits.Contains(clickedObject))
                {
                    DeselectObject(clickedObject);
                    selectedUnits.Remove(clickedObject);
                }
                else
                {
                        // Добавляем объект в список выделенных
                        SelectObject(clickedObject);
                    selectedUnits.Add(clickedObject);                  
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                float radius = 2f;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    float angle = i * (1.5f * Mathf.PI / selectedUnits.Count);
                    Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                    selectedUnits[i].GetComponent<UnitMovement>().MoveTo(hit.point + offset);
                }
            }
        }

    }

        void SelectUnitsInRectangle()
    {
        Vector3 mouseEndPosition = Input.mousePosition;
        Rect selectionRect = new Rect(mouseStartPosition.x, Screen.height - mouseStartPosition.y,
                                      mouseEndPosition.x - mouseStartPosition.x,
                                      -(mouseEndPosition.y - mouseStartPosition.y));

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<Renderer>() != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
                if (selectionRect.Contains(screenPos, true))
                {
                    if (!selectedUnits.Contains(obj))
                    {
                        if (obj.gameObject.CompareTag("Unit"))
                        {
                            SelectObject(obj);
                            selectedUnits.Add(obj);
                        }
                    }
                }
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        // Пример изменения цвета объекта при выделении
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
    }

    void DeselectObject(GameObject obj)
    {
        // Возвращаем объекту исходный цвет
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    void DeselectAllObjects()
    {
        foreach (GameObject obj in selectedUnits)
        {
            DeselectObject(obj);
        }
        selectedUnits.Clear();
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Рисуем прямоугольник выделения
            Rect rect = GetScreenRect(mouseStartPosition, Input.mousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Переворачиваем Y координаты
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        // Вычисляем углы прямоугольника
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Верхняя линия
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Нижняя линия
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        // Левая линия
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Правая линия
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }
}