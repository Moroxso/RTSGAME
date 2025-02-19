using System;
using TMPro;
using UnityEngine;

public class UnitDo : MonoBehaviour
{
    [SerializeField] public TMP_Text HPbar;
    [SerializeField] public TMP_Text Name;
    [SerializeField] private double hp = 15;
    [SerializeField] private double damage = 0.2;
    [SerializeField] private int id_unit = 1;
    [SerializeField] public GameObject SelectObject;
    [SerializeField] private GameObject HPColorBar;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private int team_id = 1;
    private float lastAttackTime = 0f;
    private string unit_name = string.Empty;
    public bool CanDoDamage = false;
    private UnitDo targetUnit;
    private SelectionManager selectionManager;
    private ObjectLogic objectlogic;

    private void Start()
    {
        selectionManager = SelectObject.GetComponent<SelectionManager>();
        determinant_id();
        HPColorBar.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
    }

    private void determinant_id()
    {
        switch (id_unit)
        {
            case 0:
                unit_name = "�������";
                break;
            case 1:
                unit_name = "������";
                break;
            case 2:
                unit_name = "������";
                break;
            case 3:
                unit_name = "��������";
                break;
            default:
                unit_name = "unit_id";
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            objectlogic = other.GetComponent<ObjectLogic>();

            CanDoDamage = true;
        }

        if (other.gameObject.CompareTag("Unit"))
        {      
            targetUnit = other.GetComponent<UnitDo>();

            if (targetUnit.team_id != team_id)
            {
                CanDoDamage = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            CanDoDamage = false;
        }

        if (other.gameObject.CompareTag("Unit"))
        {
            CanDoDamage = false;
        }
    }

    private void FixedUpdate()
    {
        if (CanDoDamage && targetUnit != null && Time.time - lastAttackTime >= attackInterval)
        {
            targetUnit.TakeDamage(damage);
            Debug.Log(hp);
            Debug.Log(targetUnit.hp);
            lastAttackTime = Time.time; // ��������� ����� ��������� �����
        }

        if (CanDoDamage && objectlogic != null && Time.time - lastAttackTime >= attackInterval)
        {
            objectlogic.TakeDamage(damage);
            lastAttackTime = Time.time; 
        }
    }

    public void TakeDamage(double damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
        if (IsSelected())
        {
            UpdateHPBar();
        }
    }
    void UpdateHPBar()
    {
        HPbar.text = Convert.ToString(this.hp) + "/15";
    }

    void UpdateUnitName()
    {
        Name.text = unit_name;
    }

    private void Die()
    {
        selectionManager.selectedUnits.Remove(this);
        Destroy(gameObject);
        OnDeselect();

    }

    private bool IsSelected()
    {
        return selectionManager.selectedUnits.Contains(this);
    }

    public void OnSelect()
    {
        HPColorBar.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        UpdateHPBar();
        UpdateUnitName();
    }

    public void OnDeselect()
    {
        HPColorBar.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
    }
}