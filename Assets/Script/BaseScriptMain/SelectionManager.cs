using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class SelectionManager : MonoBehaviour
{
    public List<UnitDo> selectedUnits = new List<UnitDo>(); // Список компонентов UnitDo
    private bool isSelecting = false;
    private Vector3 mouseStartPosition;
    private bool hasMoved = false; // Флаг для отслеживания движения мыши

    void Update()
    {
        selectedUnits.RemoveAll(obj => obj == null || !obj.gameObject.activeInHierarchy);
        ifSelectButNotCollection();

        // Начало выделения области
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mouseStartPosition = Input.mousePosition;
            hasMoved = false; // Сбрасываем флаг движения мыши

            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAllObjects();
            }
        }

        // Отслеживание движения мыши
        if (isSelecting && Input.GetMouseButton(0))
        {
            if (Vector3.Distance(mouseStartPosition, Input.mousePosition) > 5f) // Порог движения мыши
            {
                hasMoved = true;
            }
        }

        // Завершение выделения области
        if (Input.GetMouseButtonUp(0))
        {
            if (hasMoved)
            {
                SelectUnitsInRectangle();
            }
            else
            {
                // Одиночный клик
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    UnitDo unit = clickedObject.GetComponentInParent<UnitDo>();


                    if (unit != null)
                    {
                        if (selectedUnits.Contains(unit))
                        {
                            DeselectObject(unit.gameObject);
                        }
                        else
                        {
                            SelectObject(unit.gameObject);
                        }
                    }
                }
            }

            isSelecting = false;
        }

        // Перемещение юнитов (ПКМ)
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                float radius = 2f;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] == null || !selectedUnits[i].gameObject.activeInHierarchy)
                    {
                        continue; // Пропускаем неактивные или уничтоженные объекты
                    }

                    NavMeshAgent agent = selectedUnits[i].GetComponent<NavMeshAgent>();
                    if (agent != null && agent.isActiveAndEnabled)
                    {
                        float angle = i * (2f * Mathf.PI / selectedUnits.Count);
                        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                        Vector3 targetPosition = hit.point + offset;

                        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, radius, NavMesh.AllAreas))
                        {
                            agent.SetDestination(navHit.position);
                        }
                    }
                }
            }
        }



    }




    void ifSelectButNotCollection()
    {
        if (selectedUnits != null)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                UnitDo unit = selectedUnits[i].GetComponent<UnitDo>();
                if (unit.Select == false)
                {
                    selectedUnits.Remove(unit);
                }
            }
        }
    }


    void SelectUnitsInRectangle()
    {
        Vector3 mouseEndPosition = Input.mousePosition;
        Rect selectionRect = GetScreenRect(mouseStartPosition, mouseEndPosition);

        foreach (UnitDo unit in FindObjectsOfType<UnitDo>()) // Только юниты
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (selectionRect.Contains(screenPos, true))
            {
                SelectObject(unit.gameObject);
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        UnitDo unit = obj.GetComponent<UnitDo>();
        if (unit != null && !selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.OnSelect(); // Активируем GUI

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null) renderer.material.color = Color.yellow;
        }
    }

    void DeselectObject(GameObject obj)
    {
        UnitDo unit = obj.GetComponent<UnitDo>();
        if (unit != null && selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
            unit.OnDeselect(); // Деактивируем GUI

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null) renderer.material.color = Color.white;
        }
    }

    void DeselectAllObjects()
    {
        foreach (UnitDo unit in selectedUnits)
        {
            unit.OnDeselect();
            Renderer renderer = unit.GetComponent<Renderer>();
            if (renderer != null) renderer.material.color = Color.white;
        }
        selectedUnits.Clear();
    }

    // Методы для отрисовки рамки (остаются без изменений)
    void OnGUI()
    {
        if (isSelecting)
        {
            Rect rect = GetScreenRect(mouseStartPosition, Input.mousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }

        selectedUnits.RemoveAll(obj => obj == null || !obj.gameObject.activeInHierarchy);
    }

    Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
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
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }
}